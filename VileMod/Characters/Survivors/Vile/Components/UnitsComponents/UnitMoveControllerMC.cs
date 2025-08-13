using HG;
using RoR2;
using RoR2.Projectile;
using System;
using System.Linq;
using UnityEngine;
using static Rewired.ComponentControls.Effects.RotateAroundAxis;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem.PlaybackState;
using TMPro;
using UnityEngine.EventSystems;
using VileMod.Survivors.Vile;
using VileMod.Survivors.Vile.Components;
using UnityEngine.Networking;

namespace VileMod.Characters.Survivors.Vile.Components.UnitsComponents
{
    public class UnitMoveControllerMC : NetworkBehaviour
    {
        [Header("Movimentação")]
        public float moveSpeed = 5f;
        public float minDistance = 1f;
        public float maxDistance = 15f;
        public float frontSafeDistance = 5f;
        public float backToDistance = 10f;
        public float rotationSpeed = 5f;
        public float healMaxDistanceFromPlayer = 10f; // nova variável!

        [Header("Alvo")]
        public Transform target;


        [Header("Detecção de inimigos")]
        protected float allyCheckRadius = 10f;
        public LayerMask enemyLayer;
        protected Transform firePoint;
        public CharacterBody allyBody;
        public Vector3 shootDir;

        [Header("Ataque")]
        protected float FireCooldown = 0.5f; // Tempo de recarga entre disparos
        protected float FireTimer; // Tempo de recarga entre disparos
        protected float healCoefficient;
        protected bool isShield = false; // Se o ataque cura escudo ou vida


        public bool IsIdle { get; private set; }
        public bool IsRunning { get; private set; }
        public bool IsShooting { get; private set; }

        private Vector3 direction;
        private Vector3 lookDirection;

        public ProjectileController projectileController;
        public CharacterBody ownerBody;
        private Animator animator;

        protected float groundOffset = 0.2f; // Quanto acima do chão o projetil deve ficar
        protected float groundCheckDistance = 5f; // Distância máxima para verificar o chão

        public virtual void Start()
        {
            projectileController = GetComponent<ProjectileController>();
            ownerBody = projectileController.owner.GetComponent<CharacterBody>();
            animator = projectileController.ghost.gameObject.GetComponent<Animator>();
            target = ownerBody.transform;

            moveSpeed += ownerBody.moveSpeed * 1.5f;
            FireTimer = FireCooldown;

            firePoint = projectileController.ghost.gameObject.transform;
            //Debug.Log($"FirePoint: {firePoint}");

            AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_TP_In, gameObject);

            EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/TeleportOutBoom"), new EffectData
            {
                origin = transform.position,
                rotation = transform.rotation
            }, transmit: true);

        }

        public virtual void FixedUpdate()
        {
            if (target == null) return;

            float distance = Vector3.Distance(transform.position, target.position);

            // Atualiza as animações
            UpdateAnims();

            // Movimento baseado na distância
            if (distance < minDistance)
            {
                MoveAwayFromTarget();
                SetState(false, true, false); // Run
            }
            else if (distance > maxDistance)
            {
                MoveTowardsTarget(backToDistance);
                SetState(false, true, false); // Run
            }
            else if (distance > frontSafeDistance)
            {
                MoveTowardsTarget(frontSafeDistance);
                SetState(false, true, false); // Run
            }
            else
            {
                SetState(true, false, false); // Idle
            }

            StickToGround();

            Vector3 flatDirection = new Vector3(direction.x, 0, direction.z);
            if (flatDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
            }

            // TENTA CURAR DEPOIS DE AJUSTAR POSIÇÃO
            allyBody = FindAllyToHeal();

            if (allyBody && Vector3.Distance(transform.position, target.position) <= healMaxDistanceFromPlayer)
            {
                SetState(false, false, true); // Shooting

                shootDir = (allyBody.healthComponent.body.corePosition - projectileController.transform.position).normalized;

                FireTimer -= Time.fixedDeltaTime;

                if (FireTimer <= 0f)
                {
                    lookDirection = new Vector3(shootDir.x, 0, shootDir.z); // Só gira no eixo Y

                    if (lookDirection != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
                    }

                    animator.Play("Shoot", 0, 0f);
                    StartHeal(allyBody);
                    FireTimer = FireCooldown; // Reinicia o tempo de recarga
                }
            }
        }

        private void MoveTowardsTarget(float targetDistance)
        {
            direction = (target.position - transform.position).normalized;
            Vector3 moveTarget = target.position - direction * targetDistance;

            transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
        }

        private void MoveAwayFromTarget()
        {
            direction = (transform.position - target.position).normalized;
            Vector3 moveTarget = transform.position + direction;

            transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
        }

        void StickToGround()
        {
            RaycastHit hit;
            Vector3 rayOrigin = transform.position + Vector3.up; // Garante que o raycast venha de cima

            if (Physics.Raycast(rayOrigin, Vector3.down, out hit, groundCheckDistance, LayerMask.GetMask("World")))
            {
                Vector3 pos = transform.position;
                pos.y = hit.point.y + groundOffset;
                transform.position = pos;
            }
        }

        private void UpdateAnims()
        {
            //if (IsShooting && animator.GetCurrentStateName(0) != "Shoot")
            //    animator.Play("Shoot");

            if (IsRunning && animator.GetCurrentStateName(0) != "Run")
                animator.Play("Run");

            if (IsIdle && animator.GetCurrentStateName(0) != "Idle")
                animator.Play("Idle");
        }

        public Transform FindChildByName(Transform parent, string childName)
        {
            foreach (Transform child in parent)
            {
                if (child.name == childName)
                    return child;

                Transform result = FindChildByName(child, childName);
                if (result != null)
                    return result;
            }
            return null;
        }

        private CharacterBody FindAllyToHeal()
        {
            if (firePoint == null)
            {
                //Debug.LogWarning("FirePoint is not assigned.");
                return null;
            }

            TeamMask allyMask = new TeamMask();
            allyMask.AddTeam(TeamComponent.GetObjectTeam(projectileController.owner)); // Adiciona o time dos jogadores

            BullseyeSearch search = new BullseyeSearch
            {
                teamMaskFilter = allyMask, // <- Buscar aliados
                maxDistanceFilter = allyCheckRadius,
                searchOrigin = firePoint.position,
                sortMode = BullseyeSearch.SortMode.Distance,
                filterByLoS = false, // Pode curar mesmo sem linha de visão, se quiser
                viewer = null // Pode passar o body do dono se quiser evitar auto-curar
            };

            search.RefreshCandidates();

            foreach (HurtBox hb in search.GetResults())
            {
                if (!hb || !hb.healthComponent) continue;

                CharacterBody body = hb.healthComponent.body;
                //Debug.Log($"Body: {body}");

                if (!isShield)
                {
                    if (body && body.healthComponent.alive && body.healthComponent.health < body.healthComponent.fullHealth)
                    {
                        return body;
                    }

                    if (body.HasBuff(VileBuffs.RideArmorEnabledBuff))
                    {

                        //Debug.Log($"Body RideArmorComp: {body.GetComponent<VileRideArmorComponent>()}");

                        if (body.GetComponent<VileRideArmorComponent>())
                        {
                            // Se o corpo tem RideArmor, repara o RideArmor
                            if (!body.GetComponent<VileRideArmorComponent>().IsRideArmorFullHealth())
                                return body;
                        }

                    }
                }
                else
                {
                    if (body && body.healthComponent.alive && body.healthComponent.shield < body.healthComponent.fullShield)
                    {
                        return body;
                    }

                    if (body.HasBuff(VileBuffs.RideArmorEnabledBuff))
                    {

                        //Debug.Log($"Body RideArmorComp: {body.GetComponent<VileRideArmorComponent>()}");

                        if (body.GetComponent<VileRideArmorComponent>())
                        {
                            // Se o corpo tem RideArmor, repara o RideArmor
                            if (!body.GetComponent<VileRideArmorComponent>().IsRideArmorFullShield())
                                return body;
                        }

                    }
                }



            }

            return null;
        }

        public virtual void StartHeal(CharacterBody body)
        {
            //To be overrited
        }

        void OnDestroy()
        {

            AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_TP_Out, gameObject);

            EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/TeleportOutBoom"), new EffectData
            {
                origin = transform.position,
                rotation = transform.rotation
            }, transmit: true);
        }

        public void SetState(bool idle, bool run, bool shoot)
        {
            IsIdle = idle;
            IsRunning = run;
            IsShooting = shoot;
        }
    }
}