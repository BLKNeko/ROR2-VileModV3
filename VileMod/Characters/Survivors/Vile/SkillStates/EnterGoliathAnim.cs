using EntityStates;
using VileMod.Survivors.Vile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.Survivors.Vile.Components;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class EnterGoliathAnim : BaseSkillState
    {

        public static float baseDuration = 1f;
        private float duration;

        private ChildLocator childLocator;

        private Animator customAnimator;

        private string playbackRateParam;

        private bool completeAnim = false;


        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration;
            characterBody.SetAimTimer(1f);
            completeAnim = false;

            childLocator = GetModelTransform().GetComponent<ChildLocator>();

            customAnimator = childLocator.FindChildGameObject("VEH").GetComponents<Animator>()[0];

            playbackRateParam = "Slash.playbackRate";

            childLocator.FindChildGameObject("VEH").SetActive(true);



        }

        private void FallG()
        {
            if (!isAuthority) return;

            

            PlayAnimationOnAnimator(customAnimator, "FullBody, Override", "Login_Fall", playbackRateParam, duration * 0.25f, 0);

            completeAnim = true;
        }

        public override void OnExit()
        {
            completeAnim = false;
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if(!childLocator.FindChildGameObject("VEH").activeSelf)
                childLocator.FindChildGameObject("VEH").SetActive(true);

            if (!completeAnim && childLocator.FindChildGameObject("VEH").activeSelf)
                FallG();

            if (fixedAge >= duration && isAuthority && completeAnim)
            {
                outer.SetNextState(new EnterGoliathEnd());
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}