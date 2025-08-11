using EntityStates;
using VileMod.Survivors.Vile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.Survivors.Vile.Components;
using ExtraSkillSlots;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class EnterGoliath : BaseSkillState
    {

        public static float baseDuration = 0.8f;
        private float duration;
        private int boltCost;
        private VileComponent VC;
        private VileBoltComponent VBC;
        private VileRideArmorComponent VRAC;
        private ExtraSkillLocator extraSkillLocator;

        private ChildLocator childLocator;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration;
            characterBody.SetAimTimer(1f);
            boltCost = VileStaticValues.RideArmorGoliathCost; //Set the cost of entering Goliath mode

            //PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);

            VC = GetComponent<VileComponent>();
            VBC = GetComponent<VileBoltComponent>();
            VRAC = GetComponent<VileRideArmorComponent>();
            extraSkillLocator = GetComponent<ExtraSkillLocator>();
            childLocator = GetModelTransform().GetComponent<ChildLocator>();

            if (VBC.GetBoltValue() < boltCost && !characterBody.HasBuff(VileBuffs.RideArmorEnabledBuff)) 
            {
                //Play sound
                AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Error, this.gameObject);

                Chat.AddMessage(
                    $"<color=#FFA500>You need at least " +
                    $"<color=#C0C0C0>{boltCost}</color> " +
                    $"<color=#A020F0>Vile</color> <color=#C0C0C0>Bolts</color> to call " +
                    $"<color=#00BFFF>Goliath Ride Armor</color>! You currently have " +
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
                characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 5f);
                characterBody.AddTimedBuff(RoR2Content.Buffs.Immune, 5f);
                characterBody.AddTimedBuff(RoR2Content.Buffs.Intangible, 5f);
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
                //outer.SetNextState(new EnterGoliathAnim());

                if (VileConfig.enableVoiceBool.Value)
                {
                    AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_RideArmor_In_VSFX, this.gameObject);
                }

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