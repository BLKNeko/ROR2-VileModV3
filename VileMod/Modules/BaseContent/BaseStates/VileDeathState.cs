using EntityStates;
using VileMod.Modules;
using VileMod.Survivors.Vile;
using RoR2;
using RoR2.Audio;
using System;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.Survivors.Vile.Components;

namespace VileMod.Modules.BaseStates
{
    public class VileDeathState : GenericCharacterDeath
    {
        private float duration;
        public float baseDuration = 0.5f;
        private Animator animator;

        private Transform modelTransform;
        private CharacterModel characterModel;
        private HurtBoxGroup hurtboxGroup;

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



            if (VileConfig.enableVoiceBool.Value)
            {
                AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Die, this.gameObject);
            }

            EffectManager.SimpleMuzzleFlash(VileAssets.vileExplosionEffect, gameObject, "BasePos", true);

            modelTransform = GetModelTransform();
            if ((bool)modelTransform)
            {
                animator = modelTransform.GetComponent<Animator>();
                characterModel = modelTransform.GetComponent<CharacterModel>();
                hurtboxGroup = modelTransform.GetComponent<HurtBoxGroup>();
            }

            if ((bool)characterModel)
            {
                characterModel.invisibilityCount++;
            }





        }

        public override void OnExit()
        {

            //if ((bool)characterModel)
            //{
            //    characterModel.invisibilityCount--;
            //}

            base.PlayAnimation("FullBody, Override", "BufferEmpty", "attackSpeed", this.duration);

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
