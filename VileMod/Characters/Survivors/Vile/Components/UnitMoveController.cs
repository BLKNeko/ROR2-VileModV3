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

namespace VileMod.Survivors.Vile.Components
{
    public class UnitMoveController : MonoBehaviour
    {
        [Header("Movimentação")]
        public float moveSpeed = 5f;
        public float minDistance = 1f;
        public float maxDistance = 40f;
        public float frontSafeDistance = 15f;
        public float backToDistance = 30f;
        public float rotationSpeed = 5f;

        [Header("Alvo")]
        public Transform target;
        

        [Header("Detecção de inimigos")]
        protected float enemyCheckRadius = 10f;
        public LayerMask enemyLayer;
        protected Transform firePoint;
        public HurtBox enemyHurtbox;
        public Vector3 shootDir;

        [Header("Ataque")]
        protected float FireCooldown = 0.5f; // Tempo de recarga entre disparos
        protected float FireTimer; // Tempo de recarga entre disparos
        protected float damageCoeficient;

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

        }

        public virtual void FixedUpdate()
        {
            if (target == null) return;

            float distance = Vector3.Distance(transform.position, target.position);

            // Atualiza as animações
            UpdateAnims();

            // Verifica se há inimigos próximos
            //bool hasNearbyEnemies = CheckForEnemiesNearby();
            enemyHurtbox = FindTarget();

            // Atira se houver inimigos
            if (enemyHurtbox)
            {
                SetState(false, false, true); // Shooting
                
                shootDir = (enemyHurtbox.healthComponent.body.corePosition - projectileController.transform.position).normalized;

                FireTimer -= Time.fixedDeltaTime;

                if (FireTimer <= 0f)
                {
                    lookDirection = new Vector3(shootDir.x, 0, shootDir.z); // Só gira no eixo Y

                    if (lookDirection != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
                    }

                    FireAttack();
                    FireTimer = FireCooldown; // Reinicia o tempo de recarga
                }


                return;
            }

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

        bool CheckForEnemiesNearby()
        {
            if (ownerBody == null || !ownerBody.teamComponent) return false;

            TeamIndex myTeam = ownerBody.teamComponent.teamIndex;

            var enemies = TeamComponent.GetTeamMembers(TeamIndex.Monster)
                .Where(tc => tc.teamIndex != myTeam)
                .Where(tc => Vector3.Distance(tc.transform.position, transform.position) <= enemyCheckRadius);

            return enemies.Any();
        }

        private void UpdateAnims()
        {
            if (IsShooting && animator.GetCurrentStateName(0) != "Shoot")
                animator.Play("Shoot");

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
            if(firePoint == null)
            {
                Debug.LogWarning("FirePoint is not assigned.");
                return null;
            }

            BullseyeSearch search = new BullseyeSearch();
            search.teamMaskFilter = TeamMask.GetUnprotectedTeams(TeamIndex.Player);
            search.maxDistanceFilter = enemyCheckRadius;
            search.searchOrigin = firePoint.position;
            search.sortMode = BullseyeSearch.SortMode.Distance;
            search.filterByLoS = true; // Agora só alvos com linha de visão
            search.RefreshCandidates();

            return search.GetResults().FirstOrDefault();
        }

        public virtual void FireAttack()
        {
            //Precisa ser sobrescrito com o ataque desejado.
        }

        public void SetState(bool idle, bool run, bool shoot)
        {
            IsIdle = idle;
            IsRunning = run;
            IsShooting = shoot;
        }
    }
}