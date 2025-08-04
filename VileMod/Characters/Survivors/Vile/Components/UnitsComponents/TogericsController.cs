using RoR2;
using System.Collections;
using UnityEngine;
using BepInEx;
using RoR2.Projectile;
using VileMod.Modules;
using VileMod.Survivors.Vile;
using R2API;
using VileMod.Survivors.Vile.Components;

namespace VileMod.Characters.Survivors.Vile.Components.UnitsComponents
{
    public class TogericsController : MonoBehaviour
    {
        private ProjectileOverlapAttack overlapAttack;

        private ProjectileImpactExplosion impactExplosion;

        private ProjectileController projectileController;

        private CharacterBody ownerBody;

        private float timer = 0f;
        private float timeLimit = 0.8f;
        private float damageCoefficient;
        private float damagebonus = 1f;

        private BlastAttack blastAttack;

        void Start()
        {
            impactExplosion = GetComponent<ProjectileImpactExplosion>();
            overlapAttack = GetComponent<ProjectileOverlapAttack>();
            projectileController = GetComponent<ProjectileController>();
            damageCoefficient = VileStaticValues.UnitTogericsDamageCoefficient;

            ownerBody = projectileController.owner.GetComponent<CharacterBody>();

            //Debug.Log("Wake");

            //EffectManager.SpawnEffect(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ExplodeOnDeathVoidExplosionEffect"), new EffectData
            //{
            //    origin = gameObject.transform.position,
            //    scale = 8f,

            //}, true);

        }


        void FixedUpdate()
        {

            if (overlapAttack != null)
            {
                timer += Time.deltaTime;

                // Carrega efeito de impacto
                GameObject effectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/DaggerImpact");
                EffectIndex effectIndex = EffectCatalog.FindEffectIndexFromPrefab(effectPrefab);

                BlastAttack CSBlastAttack = new BlastAttack();
                CSBlastAttack.attacker = ownerBody.gameObject;
                CSBlastAttack.inflictor = gameObject;
                CSBlastAttack.teamIndex = TeamComponent.GetObjectTeam(ownerBody.gameObject);
                CSBlastAttack.baseDamage = damageCoefficient * ownerBody.damage;
                CSBlastAttack.baseForce = 200f;
                CSBlastAttack.position = transform.position;
                CSBlastAttack.radius = 10f;
                CSBlastAttack.bonusForce = new Vector3(1f, 1f, 1f);
                CSBlastAttack.damageType = DamageType.Stun1s | DamageType.BleedOnHit;
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

    }

}