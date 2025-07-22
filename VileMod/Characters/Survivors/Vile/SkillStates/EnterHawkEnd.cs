using EntityStates;
using VileMod.Survivors.Vile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.Survivors.Vile.Components;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class EnterHawkEnd : BaseSkillState
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


        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration;
            characterBody.SetAimTimer(1f);

            VC = GetComponent<VileComponent>();

            childLocator = GetModelTransform().GetComponent<ChildLocator>();

            customAnimator = childLocator.FindChildGameObject("HAWK").GetComponents<Animator>()[0];

            playbackRateParam = "Slash.playbackRate";

            VC.EnterHawk();

            //PlayAnimationOnAnimator(customAnimator, "FullBody, Override", "Login", playbackRateParam, duration * 0.5f, 0.1f * duration);

        }

        public override void OnExit()
        {

            if (NetworkServer.active)
            {
                characterBody.AddBuff(VileBuffs.HawkBuff);
            }

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