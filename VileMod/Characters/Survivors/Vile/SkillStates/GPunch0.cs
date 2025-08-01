using VileMod.Modules.BaseStates;
using RoR2;
using UnityEngine;
using static UnityEngine.UI.Selectable;
using EntityStates;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class GPunch0 : BaseMeleeAttackNeko2
    {
        public override void OnEnter()
        {
            hitboxGroupName = "GoliathHitbox";

            damageType = DamageTypeCombo.GenericPrimary;
            damageCoefficient = VileStaticValues.GPunch0DamageCoefficient;
            procCoefficient = 1f;
            pushForce = 300f;
            bonusForce = Vector3.zero;
            baseDuration = 0.8f;
            childLocator = GetModelTransform().GetComponent<ChildLocator>();

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            //for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            attackStartPercentTime = 0.3f;
            attackEndPercentTime = 0.6f;

            //this is the point at which the attack can be interrupted by itself, continuing a combo
            earlyExitPercentTime = 0.6f;

            hitStopDuration = 0.2f;
            attackRecoil = 0.5f;
            hitHopVelocity = 4f;

            swingSoundString = "HenrySwordSwing";
            hitSoundString = "";
            muzzleString = swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            playbackRateParam = "Slash.playbackRate";
            swingEffectPrefab = VileAssets.swordSwingEffect;
            hitEffectPrefab = VileAssets.swordHitImpactEffect;

            impactSound = VileAssets.swordHitSoundEvent.index;



            customAnimator = childLocator.FindChildGameObject("VEH").GetComponents<Animator>()[0];
            SetCustomAnimator(customAnimator);
            //playCustomExitAnim = true;

            //Debug.Log("ChildLocator" + childLocator);
            //Debug.Log("CustomAnimator" + customAnimator);
            //Debug.Log("Animator" + animator);
            Debug.Log("GP0");

            //SetHitReset(true, 3);

            GPunch1 GP1 = new GPunch1();
            SetNextEntityState(GP1);

            //PlayAnimationOnAnimator(customAnimator, "Gesture, Override", "VEH_ATK0_S", playbackRateParam, duration, 0.1f * duration);

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
            PlayAnimationOnAnimator(customAnimator, "Gesture, Override", "VEH_ATK0_S", playbackRateParam, duration * 0.3f, 0.1f * duration);
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
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }

    }
}