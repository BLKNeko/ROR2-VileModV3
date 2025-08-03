using RoR2;
using System.Collections;
using UnityEngine;
using BepInEx;
using RoR2.Projectile;
using VileMod.Modules;
using VileMod.Survivors.Vile;

namespace VileMod.Characters.Survivors.Vile.Components.UnitsComponents
{
    public class TogericsController : MonoBehaviour
    {
        private ProjectileOverlapAttack overlapAttack;

        private ProjectileImpactExplosion impactExplosion;

        private ProjectileController projectileController;

        private float timer = 0f;
        private float timeLimit = 0.8f;
        private float damageCoeficient;
        private float damagebonus = 1f;

        private BlastAttack blastAttack;

        void Awake()
        {
            impactExplosion = GetComponent<ProjectileImpactExplosion>();
            overlapAttack = GetComponent<ProjectileOverlapAttack>();
            projectileController = GetComponent<ProjectileController>();
            damageCoeficient = VileStaticValues.UnitTogericsDamageCoefficient;

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

                blastAttack = new BlastAttack();
                blastAttack.attacker = gameObject;
                blastAttack.inflictor = gameObject;
                blastAttack.teamIndex = TeamComponent.GetObjectTeam(projectileController.owner);
                blastAttack.baseDamage = damageCoeficient * projectileController.owner.GetComponent<CharacterBody>().damage;
                blastAttack.baseForce = 10f;
                blastAttack.position = gameObject.transform.position;
                blastAttack.radius = 10f;
                blastAttack.bonusForce = new Vector3(1f, 1f, 1f);
                blastAttack.damageType = DamageType.SlowOnHit | DamageType.BleedOnHit;
                blastAttack.damageColorIndex = DamageColorIndex.Bleed;

                //Debug.Log("damageCoeficient: " + damageCoeficient);
                //Debug.Log("damageStat: " + damageStat);
                //Debug.Log("blastAttack.baseDamage: " + blastAttack.baseDamage);
                //Debug.Log("projectileController.owner: " + projectileController.owner);
                //Debug.Log("projectileController.owner.GetComponent<CharacterBody>().damage: " + projectileController.owner.GetComponent<CharacterBody>().damage);

                if (timer > timeLimit)
                {
                    //overlapAttack.ResetOverlapAttack();
                    blastAttack.Fire();
                    timer = 0f;
                }



            }
        }

    }

}