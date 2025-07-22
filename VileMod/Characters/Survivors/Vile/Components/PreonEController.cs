using RoR2;
using RoR2.Projectile;
using System;
using System.Linq;
using UnityEngine;
using static UnityEngine.ParticleSystem.PlaybackState;

namespace VileMod.Survivors.Vile.Components
{
    public class PreonEController : MonoBehaviour
    {
        public float lifetime = 10f;
        public float scanRange = 30f;
        public float fireInterval = 0.25f;
        public GameObject projectilePrefab;
        public GameObject projectileGhostGameObj;
        public GameObject projectileGameObject;
        public Transform firePoint;

        private Vector3 direction;
        private Vector3 shootDir;
        private Vector3 originEXE;

        private float fireTimer;

        private Animator animator;

        private ProjectileController projectileController;

        void Awake()
        {
            // Caso o Animator esteja no próprio objeto
            //animator = GetComponent<Animator>();

            // Caso o Animator esteja em um filho, use:
            //animator = GetComponentInChildren<Animator>();

            projectileController = GetComponent<ProjectileController>();
            projectileGameObject = projectileController.gameObject;
            

            Debug.Log($"PreonEController Awake: {gameObject.name}");
            Debug.Log($"projectileController.gameObject: {projectileController.gameObject}");
            Debug.Log($"projectileController.gameObject.name: {projectileController.gameObject.name}");

            

            projectileGhostGameObj = projectileController.ghostPrefab.gameObject;

            Debug.Log($"projectileGhostGameObj.GetComponents<Animator>()[0]: {projectileGhostGameObj.GetComponents<Animator>()[0]}");
            animator = projectileGhostGameObj.GetComponents<Animator>()[0];

            Debug.Log($"firePoint: {FindChildByName(projectileGhostGameObj.transform, "PreonEShootPoint")}");
            firePoint = FindChildByName(projectileGhostGameObj.transform, "PreonEShootPoint");

            Debug.Log($"projectileController.gameObject.pos: {projectileController.gameObject.transform.position}");
            Debug.Log($"firePoint.pos: {firePoint.position}");




            //if (!GetComponent<TeamComponent>())
            //{
            //    var teamComponent = gameObject.AddComponent<TeamComponent>();
            //    teamComponent.teamIndex = TeamIndex.Player; // ou outro time se for necessário
            //}

        }

        void Start()
        {
            Destroy(gameObject, lifetime);
        }

        void FixedUpdate()
        {
            fireTimer -= Time.fixedDeltaTime;

            HurtBox target = FindTarget();

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

                if (fireTimer <= 0f)
                {
                    Fire(target);
                    fireTimer = fireInterval;
                }
            }
        }

        void Fire(HurtBox target)
        {
            if (target)
            {

                animator.Play("Shoot");

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

        Transform FindChildByName(Transform parent, string childName)
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

        HurtBox FindTarget()
        {
            BullseyeSearch search = new BullseyeSearch();
            search.teamMaskFilter = TeamMask.GetUnprotectedTeams(TeamIndex.Player);
            search.maxDistanceFilter = scanRange;
            search.searchOrigin = projectileController.gameObject.transform.position;
            search.sortMode = BullseyeSearch.SortMode.Distance;
            search.filterByLoS = true; // Agora só alvos com linha de visão
            search.RefreshCandidates();

            return search.GetResults().FirstOrDefault();
        }
    }
}