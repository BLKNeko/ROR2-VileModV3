using EntityStates;
using VileMod.Survivors.Vile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.Survivors.Vile.Components;
using RoR2.Projectile;
using VileMod.Modules;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class VCerberusPhanton : BaseSkillState
    {
        public static float damageCoefficient = VileStaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.8f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 1000f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;
        private string playbackRateParam;

        private VileComponent VC;
        private float elementBonus;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "VLCannonMuzz";
            playbackRateParam = "ShootGun.playbackRate";
            elementBonus = 0.4f + characterBody.level / 100f;
            VC = GetComponent<VileComponent>();

            PlayCrossfade("Gesture, Override", "VileCannon", playbackRateParam, duration * 0.7f, 0.1f * duration);

        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= fireTime)
            {
                Fire();
            }

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        private void Fire()
        {
            if (!hasFired)
            {
                hasFired = true;

                if (isAuthority)
                {
                    characterBody.AddSpreadBloom(0.8f);
                    EffectManager.SimpleMuzzleFlash(EntityStates.Mage.Weapon.FireRoller.fireMuzzleflashEffectPrefab, gameObject, muzzleString, true);
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    Ray aimRay = GetAimRay();

                    // Ângulo entre os projéteis (em graus)
                    float angleSpread = 5f;

                    // Direções com espalhamento: centro, esquerda, direita
                    Vector3[] directions = new Vector3[]
                    {
                        aimRay.direction, // centro
                        Quaternion.AngleAxis(-angleSpread, Vector3.up) * aimRay.direction, // esquerda
                        Quaternion.AngleAxis(angleSpread, Vector3.up) * aimRay.direction  // direita
                    };

                    if (VileConfig.enableVoiceBool.Value)
                    {
                        AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Attack, this.gameObject);
                    }

                    AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Laser_Shot, this.gameObject);

                    foreach (Vector3 direction in directions)
                    {
                        FireProjectileInfo projectileInfo = new FireProjectileInfo
                        {
                            projectilePrefab = VileAssets.CerberusPhantonFMJProjectile,
                            position = aimRay.origin,
                            rotation = Util.QuaternionSafeLookRotation(direction),
                            owner = gameObject,
                            damage = damageCoefficient * damageStat,
                            force = force,
                            crit = RollCrit(),
                            damageColorIndex = DamageColorIndex.Default
                        };

                        ProjectileManager.instance.FireProjectile(projectileInfo);
                    }

                    VC.SetElementValues(0f, elementBonus, 0f, true, false, true);
                    VC.SetExtraHeatValues(-0.2f);
                }
            }
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}