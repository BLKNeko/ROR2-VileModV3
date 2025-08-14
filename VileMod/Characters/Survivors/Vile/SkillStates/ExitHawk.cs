using EntityStates;
using VileMod.Survivors.Vile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.Survivors.Vile.Components;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class ExitHawk : BaseSkillState
    {

        public static float baseDuration = 1f;
        private float duration;
        private VileComponent VC;


        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            characterBody.SetAimTimer(1f);

            //PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);

            VC = GetComponent<VileComponent>();

            PlayAnimation("Body", "Idle", "ShootGun.playbackRate", 0f);


            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 0.5f * duration);

                if (characterBody.HasBuff(VileBuffs.HawkBuff))
                {
                    characterBody.RemoveBuff(VileBuffs.HawkBuff);
                }
            }

            EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/TeleportOutBoom"), new EffectData
            {
                origin = transform.position,
                rotation = transform.rotation
            }, transmit: true);

            if (VileConfig.enableVoiceBool.Value)
            {
                AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Return, this.gameObject);
            }

            AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Ride_Armor_Out, this.gameObject);
            VC.ExitHawk();

        }

        public override void OnExit()
        {
            AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_TP_Out, this.gameObject);
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