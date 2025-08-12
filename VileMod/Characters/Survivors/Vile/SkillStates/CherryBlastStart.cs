using EntityStates;
using VileMod.Survivors.Vile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.Survivors.Vile.Components;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class CherryBlastStart : BaseSkillState
    {

        public static float baseDuration = 0.5f;
        private float duration;
        private string playbackRateParam = "ShootGun.playbackRate";

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            characterBody.SetAimTimer(1f);

            //PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);

            if (NetworkServer.active)
            {
                if (!characterBody.HasBuff(VileBuffs.PrimaryHeatBuff))
                {
                    characterBody.AddBuff(VileBuffs.PrimaryHeatBuff);
                }
            }

            PlayCrossfade("LeftArm, Override", "VulcanLoop", playbackRateParam, duration * 0.7f, 0.1f * duration);

        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextState(new CherryBlast());
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}