using EntityStates;
using VileMod.Modules;
using VileMod.Survivors.Vile;
using VileMod.Survivors.Vile.Components;
using RoR2;
using RoR2.Audio;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace VileMod.Modules.BaseStates
{
    public class VileSpawnState : GenericCharacterSpawnState
    {
        private float duration;
        public float baseDuration = 1f;
        private Animator animator;

        private VileComponent VC;
        private VileBoltComponent VBC;


        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;

            VC = GetComponent<VileComponent>();
            VBC = GetComponent<VileBoltComponent>();

            VBC.SetBoltValue(0);

            if (characterBody.HasBuff(VileBuffs.GoliathBuff))
            {
                characterBody.RemoveBuff(VileBuffs.GoliathBuff);
                VC.ExitGoliath();
            } 

            if (characterBody.HasBuff(VileBuffs.HawkBuff))
            {
                characterBody.RemoveBuff(VileBuffs.HawkBuff);
                VC.ExitHawk();
            }

            if (characterBody.HasBuff(VileBuffs.CyclopsBuff))
            {
                characterBody.RemoveBuff(VileBuffs.CyclopsBuff);
                VC.ExitCyclops();
            }

            if (characterBody.HasBuff(VileBuffs.RideArmorEnabledBuff))
                characterBody.RemoveBuff(VileBuffs.RideArmorEnabledBuff);

        }
        public override void OnExit()
        {

            AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Ready, this.gameObject);

            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();



        }

        

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}

