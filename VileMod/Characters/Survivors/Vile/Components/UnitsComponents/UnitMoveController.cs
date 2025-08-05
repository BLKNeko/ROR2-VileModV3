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

namespace VileMod.Characters.Survivors.Vile.Components.UnitsComponents
{
    public class UnitMoveController : MonoBehaviour
    {
        [Header("Movimentação")]
        protected float moveSpeed = 5f;
        protected float minDistance = 1f;
        protected float maxDistance = 40f;
        protected float frontSafeDistance = 15f;
        protected float backToDistance = 30f;
        protected float rotationSpeed = 5f;
        protected float enemyReachDistance = 2f;
        protected float procCoefficient = 1f;
        protected float distance;

        protected bool shouldRotateY = false;
        protected bool shouldFollowGroundOffset = false;
        protected bool shouldMoveTowardEnemy = false;
        protected bool shouldAnimate = true;

        [Header("Alvo")]
        public Transform playerTarget;


        [Header("Detecção de inimigos")]
        protected float enemyCheckRadius = 10f;
        public LayerMask enemyLayer;
        protected Transform firePoint;
        public HurtBox enemyHurtbox;
        public Vector3 shootDir;
        private float enemyDistance;


        [Header("Ataque")]
        protected float FireCooldown = 0.5f; // Tempo de recarga entre disparos
        protected float FireTimer; // Tempo de recarga entre disparos
        protected float damageCoefficient;
        protected bool shouldFilterByLOS = true; // Se deve filtrar por linha de visão

        public bool IsIdle { get; private set; }
        public bool IsRunning { get; private set; }
        public bool IsShooting { get; private set; }

        private Vector3 direction;
        private Vector3 lookDirection;

        public ProjectileOverlapAttack overlapAttack;
        public ProjectileController projectileController;
        public CharacterBody ownerBody;
        private Animator animator;

        protected float groundOffset = 0.2f; // Quanto acima do chão o projetil deve ficar
        protected float groundCheckDistance = 5f; // Distância máxima para verificar o chão

        public virtual void Start()
        {
            projectileController = GetComponent<ProjectileController>();
            overlapAttack = GetComponent<ProjectileOverlapAttack>();
            ownerBody = projectileController.owner.GetComponent<CharacterBody>();
            animator = projectileController.ghost.gameObject.GetComponent<Animator>();
            playerTarget = ownerBody.transform;

            moveSpeed += ownerBody.moveSpeed * 1.5f;
            FireTimer = FireCooldown;

            AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_TP_In, gameObject);

            EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/TeleportOutBoom"), new EffectData
            {
                origin = transform.position,
                rotation = transform.rotation
            }, transmit: true);

            StickToGround();

        }

        public virtual void FixedUpdate()
        {
            if (playerTarget == null) return;

            if (!shouldFollowGroundOffset)
            {
                distance = Vector3.Distance(transform.position, playerTarget.position);
            }
            else
            {
                Vector3 Target2 = playerTarget.position;
                Target2.y += groundOffset; // Adiciona o offset do chão ao alvo
                distance = Vector3.Distance(transform.position, Target2);
            }

            // Atualiza as animações
            if (shouldAnimate)
                UpdateAnims();

            // Verifica se há inimigos próximos
            //bool hasNearbyEnemies = CheckForEnemiesNearby();
            enemyHurtbox = FindTarget();

            // Atira se houver inimigos
            if (enemyHurtbox)
            {
                //Moved to FIRE
                //SetState(false, false, true); // Shooting

                shootDir = (enemyHurtbox.healthComponent.body.corePosition - projectileController.transform.position).normalized;



                enemyDistance = Vector3.Distance(transform.position, enemyHurtbox.healthComponent.body.corePosition);

                if (!shouldMoveTowardEnemy)
                {
                    FireTimer -= Time.fixedDeltaTime;

                    if (FireTimer <= 0f)
                    {

                        LookTowardsEnemy();
                        animator.Play("Shoot", 0, 0f);
                        FireAttack();
                        FireTimer = FireCooldown; // Reinicia o tempo de recarga
                    }
                }
                else
                {
                    FireTimer -= Time.fixedDeltaTime;

                    if (enemyDistance > enemyReachDistance)
                    {
                        LookTowardsEnemy();
                        MoveTowardsEnemy(enemyDistance, enemyHurtbox.healthComponent.body.transform);
                    }
                    else
                    {


                        if (FireTimer <= 0f)
                        {

                            LookTowardsEnemy();
                            animator.Play("Shoot", 0, 0f);
                            FireAttack();
                            FireTimer = FireCooldown; // Reinicia o tempo de recarga
                        }
                    }
                }




                return;
            }

            if (!shouldMoveTowardEnemy)
                BaseMovment();
            else
                BaseMeeleeMovment();

            StickToGround();

            LookTowardsTarget();

        }

        public virtual void BaseMovment()
        {
            // Movimento baseado na distância
            if (distance < minDistance)
            {
                //Debug.Log($"Distance: {distance} < MinDistance: {minDistance}");
                MoveAwayFromTarget();
                SetState(false, true, false); // Run
            }
            else if (distance > maxDistance)
            {
                //Debug.Log($"Distance: {distance} > MaxDistance: {maxDistance}");
                MoveTowardsTarget(backToDistance);
                SetState(false, true, false); // Run
            }
            else if (distance > frontSafeDistance)
            {
                //Debug.Log($"Distance: {distance} > FrontSafeDistance: {frontSafeDistance}");
                MoveTowardsTarget(frontSafeDistance);
                SetState(false, true, false); // Run
            }
            else
            {
                //Debug.Log($"Distance: {distance} is within range.");
                SetState(true, false, false); // Idle
            }
        }

        public virtual void BaseMeeleeMovment()
        {
            if (enemyHurtbox != null) return;

            // Movimento baseado na distância
            if (distance < minDistance)
            {
                //Debug.Log($"Distance: {distance} < MinDistance: {minDistance}");
                MoveAwayFromTarget();
                SetState(false, true, false); // Run
            }
            else if (distance > maxDistance)
            {
                //Debug.Log($"Distance: {distance} > MaxDistance: {maxDistance}");
                MoveTowardsTarget(backToDistance);
                SetState(false, true, false); // Run
            }
            else if (distance > frontSafeDistance)
            {
                //Debug.Log($"Distance: {distance} > FrontSafeDistance: {frontSafeDistance}");
                MoveTowardsTarget(frontSafeDistance);
                SetState(false, true, false); // Run
            }
            else
            {
                //Debug.Log($"Distance: {distance} is within range.");
                SetState(true, false, false); // Idle
            }
        }

        private void LookTowardsEnemy()
        {

            if (!shouldRotateY)
            {
                lookDirection = new Vector3(shootDir.x, 0, shootDir.z); // Só gira no eixo Y

                if (lookDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
                }
            }
            else
            {
                lookDirection = new Vector3(shootDir.x, shootDir.y, shootDir.z); // Só gira no eixo Y

                if (lookDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
                }
            }

        }

        private void LookTowardsTarget()
        {

            if (!shouldRotateY)
            {
                Vector3 flatDirection = new Vector3(direction.x, 0, direction.z);
                if (flatDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
                }
            }
            else
            {
                Vector3 flatDirection = new Vector3(direction.x, direction.y, direction.z);
                if (flatDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
                }
            }

        }

        private void MoveTowardsEnemy(float enemyDistance, Transform enemyTransform)
        {
            direction = (enemyTransform.position - transform.position).normalized;

            if (enemyDistance > enemyReachDistance)
            {
                Vector3 moveTarget = enemyTransform.position - direction * enemyReachDistance;
                transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
            }
        }

        private void MoveTowardsTarget(float targetDistance)
        {

            if (!shouldFollowGroundOffset)
            {
                direction = (playerTarget.position - transform.position).normalized;
                Vector3 moveTarget = playerTarget.position - direction * targetDistance;

                transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
            }
            else
            {

                // Se deve seguir o offset do chão, calcula a posição alvo com o offset
                playerTarget.position = new Vector3(playerTarget.position.x, playerTarget.position.y + groundOffset, playerTarget.position.z);

                direction = (playerTarget.position - transform.position).normalized;
                Vector3 moveTarget = playerTarget.position - direction * targetDistance;

                transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
            }


        }

        private void MoveAwayFromTarget()
        {

            if (!shouldFollowGroundOffset)
            {
                direction = (transform.position - playerTarget.position).normalized;
                Vector3 moveTarget = transform.position + direction;

                transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
            }
            else
            {

                // Se deve seguir o offset do chão, calcula a posição alvo com o offset
                playerTarget.position = new Vector3(playerTarget.position.x, playerTarget.position.y + groundOffset, playerTarget.position.z);

                direction = (transform.position - playerTarget.position).normalized;
                Vector3 moveTarget = transform.position + direction;

                transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
            }

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
            //if (IsShooting)
            //    animator.Play("Shoot", 0, 0f);

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

        private HurtBox FindTarget()
        {
            if (firePoint == null)
            {
                Debug.LogWarning("FirePoint is not assigned.");
                return null;
            }

            BullseyeSearch search = new BullseyeSearch();
            search.teamMaskFilter = TeamMask.GetUnprotectedTeams(TeamIndex.Player);
            search.maxDistanceFilter = enemyCheckRadius;
            search.searchOrigin = firePoint.position;
            search.sortMode = BullseyeSearch.SortMode.Distance;
            search.filterByLoS = shouldFilterByLOS; // Agora só alvos com linha de visão
            search.RefreshCandidates();

            return search.GetResults().FirstOrDefault();
        }

        public virtual void FireAttack()
        {
            //Precisa ser sobrescrito com o ataque desejado.
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