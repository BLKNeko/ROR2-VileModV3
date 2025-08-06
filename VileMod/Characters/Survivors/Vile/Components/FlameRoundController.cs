using RoR2;
using RoR2.Projectile;
using UnityEngine;
using VileMod.Survivors.Vile;
using static UnityEngine.UI.GridLayoutGroup;

namespace VileMod.Characters.Survivors.Vile.Components
{
    public class FlameRoundController : MonoBehaviour
    {
        public float radius = 5f;
        public float speed = 20f;
        public float duration = 10f;
        public float returnSpeed = 1f;
        public float destroyDistance = 0.5f;
        public float groundOffset = 1f;
        protected float groundCheckDistance = 5f;

        private float timer = 0f;
        private float timeLimit = 0.25f;
        private float damageCoefficient;

        private Vector3 direction;
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

            Ray aimRay = ownerBody.inputBank.GetAimRay();
            direction = aimRay.direction;

            transform.rotation = Quaternion.identity;


        }

        private void FixedUpdate()
        {


            if (overlapAttack != null)
            {
                timer += Time.deltaTime;

                // Carrega efeito de impacto
                GameObject effectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/IgniteExplosionVFX");
                EffectIndex effectIndex = EffectCatalog.FindEffectIndexFromPrefab(effectPrefab);

                BlastAttack CSBlastAttack = new BlastAttack();
                CSBlastAttack.attacker = ownerBody.gameObject;
                CSBlastAttack.inflictor = gameObject;
                CSBlastAttack.teamIndex = TeamComponent.GetObjectTeam(ownerBody.gameObject);
                CSBlastAttack.baseDamage = damageCoefficient * ownerBody.damage;
                CSBlastAttack.baseForce = 50f;
                CSBlastAttack.position = transform.position;
                CSBlastAttack.radius = 6f;
                CSBlastAttack.bonusForce = new Vector3(1f, 1f, 1f);
                CSBlastAttack.damageType = DamageType.IgniteOnHit | DamageTypeCombo.GenericUtility;
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
                    AkSoundEngine.PostEvent(VileStaticValues.Play_Fire_SFX, this.gameObject);
                    BlastAttack.Result result = CSBlastAttack.Fire();
                    timer = 0f;
                }



            }

        }


        void Update()
        {
            if (!ownerTransform) return;

            transform.position += direction.normalized * speed * Time.deltaTime;
            StickToGround();

            transform.rotation = Quaternion.identity;

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