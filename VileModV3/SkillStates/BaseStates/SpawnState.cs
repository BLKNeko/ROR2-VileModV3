using EntityStates;
using VileMod.Modules;
using RoR2;
using RoR2.Audio;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace VileMod.SkillStates.BaseStates
{
    public class SpawnState : GenericCharacterSpawnState
    {
        private float duration;
        public float baseDuration = 3f;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration;

            //VilePlugin.isvisible = true;
            //VilePlugin.needtocheck = true;


            //AdeptRough.MoraleGauge = 0.01f;

            Util.PlaySound(Sounds.VReady, base.gameObject);
            base.PlayAnimation("Body", "Spawn", "attackSpeed", this.duration);
            //base.PlayAnimation("FullBody, Override", "Death", "attackSpeed", this.duration);

        }
        public override void OnExit()
        {
            Util.PlaySound(Sounds.vileSpawn, base.gameObject);
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

