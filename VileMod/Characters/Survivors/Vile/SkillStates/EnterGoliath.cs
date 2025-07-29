using EntityStates;
using VileMod.Survivors.Vile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.Survivors.Vile.Components;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class EnterGoliath : BaseSkillState
    {

        public static float baseDuration = 1f;
        private float duration;
        private int boltCost;
        private VileComponent VC;
        private VileBoltComponent VBC;
        private VileRideArmorComponent VRAC;


        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration;
            characterBody.SetAimTimer(1f);
            boltCost = 1; //Set the cost of entering Goliath mode

            //PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);

            VC = GetComponent<VileComponent>();
            VBC = GetComponent<VileBoltComponent>();
            VRAC = GetComponent<VileRideArmorComponent>();

            if(VBC.GetBoltValue() < boltCost && !characterBody.HasBuff(VileBuffs.RideArmorEnabledBuff)) 
            { 
                //Play sound

                Chat.AddMessage($"You need at least {boltCost} Vile Bolts to enter Goliath mode! You currently have {VBC.GetBoltValue()} Vile Bolts.");

                outer.SetNextStateToMain();
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