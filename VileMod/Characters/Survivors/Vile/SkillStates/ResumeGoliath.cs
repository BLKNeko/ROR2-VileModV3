using EntityStates;
using VileMod.Survivors.Vile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.Survivors.Vile.Components;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class ResumeGoliath : BaseSkillState
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

            customAnimator = childLocator.FindChildGameObject("VEH").GetComponents<Animator>()[0];

            playbackRateParam = "Slash.playbackRate";

            EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/TeleportOutBoom"), new EffectData
            {
                origin = transform.position,
                rotation = transform.rotation
            }, transmit: true);

            AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_TP_In, this.gameObject);

            VC.EnterGoliath();

            PlayAnimationOnAnimator(customAnimator, "FullBody, Override", "Login", playbackRateParam, duration * 0.5f, 0.1f * duration);

        }

        public override void OnExit()
        {

            AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Ride_Armor_In_SFX, this.gameObject);

            if (NetworkServer.active)
            {
                characterBody.AddBuff(VileBuffs.GoliathBuff);
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