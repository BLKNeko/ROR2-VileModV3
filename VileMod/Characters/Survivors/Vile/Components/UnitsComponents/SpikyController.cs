using RoR2;
using RoR2.Projectile;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace VileMod.Survivors.Vile.Components
{
    public class SpikyController : MonoBehaviour
    {
        public float radius = 5f;
        public float speed = 150f;
        public float duration = 40f;
        public float returnSpeed = 1f;
        public float destroyDistance = 0.5f;
        public float groundOffset = 1f;
        protected float groundCheckDistance = 5f;

        private float timer = 0f;
        private float timeLimit = 0.25f;
        private float damageCoefficient;

        private float angle;
        private float age;
        private bool returning = false;
        private Transform ownerTransform;
        private ProjectileController projectileController;
        private ProjectileOverlapAttack overlapAttack;
        public CharacterBody ownerBody;

        private BlastAttack blastAttack;


        void Start()
        {
            projectileController = GetComponent<ProjectileController>();
            overlapAttack = GetComponent<ProjectileOverlapAttack>();

            ownerBody = projectileController.owner.GetComponent<CharacterBody>();

            speed += ownerBody.attackSpeed * 10f;

            damageCoefficient = VileStaticValues.UnitSpikyDamageCoefficient; // VileStaticValues.VSpikyControllerDamageCoefficient;

            //Debug.Log("ownerBody: " + ownerBody);

            if (projectileController && projectileController.owner)
            {
                ownerTransform = ownerBody.transform;
            }

            if (ownerTransform)
            {
                angle = 0f;
                Vector3 offset = new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)) * radius;
                transform.position = ownerTransform.position + offset;
            }

            AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_TP_In, gameObject);

            EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/TeleportOutBoom"), new EffectData
            {
                origin = transform.position,
                rotation = transform.rotation
            }, transmit: true);

        }

        private void FixedUpdate()
        {


            if (overlapAttack != null)
            {
                timer += Time.deltaTime;

                // Carrega efeito de impacto
                GameObject effectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/ImpactLightning");
                EffectIndex effectIndex = EffectCatalog.FindEffectIndexFromPrefab(effectPrefab);

                BlastAttack CSBlastAttack = new BlastAttack();
                CSBlastAttack.attacker = ownerBody.gameObject;
                CSBlastAttack.inflictor = gameObject;
                CSBlastAttack.teamIndex = TeamComponent.GetObjectTeam(ownerBody.gameObject);
                CSBlastAttack.baseDamage = damageCoefficient * ownerBody.damage;
                CSBlastAttack.baseForce = 50f;
                CSBlastAttack.position = transform.position;
                CSBlastAttack.radius = 5f;
                CSBlastAttack.bonusForce = new Vector3(1f, 1f, 1f);
                CSBlastAttack.damageType = DamageType.Generic;
                CSBlastAttack.damageColorIndex = DamageColorIndex.Default;
                CSBlastAttack.procCoefficient = 1f;
                CSBlastAttack.procChainMask = default;
                CSBlastAttack.crit = ownerBody.RollCrit();
                CSBlastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                CSBlastAttack.impactEffect = effectIndex;


                //Debug.Log("damageCoeficient: " + damageCoeficient);
                //Debug.Log("damageStat: " + damageStat);
                //Debug.Log("blastAttack.baseDamage: " + blastAttack.baseDamage);
                //Debug.Log("projectileController.owner: " + projectileController.owner);
                //Debug.Log("projectileController.owner.GetComponent<CharacterBody>().damage: " + projectileController.owner.GetComponent<CharacterBody>().damage);

                if (timer > timeLimit)
                {
                    //overlapAttack.ResetOverlapAttack();
                    //blastAttack.Fire();
                    BlastAttack.Result result = CSBlastAttack.Fire();
                    timer = 0f;
                }



            }

        }


        void Update()
        {
            if (!ownerTransform) return;

            

            if (!returning)
            {
                // Fase de órbita
                angle += speed * Time.deltaTime;
                float rad = angle * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * radius;
                transform.position = ownerTransform.position + offset;

                StickToGround();

                TurnToPlayer();

                age += Time.deltaTime;
                if (age >= duration)
                {
                    returning = true;
                }
            }
            else
            {
                // Fase de retorno
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    ownerTransform.position,
                    returnSpeed * Time.deltaTime
                );

            }
        }

        private void TurnToPlayer()
        {
            Vector3 direction = (ownerTransform.position - transform.position).normalized;
            Vector3 flatDirection = new Vector3(direction.x, 0, direction.z);
            if (flatDirection.sqrMagnitude > 0f)
            {
                // Rotação base olhando pro dono
                Quaternion targetRotation = Quaternion.LookRotation(flatDirection, Vector3.up);

                // Correção: depende de como seu modelo está mapeado.
                // Se a face (o “plano” da roda) estiver no eixo +Y local, por exemplo:
                Quaternion correction = Quaternion.Euler(90f, 0f, 0f);
                // Ou se estiver em +X: Quaternion.Euler(0f, 0f, 90f); etc.

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation * correction, Time.deltaTime * 3f);
            }
        }


        private void StickToGround()
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

        void OnDestroy()
        {

            AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_TP_Out, gameObject);

            EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/TeleportOutBoom"), new EffectData
            {
                origin = transform.position,
                rotation = transform.rotation
            }, transmit: true);
        }

    }

}