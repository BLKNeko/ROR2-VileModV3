using EntityStates;
using VileMod.Survivors.Vile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.Survivors.Vile.Components;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class RepairRideArmor : BaseSkillState
    {

        public static float baseDuration = 1f;
        private float duration;
        private int boltCost;
        private int boltAmount;
        private float repairMultiplyer;
        private float rMaxHealth;
        private float rHealth;
        private VileComponent VC;
        private VileBoltComponent VBC;
        private VileRideArmorComponent VRAC;

        private ChildLocator childLocator;

        private Animator customAnimator;

        private string playbackRateParam;


        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            characterBody.SetAimTimer(1f);
            boltCost = 1; //Set the cost of entering Goliath mode
            playbackRateParam = "Slash.playbackRate";

            childLocator = GetModelTransform().GetComponent<ChildLocator>();

            customAnimator = childLocator.FindChildGameObject("VEH").GetComponents<Animator>()[0];

            VC = GetComponent<VileComponent>();
            VBC = GetComponent<VileBoltComponent>();
            VRAC = GetComponent<VileRideArmorComponent>();

            //PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);

            // Obter dados atuais
            boltAmount = VBC.GetBoltValue();
            rMaxHealth = VRAC.GetRMaxHealthValue();
            rHealth = VRAC.GetRHealthValue();

            repairMultiplyer = rMaxHealth / 1000f;
            float missingHealth = rMaxHealth - rHealth;
            float totalHealPossible = boltAmount * repairMultiplyer;

            if (missingHealth <= 0f)
            {
                Chat.AddMessage("Ride Armor is already fully repaired!");
                return;
            }

            float healAmount;
            int boltsToConsume;

            if (totalHealPossible >= missingHealth)
            {
                healAmount = missingHealth;
                boltsToConsume = Mathf.CeilToInt(missingHealth / repairMultiplyer);
            }
            else
            {
                healAmount = totalHealPossible;
                boltsToConsume = boltAmount;
            }

            // Aplicar cura
            VRAC.RepairRideArmor(healAmount);
            VBC.ChangeBoltValue(-boltsToConsume);

            Chat.AddMessage($"Ride Armor repaired for {healAmount:F0} HP using {boltsToConsume} Vile Bolts.");

            PlayAnimationOnAnimator(customAnimator, "FullBody, Override", "VEH_Win", playbackRateParam, duration * 0.3f, 0.1f * duration);

            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 1.5f * duration);
                characterBody.AddTimedBuff(RoR2Content.Buffs.Intangible, 1.5f * duration);
                characterBody.AddTimedBuff(RoR2Content.Buffs.Immune, 1.5f * duration);

            }

            AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_SFX_Recov_HP, this.gameObject);

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