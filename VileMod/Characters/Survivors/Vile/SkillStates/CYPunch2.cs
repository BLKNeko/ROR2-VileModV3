using VileMod.Modules.BaseStates;
using RoR2;
using UnityEngine;
using EntityStates;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class CYPunch2 : BaseMeleeAttackNeko2
    {
        public override void OnEnter()
        {
            hitboxGroupName = "GoliathHitbox";

            damageType = DamageTypeCombo.GenericPrimary;
            damageCoefficient = VileStaticValues.CyPunchDamageCoefficient;
            procCoefficient = 1f;
            pushForce = 500f;
            bonusForce = Vector3.up;
            baseDuration = 0.5f;
            childLocator = GetModelTransform().GetComponent<ChildLocator>();

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            //for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            attackStartPercentTime = 0.1f;
            attackEndPercentTime = 0.6f;

            //this is the point at which the attack can be interrupted by itself, continuing a combo
            earlyExitPercentTime = 0.5f;

            hitStopDuration = 0.2f;
            attackRecoil = 0.5f;
            hitHopVelocity = 4f;

            swingSoundString = VileStaticValues.Play_Vile_CYPunch_SFX;
            hitSoundString = "";
            muzzleString = swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            playbackRateParam = "Slash.playbackRate";
            swingEffectPrefab = VileAssets.swordSwingEffect;
            hitEffectPrefab = VileAssets.swordHitImpactEffect;

            impactSound = VileAssets.swordHitSoundEvent.index;

            customAnimator = childLocator.FindChildGameObject("CY").GetComponents<Animator>()[0];
            SetCustomAnimator(customAnimator);
            //playCustomExitAnim = true;

            SetHitReset(true, 3);

            base.OnEnter();
        }

        protected override void PlayCustomExitAnimation()
        {
            //PlayCrossfade("Gesture, Override", "VEH_ATK0_L_END", playbackRateParam, duration, 0.1f * duration);
            //PlayAnimationOnAnimator(customAnimator, "Gesture, Override", "VEH_ATK0_R_END", playbackRateParam, duration * 0.2f, 0.1f * duration);
        }

        protected override void PlayAttackAnimation()
        {
            //PlayCrossfade("Gesture, Override", "VEH_ATK0_L", playbackRateParam, duration, 0.1f * duration);\
            PlayAnimationOnAnimator(customAnimator, "Gesture, Override", "CY_ATK0_2", playbackRateParam, duration * 0.8f, 0.1f * duration);
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
            skillLocator.primary.temporaryCooldownPenalty += 1f;
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }

    }
}