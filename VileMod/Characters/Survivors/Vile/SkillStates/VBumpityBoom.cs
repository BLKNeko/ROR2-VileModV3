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
    public class VBumpityBoom : BaseSkillState
    {
        public static float damageCoefficient = HenryStaticValues.gunDamageCoefficient;
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

        private VileComponent VC;
        private float elementBonus;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "Muzzle";
            elementBonus = 0.4f + characterBody.level / 100f;
            VC = GetComponent<VileComponent>();

            //PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);

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
                    EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireBarrage.effectPrefab, gameObject, muzzleString, true);
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    //if (XConfig.enableVoiceBool.Value)
                    //{
                    //    AkSoundEngine.PostEvent(XStaticValues.X_shotgunIce_VSFX, this.gameObject);
                    //}

                    //PlayAnimation("Gesture, Override", "XBusterChargeAttack", "attackSpeed", this.duration);

                    Ray aimRay = GetAimRay();

                    FireProjectileInfo VProjectille = new FireProjectileInfo();
                    VProjectille.projectilePrefab = VileAssets.BumpityBombProjectile;
                    VProjectille.position = aimRay.origin;
                    VProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    VProjectille.owner = gameObject;
                    VProjectille.damage = damageCoefficient * damageStat;
                    VProjectille.force = force;
                    VProjectille.crit = RollCrit();
                    //XBusterMediumProjectille.speedOverride = XBusterMediumProjectille.speedOverride * 0.8f;
                    VProjectille.damageColorIndex = DamageColorIndex.Default;

                    ProjectileManager.instance.FireProjectile(VProjectille);

                    VC.SetElementValues(0f, 0f, elementBonus, true, true, false);

                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}