using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.SkillStates;

namespace VileMod.SkillStates.BaseStates
{
    public class Fury : GenericCharacterMain
    {
        public float Timer = 5f;
        public float ChillTime;
        public float ChillDelay = 1.5f;
        public float PassiveTimer = 0f;
        public bool isHeated;
        public float HeatTime = 5f;
        public float baseDuration = 1f;
        public double MinHP;
        public static bool isCrit;

        private float duration;
        private Animator animator;
        public override void OnEnter()
        {
            base.OnEnter();

        }
        public override void OnExit()
        {
            base.OnExit();
        }


        public override void Update()
        {
            base.Update();

            ChillTime += Time.deltaTime;

            if (base.inputBank.skill1.justReleased)
                ChillDelay = 0.1f;


            if (ChillTime >= 2f)
            {
                if (ChillDelay >= 1.5f - (base.attackSpeedStat / 10))
                    ChillDelay = 1.5f - (base.attackSpeedStat / 10);
                else
                    ChillDelay += 0.15f;

                ChillTime = 0f;
            }

            CherryBlast.Chilldelay = ChillDelay;


        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();
            Timer += Time.fixedDeltaTime;

            if (base.inputBank.skill2.justReleased || base.inputBank.skill3.justReleased || base.inputBank.skill4.justReleased)
            {
                Timer = 0f;
            }

            if (Timer <= HeatTime)
                CherryBlast.heat = true;
            else
            {
                CherryBlast.heat = false;
                CherryBlast.buffSkillIndex = 0;
            }

            if (base.inputBank.skill2.justReleased && (Timer <= HeatTime))
                CherryBlast.buffSkillIndex = 1;

            if (base.inputBank.skill3.justReleased && (Timer <= HeatTime))
                CherryBlast.buffSkillIndex = 2;

            if (base.inputBank.skill4.justReleased && (Timer <= HeatTime))
                CherryBlast.buffSkillIndex = 3;

            //-------PASSIVE EFFECT

            PassiveTimer -= Time.fixedDeltaTime;

            MinHP = 0.35 + (base.characterBody.level / 200);
            if (base.characterBody.healthComponent.combinedHealthFraction < MinHP && PassiveTimer < 5f)
            {
                Util.PlaySound(Modules.Sounds.vilePassive, base.gameObject);
                //EffectManager.SimpleMuzzleFlash(Modules.Assets.RedEyeVFX, base.gameObject, "EYE", true);
                base.healthComponent.AddBarrierAuthority(base.characterBody.healthComponent.fullHealth / 2f);
                if (NetworkServer.active)
                {
                    base.characterBody.AddTimedBuff(RoR2Content.Buffs.LifeSteal, 6f);
                    base.characterBody.AddTimedBuff(RoR2Content.Buffs.FullCrit, 10f);
                    base.characterBody.AddTimedBuff(RoR2Content.Buffs.Warbanner, 10f);
                    base.characterBody.AddTimedBuff(RoR2Content.Buffs.NoCooldowns, 3f);
                }
                PassiveTimer = 50f;

            }

            //------------------------ Check Crit because passive interfer on the primary skill

            if (base.characterBody.HasBuff(RoR2Content.Buffs.FullCrit))
                isCrit = true;
            else
                isCrit = Util.CheckRoll(base.critStat, base.characterBody.master);

            return;

        }

        public static bool GetHeat()
        {
            Fury VP = new Fury();


            bool heat = VP.isHeated;

            return heat;
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
