using EntityStates;
using VileMod.Modules.BaseStates;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;
using On.RoR2.UI;
using UnityEngine.UI;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class HawkDash : BaseSkillState
    {

        public static float initialSpeedCoefficient = 4f;
        public static float finalSpeedCoefficient = 4f;
        public static float dodgeFOV = global::EntityStates.Commando.DodgeState.dodgeFOV;

        private float rollSpeed;
        private Vector3 forwardDirection;
        private Animator animator;
        private Vector3 previousPosition;

        private ChildLocator childLocator;

        private string LDashPos = "LDashPos";
        private string RDashPos = "RDashPos";

        public static float duration = 0.7f;

        public override void OnEnter()
        {

            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab, gameObject, LDashPos, true);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab, gameObject, RDashPos, true);

            AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Ride_Armor_Boost, this.gameObject);

            animator = GetModelAnimator();
            characterBody.SetAimTimer(0.8f);
            Ray aimRay = GetAimRay();

            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(VileBuffs.HawkDashBuff, 5f);
            }

            base.characterMotor.Motor.ForceUnground(0.1f);

            if (isAuthority && inputBank && characterDirection)
            {
                forwardDirection = aimRay.direction.normalized;
            }

            if (characterMotor && characterDirection)
            {
                characterMotor.velocity = forwardDirection.normalized * moveSpeedStat * initialSpeedCoefficient;
            }

            childLocator = GetModelTransform().GetComponent<ChildLocator>();

            childLocator.FindChildGameObject("HKBoostVFX").SetActive(true);
            

            //base.PlayAnimation("FullBody, Override", "DashStart", "attackSpeed", duration);

            base.OnEnter();
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();

            base.characterMotor.Motor.ForceUnground(0.1f);

            if (characterDirection) characterDirection.forward = forwardDirection;

            if (cameraTargetParams)
                cameraTargetParams.fovOverride = Mathf.Lerp(dodgeFOV, 60f, fixedAge / duration);


            if (characterMotor && characterDirection)
            {
                characterMotor.velocity = forwardDirection.normalized * moveSpeedStat * Mathf.Lerp(initialSpeedCoefficient, finalSpeedCoefficient, fixedAge / duration);
            }

            if (isAuthority && fixedAge >= duration)
            {
                outer.SetNextStateToMain();
                return;
            }

        }

        public override void OnExit()
        {
            characterMotor.velocity = forwardDirection.normalized * moveSpeedStat * Mathf.Lerp(initialSpeedCoefficient, finalSpeedCoefficient, fixedAge / duration);

            childLocator.FindChildGameObject("HKBoostVFX").SetActive(false);

            //base.PlayAnimation("FullBody, Override", "DashEnd", "attackSpeed", duration);

            base.OnExit();
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(forwardDirection);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            forwardDirection = reader.ReadVector3();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}