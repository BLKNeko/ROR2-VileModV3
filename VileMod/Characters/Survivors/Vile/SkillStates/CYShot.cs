using EntityStates;
using VileMod.Survivors.Vile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.Survivors.Vile.Components;
using RoR2.Projectile;
using VileMod.Modules;
using R2API;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class CYShot : BaseSkillState
    {
        public static float damageCoefficient = VileStaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 1000f;
        public static float recoil = 4f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        private VileComponent VC;

        private ChildLocator childLocator;

        private Animator customAnimator;
        private string playbackRateParam;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "CYLMuzz";
            playbackRateParam = "Slash.playbackRate";
            VC = GetComponent<VileComponent>();

            childLocator = GetModelTransform().GetComponent<ChildLocator>();

            customAnimator = childLocator.FindChildGameObject("CY").GetComponents<Animator>()[0];

            PlayAnimationOnAnimator(customAnimator, "FullBody, Override", "VEHShootS", playbackRateParam, duration * 0.3f, 0.1f * duration);

        }

        public override void OnExit()
        {
            PlayAnimationOnAnimator(customAnimator, "FullBody, Override", "VEHShootE", playbackRateParam, duration * 0.3f, 0.1f * duration);
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
                    EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireShotgun.effectPrefab, gameObject, muzzleString, true);
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    if (VileConfig.enableVoiceBool.Value)
                    {
                        AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Attack, this.gameObject);
                    }

                    AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Charge_Shoot, this.gameObject);

                    Ray aimRay = GetAimRay();

                    FireProjectileInfo VShotgunIceProjectille = new FireProjectileInfo();
                    VShotgunIceProjectille.projectilePrefab = VileAssets.CYPlasmaProjectile;
                    VShotgunIceProjectille.position = aimRay.origin;
                    VShotgunIceProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    VShotgunIceProjectille.owner = gameObject;
                    VShotgunIceProjectille.damage = damageCoefficient * damageStat;
                    VShotgunIceProjectille.force = force;
                    VShotgunIceProjectille.crit = RollCrit();
                    //XBusterMediumProjectille.speedOverride = XBusterMediumProjectille.speedOverride * 0.8f;
                    VShotgunIceProjectille.damageColorIndex = DamageColorIndex.Luminous;
                    


                    ProjectileManager.instance.FireProjectile(VShotgunIceProjectille);

                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}