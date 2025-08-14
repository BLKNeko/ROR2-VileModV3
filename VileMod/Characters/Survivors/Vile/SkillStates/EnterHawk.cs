using EntityStates;
using VileMod.Survivors.Vile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.Survivors.Vile.Components;
using ExtraSkillSlots;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class EnterHawk : BaseSkillState
    {

        public static float baseDuration = 0.8f;
        private float duration;
        private int boltCost;
        private VileComponent VC;
        private VileBoltComponent VBC;
        private VileRideArmorComponent VRAC;
        private ExtraSkillLocator extraSkillLocator;


        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            characterBody.SetAimTimer(1f);
            boltCost = VileStaticValues.RideArmorHawkCost; //Set the cost of entering Goliath mode

            //PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);

            VC = GetComponent<VileComponent>();
            VBC = GetComponent<VileBoltComponent>();
            VRAC = GetComponent<VileRideArmorComponent>();
            extraSkillLocator = GetComponent<ExtraSkillLocator>();

            if (VBC.GetBoltValue() < boltCost && !characterBody.HasBuff(VileBuffs.RideArmorEnabledBuff)) 
            {
                //Play sound
                AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Error, this.gameObject);

                Chat.AddMessage(
                    $"<color=#FFA500>You need at least " +
                    $"<color=#C0C0C0>{boltCost}</color> " +
                    $"<color=#A020F0>Vile</color> <color=#C0C0C0>Bolts</color> to call " +
                    $"<color=#00BFFF>Hawk Ride Armor</color>! You currently have " +
                    $"<color=#C0C0C0>{VBC.GetBoltValue()}</color> " +
                    $"<color=#A020F0>Vile</color> <color=#C0C0C0>Bolts</color>.</color>"
                );

                outer.SetNextStateToMain();
                extraSkillLocator.extraFourth.Reset();
                return;
            }

            if (NetworkServer.active)
            {
                //characterBody.AddBuff(VileBuffs.GoliathBuff);
                characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 7f);
                characterBody.AddTimedBuff(RoR2Content.Buffs.Immune, 7f);
                characterBody.AddTimedBuff(RoR2Content.Buffs.Intangible, 7f);
            }

            VBC.ChangeBoltValue(-boltCost); //Remove 1000 bolts
            //VC.EnterGoliath();

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

                if (VileConfig.enableVoiceBool.Value)
                {
                    AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_RideArmor_In_VSFX, this.gameObject);
                }

                outer.SetNextState(new EnterHawkEnd());
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}