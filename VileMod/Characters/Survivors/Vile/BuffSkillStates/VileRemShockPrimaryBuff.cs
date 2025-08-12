using EntityStates;
using VileMod.Survivors.Vile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.Survivors.Vile.Components;

namespace VileMod.Characters.Survivors.Vile.BuffSkillStates
{
    public class VileRemShockPrimaryBuff : BaseSkillState
    {

        public static float baseDuration = 1f;
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
                if (characterBody.HasBuff(VileBuffs.PrimaryShockBuff))
                {
                    characterBody.RemoveBuff(VileBuffs.PrimaryShockBuff);
                }
            }

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