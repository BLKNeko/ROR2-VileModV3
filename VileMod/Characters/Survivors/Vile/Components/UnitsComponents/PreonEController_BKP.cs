using HG;
using RoR2;
using RoR2.Projectile;
using System;
using System.Linq;
using UnityEngine;
using static Rewired.ComponentControls.Effects.RotateAroundAxis;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem.PlaybackState;

namespace VileMod.Characters.Survivors.Vile.Components.UnitsComponents
{
    public class PreonEController_BKP : MonoBehaviour
    {
        private float lifetime = 60f;
        private float scanRange = 30f;
        private float fireInterval = 0.25f;
        private float distance;
        private GameObject projectileOwner;
        private CharacterBody ownerBody;
        private GameObject projectilePrefab;
        private GameObject projectileGhostGameObj;
        private GameObject projectileGameObject;
        private Transform firePoint;
        private Transform bodyTransform;

        private Vector3 direction;
        private Vector3 shootDir;
        private Vector3 originEXE;

        private float fireTimer;

        private Animator animator;


        private float minDistance = 5f;
        private float maxDistance = 15f;
        private float moveSpeed = 10f;

        private bool shouldMoveCloser = false;
        private bool shouldMoveAway = false;

        private ProjectileController projectileController;

        private enum ProjectileState
        {
            Idle,
            Running,
            Shooting
        }

        private ProjectileState currentState = ProjectileState.Idle;

        void Start()
        {
            projectileController = GetComponent<ProjectileController>();
            projectileGameObject = projectileController.gameObject;
            projectileOwner = projectileController.owner;
            ownerBody = projectileController.owner.GetComponent<CharacterBody>();

            moveSpeed += ownerBody.moveSpeed * 1.5f;

            //Debug.Log("ownerBody: " + ownerBody);

            if (projectileController && projectileController.owner)
            {
                bodyTransform = ownerBody.transform;
            }

            //Debug.Log($"PreonEController Awake: {gameObject.name}");
            //Debug.Log($"projectileController.gameObject: {projectileController.gameObject}");
            //Debug.Log($"projectileController.gameObject.name: {projectileController.gameObject.name}");

            Destroy(gameObject, lifetime);
        }

        private void FixedUpdate()
        {
            fireTimer -= Time.fixedDeltaTime;

            if (!projectileGhostGameObj)
            {
                if (projectileController.ghost.gameObject)
                {
                    projectileGhostGameObj = projectileController.ghost.gameObject;
                    //Debug.Log($"projectileController.ghost.gameObject: {projectileController.ghost.gameObject}");

                }
            }

            if (projectileGhostGameObj && !animator)
            {
                if (projectileController.ghost.gameObject.GetComponent<Animator>() != null)
                {
                    animator = projectileController.ghost.gameObject.GetComponent<Animator>();
                    //Debug.Log($"projectileController.ghost.gameObject.GetComponent<Animator>(): {projectileController.ghost.gameObject.GetComponent<Animator>()}");
                }
            }

            if (!animator && !projectileGhostGameObj)
            {
                //Debug.LogWarning("Animator not found on projectileGhostGameObj.");
                return;
            }

            firePoint = FindChildByName(projectileGhostGameObj.transform, "PreonEShootPoint");

            HurtBox target = FindTarget();

            //Debug.Log("distance: " + distance);
            MoveProjectile();
            UpdateAnims();

            if (target)
            {
                var characterBody = target.healthComponent?.body;
                Vector3 targetPosition;

                if (characterBody != null)
                {
                    targetPosition = characterBody.corePosition;
                }
                else
                {
                    // Fallback para o centro do collider da hitbox
                    targetPosition = target.transform.position + target.collider.bounds.center - target.collider.transform.position;
                }

                shootDir = (targetPosition - projectileController.transform.position).normalized;
                direction = new Vector3(shootDir.x, 0, shootDir.z); // Só gira no eixo Y

                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
                }

                if (fireTimer <= 0f && distance < maxDistance)
                {
                    currentState = ProjectileState.Shooting;
                    Fire(target);
                    fireTimer = fireInterval;
                }
            }
        }

        private void Fire(HurtBox target)
        {
            if (target)
            {

                //animator.Play("Shoot");

                new BulletAttack
                {
                    bulletCount = 1,
                    aimVector = shootDir,
                    origin = firePoint.position,
                    damage = 5f,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BulletAttack.FalloffModel.None,
                    maxDistance = 1000f,
                    force = 800f,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    minSpread = 0f,
                    maxSpread = 0f,
                    isCrit = false,
                    owner = gameObject,
                    smartCollision = true,
                    procChainMask = default,
                    procCoefficient = 1f,
                    radius = 0.75f,
                    sniper = false,
                    stopperMask = LayerIndex.CommonMasks.bullet,
                    weapon = null,
                    tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/tracers/TracerBanditShotgun"),
                    spreadPitchScale = 1f,
                    spreadYawScale = 1f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FireShotgun.hitEffectPrefab,
                }.Fire();
            }
        }

        private void UpdateAnims()
        {
            if (currentState == ProjectileState.Shooting && animator.GetCurrentStateName(0) != "Shoot")
                animator.Play("Shoot");

            if (currentState == ProjectileState.Running && animator.GetCurrentStateName(0) != "Run")
                animator.Play("Run");

            if (currentState == ProjectileState.Idle && animator.GetCurrentStateName(0) != "Idle")
                animator.Play("Idle");
        }

        private void MoveProjectile()
        {
            if (projectileOwner == null) return;

            Vector3 toTarget = bodyTransform.position - transform.position;
            distance = toTarget.magnitude;

            if (distance > maxDistance)
            {
                // Está longe demais, se aproxima
                MoveTowards(bodyTransform.position);
                currentState = ProjectileState.Running;
            }
            else if (distance < minDistance)
            {
                // Está perto demais, se afasta
                MoveAwayFrom(bodyTransform.position);
            }
            else
            {
                // Dentro da distância ideal: opcionalmente, pare ou flutue
                // Aqui você pode rodar idle ou animações, etc.

                if (FindTarget() != null)
                    currentState = ProjectileState.Idle;

            }
        }

        private void CheckIfShouldMove()
        {

            if (distance > maxDistance)
            {
                shouldMoveAway = false;
                shouldMoveCloser = true;
            }

            if (distance <= maxDistance / 2)
            {
                shouldMoveAway = false;
                shouldMoveCloser = false;
            }

            if (distance < minDistance)
            {
                shouldMoveAway = true;
                shouldMoveCloser = false;
            }

            if (distance > minDistance * 1.25f)
            {
                shouldMoveAway = false;
            }

        }

        private void MoveTowards(Vector3 position)
        {
            Vector3 direction = (position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        void MoveAwayFrom(Vector3 position)
        {
            Vector3 direction = (transform.position - position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        private Transform FindChildByName(Transform parent, string childName)
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
            BullseyeSearch search = new BullseyeSearch();
            search.teamMaskFilter = TeamMask.GetUnprotectedTeams(TeamIndex.Player);
            search.maxDistanceFilter = scanRange;
            search.searchOrigin = firePoint.position;
            search.sortMode = BullseyeSearch.SortMode.Distance;
            search.filterByLoS = true; // Agora só alvos com linha de visão
            search.RefreshCandidates();

            return search.GetResults().FirstOrDefault();
        }
    }
}