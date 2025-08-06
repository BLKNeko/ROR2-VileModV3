using ExtraSkillSlots;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.Modules;

namespace VileMod.Survivors.Vile.Components
{
    internal class VileFury : MonoBehaviour
    {
        private Transform modelTransform;

        private Animator Anim;

        private HealthComponent HealthComp;

        private CharacterBody Body;

        private FootstepHandler footstepHandler;

        private CharacterModel model;
        private ChildLocator childLocator;

        private float MinHP;
        private float Timer = 0;


        private void Start()
        {
            //any funny custom behavior you want here
            //for example, enforcer uses a component like this to change his guns depending on selected skill
            if (Body == null)
            {
                Body = GetComponent<CharacterBody>();
            }

            HealthComp = Body.GetComponent<HealthComponent>();

            modelTransform = Body.transform;

            model = Body.GetComponent<ModelLocator>().modelTransform.gameObject.GetComponent<CharacterModel>();

            childLocator = Body.GetComponent<ModelLocator>().modelTransform.gameObject.GetComponent<CharacterModel>().GetComponent<ChildLocator>();

            Anim = Body.characterDirection.modelAnimator;


        }

        private void FixedUpdate()
        {
            if (!Body.hasAuthority) return;

            VFury();

        }

        private void VFury()
        {
            if (Timer > 0)
                Timer -= Time.fixedDeltaTime;

            MinHP = (float)(0.35 + (Body.level / 200));

            if (Body.healthComponent.combinedHealthFraction < MinHP && Timer < 1f)
            {

                if (VileConfig.enableVoiceBool.Value)
                {
                    AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Fury, this.gameObject);
                }


                HealthComp.AddBarrierAuthority(Body.healthComponent.fullHealth / 2f);
                if (NetworkServer.active)
                {
                    Body.AddTimedBuff(RoR2Content.Buffs.LifeSteal, 7f);
                    Body.AddTimedBuff(RoR2Content.Buffs.FullCrit, 7f);
                    Body.AddTimedBuff(RoR2Content.Buffs.SmallArmorBoost, 7f);
                    Body.AddTimedBuff(RoR2Content.Buffs.NoCooldowns, 7f);
                    Body.AddTimedBuff(RoR2Content.Buffs.Warbanner, 7f);
                    Body.AddTimedBuff(VileBuffs.VileFuryBuff, 7f);
                }
                Timer = 200f;

            }
        }

    }
}