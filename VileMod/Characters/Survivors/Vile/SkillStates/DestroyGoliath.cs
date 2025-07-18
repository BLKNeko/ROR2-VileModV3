using EntityStates;
using VileMod.Survivors.Vile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.Survivors.Vile.Components;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class DestroyGoliath : BaseSkillState
    {

        public static float baseDuration = 1f;
        private float duration;
        private VileComponent VC;


        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration;
            characterBody.SetAimTimer(1f);

            //PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);

            VC = GetComponent<VileComponent>();


            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 1.5f * duration);
                characterBody.AddTimedBuff(RoR2Content.Buffs.Immune, 1.5f * duration);

                if (characterBody.HasBuff(VileBuffs.GoliathBuff))
                {
                    characterBody.RemoveBuff(VileBuffs.GoliathBuff);
                }

                if (characterBody.HasBuff(VileBuffs.RideArmorEnabledBuff))
                {
                    characterBody.RemoveBuff(VileBuffs.RideArmorEnabledBuff);
                }
            }

            Debug.Log("Destroying Goliath state entered");

            EffectManager.SimpleMuzzleFlash(VileAssets.rideExplosionEffect, gameObject, "BasePos", true);

            VC.ExitGoliath();

            Vector3 backward = characterDirection.forward * -1f; // Direção para trás do personagem
            Vector3 pushDirection = (backward + Vector3.up).normalized; // para cima + para trás
            float pushForce = 50f;

            characterBody.characterMotor.velocity = pushDirection * pushForce;

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