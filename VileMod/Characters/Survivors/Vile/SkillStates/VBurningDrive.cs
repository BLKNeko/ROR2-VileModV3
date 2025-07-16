using VileMod.Modules.BaseStates;
using RoR2;
using UnityEngine;
using static UnityEngine.UI.Selectable;
using EntityStates;
using VileMod.Survivors.Vile.Components;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class VBurningDrive : BaseMeleeAttackNeko2
    {

        private VileComponent VC;

        public override void OnEnter()
        {
            hitboxGroupName = "SwordGroup";

            damageType |= DamageTypeCombo.GenericSpecial;
            damageType |= DamageType.IgniteOnHit;
            damageCoefficient = HenryStaticValues.swordDamageCoefficient;
            procCoefficient = 1f;
            pushForce = 500f;
            bonusForce = Vector3.zero;
            baseDuration = 0.8f;
            childLocator = GetModelTransform().GetComponent<ChildLocator>();

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            //for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            attackStartPercentTime = 0.2f;
            attackEndPercentTime = 0.8f;

            //this is the point at which the attack can be interrupted by itself, continuing a combo
            earlyExitPercentTime = 0.6f;

            hitStopDuration = 0.012f;
            attackRecoil = 0.5f;
            hitHopVelocity = 4f;

            swingSoundString = "HenrySwordSwing";
            hitSoundString = "";
            muzzleString = swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            playbackRateParam = "Slash.playbackRate";
            swingEffectPrefab = VileAssets.swordSwingEffect;
            hitEffectPrefab = VileAssets.swordHitImpactEffect;

            impactSound = VileAssets.swordHitSoundEvent.index;

            VC = GetComponent<VileComponent>();

            damageCoefficient = (damageCoefficient + (VC.GetBaseHeatValue() * 2f)) + (damageCoefficient * (VC.GetBaseOverHeatValue() * 5));

            SetHitReset(true, 5);

            float elementBonus = (0.1f + characterBody.level / 100f) + ((VC.GetBaseHeatValue() + VC.GetBaseOverHeatValue() / 2f));

            VC.SetElementValues(0f, 0f, elementBonus, true, true, false);
            

            base.OnEnter();
        }

        protected override void PlayCustomExitAnimation()
        {
            //PlayCrossfade("Gesture, Override", "VEH_ATK0_L_END", playbackRateParam, duration, 0.1f * duration);
            //PlayAnimationOnAnimator(customAnimator, "Gesture, Override", "VEH_ATK0_L_END", playbackRateParam, duration * 0.2f, 0.1f * duration);
        }

        protected override void PlayAttackAnimation()
        {
            //PlayCrossfade("Gesture, Override", "VEH_ATK0_S", playbackRateParam, duration, 0.1f * duration);
            //PlayAnimationOnAnimator(customAnimator, "Gesture, Override", "VEH_ATK0_S", playbackRateParam, duration * 0.3f, 0.1f * duration);
        }

        protected override void PlaySwingEffect()
        {
            base.PlaySwingEffect();
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        public override void OnExit()
        {
            VC.SetHeatValue(0f);
            VC.SetOverHeatValue(0f);

            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }

    }
}