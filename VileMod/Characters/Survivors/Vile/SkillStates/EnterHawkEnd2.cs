using EntityStates;
using VileMod.Survivors.Vile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.Survivors.Vile.Components;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class EnterHawkEnd2 : BaseSkillState
    {

        public static float baseDuration = 1f;
        private float duration;
        private int boltCost;
        private VileComponent VC;
        private VileBoltComponent VBC;
        private VileRideArmorComponent VRAC;

        private ChildLocator childLocator;

        private Animator customAnimator;

        private string playbackRateParam;

        private bool rideFinished = false;

        private GameObject rideArmorInstance;


        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration;
            characterBody.SetAimTimer(1f);

            VC = GetComponent<VileComponent>();

            childLocator = GetModelTransform().GetComponent<ChildLocator>();

            customAnimator = childLocator.FindChildGameObject("HAWK").GetComponents<Animator>()[0];

            playbackRateParam = "Slash.playbackRate";

            VC.EnterHawk(true);

        }

        public override void OnExit()
        {


            PlayAnimationOnAnimator(customAnimator, "FullBody, Override", "R_Login", playbackRateParam, duration * 0.5f, 0.1f * duration);
            AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Ride_Armor_In_SFX, this.gameObject);

            if (NetworkServer.active)
            {
                if (!characterBody.HasBuff(VileBuffs.HawkBuff))
                    characterBody.AddBuff(VileBuffs.HawkBuff);

                if (!characterBody.HasBuff(VileBuffs.RideArmorEnabledBuff))
                    characterBody.AddBuff(VileBuffs.RideArmorEnabledBuff);
            }

            rideFinished = false;

            PlayAnimation("Body", "IdleLock", "ShootGun.playbackRate", 1.8f);

            AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Armed, this.gameObject);

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}