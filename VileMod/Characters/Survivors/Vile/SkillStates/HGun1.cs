using EntityStates;
using VileMod.Modules;
using VileMod.Survivors.Vile;
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.Survivors.Vile.Components;
using UnityEngine.UIElements.Experimental;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class HGun1 : BaseSkillState
    {
        public static float damageCoefficient = VileStaticValues.HGun1DamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 600f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerSmokeChase");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private Animator animator;
        private string muzzleString;
        private string muzzleString2;

        private float fireTimer;
        private float fireInterval = 0.2f;
        private int missilesFired;
        private int totalMissiles;

        private HuntressTracker huntressTracker;
        private float stopwatch;
        private Transform modelTransform;
        private bool hasTriedToThrowDagger;
        private HurtBox initialOrbTarget;
        private ChildLocator childLocator;

        private float missleAmount;
        private VileComponent VC;

        private Animator customAnimator;
        private string playbackRateParam = "ShootGun.playbackRate";

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();

            muzzleString = "HKGunLMuzz";
            muzzleString2 = "HKGunRMuzz";

            this.modelTransform = base.GetModelTransform();
            this.animator = base.GetModelAnimator();
            this.huntressTracker = base.GetComponent<HuntressTracker>();
            VC = GetComponent<VileComponent>();

            if (this.modelTransform)
            {
                this.childLocator = this.modelTransform.GetComponent<ChildLocator>();
            }

            if (base.characterBody)
            {
                base.characterBody.SetAimTimer(this.duration);
            }

            if (this.huntressTracker && base.isAuthority)
            {
                this.initialOrbTarget = this.huntressTracker.GetTrackingTarget();
            }

            fireInterval = 0.4f - attackSpeedStat * 0.1f; // Adjust fire interval based on attack speed

            fireInterval = Mathf.Clamp(fireInterval, 0.2f, 0.4f);

            fireTimer = 0f;
            missilesFired = 0;

            customAnimator = childLocator.FindChildGameObject("HAWK").GetComponents<Animator>()[0];

            //totalMissiles = Mathf.RoundToInt(10f + VC.GetBaseHeatValue() * 5f + VC.GetBaseOverHeatValue() * 10f);
            totalMissiles = 3;

            if (VileConfig.enableVoiceBool.Value)
            {
                AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Attack, this.gameObject);
            }




        }

        public override void OnExit()
        {

            base.OnExit();
        }

        protected virtual GenericDamageOrb CreateArrowOrb()
        {
            return new HomingTorpedoOrb();
        }



        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (missilesFired >= totalMissiles)
                return;

            fireTimer -= Time.fixedDeltaTime;
            if (fireTimer <= 0f)
            {
                FireMissile(missilesFired);
                missilesFired++;
                fireTimer = fireInterval;
            }

            // Transição opcional quando terminar
            if (missilesFired >= totalMissiles)
            {
                outer.SetNextStateToMain();
            }
        }

        private void FireMissile(int index)
        {
            if (!NetworkServer.active)
                return;

            HurtBox hurtBox = this.initialOrbTarget;

            // Se o alvo atual estiver morto, tenta buscar outro
            if (hurtBox == null || !hurtBox.healthComponent || !hurtBox.healthComponent.alive)
            {
                initialOrbTarget = huntressTracker.GetTrackingTarget();
                hurtBox = initialOrbTarget;
            }

            // Se ainda não tiver um alvo válido, sai
            if (hurtBox == null || !hurtBox.healthComponent || !hurtBox.healthComponent.alive)
                return;

            Transform muzzleTransform = (index % 2 == 0)
                ? this.childLocator.FindChild(this.muzzleString)
                : this.childLocator.FindChild(this.muzzleString2);

            string muzzleName = (index % 2 == 0) ? this.muzzleString : this.muzzleString2;

            if (muzzleTransform)
            {
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab, base.gameObject, muzzleName, true);

                if (index % 2 == 0)
                {
                    PlayAnimationOnAnimator(customAnimator, "LeftArm, Override", "HKShootL", playbackRateParam, duration * 0.7f, 0.15f * duration);
                }
                else
                {
                    PlayAnimationOnAnimator(customAnimator, "RightArm, Override", "HKShootR", playbackRateParam, duration * 0.7f, 0.15f * duration);
                }



                GenericDamageOrb orb = CreateArrowOrb();
                orb.origin = muzzleTransform.position;
                orb.damageValue = damageStat * 1f;
                orb.isCrit = RollCrit();
                orb.teamIndex = TeamComponent.GetObjectTeam(base.gameObject);
                orb.attacker = base.gameObject;
                orb.target = hurtBox;
                orb.damageColorIndex = DamageColorIndex.Default;
                orb.procChainMask = default;
                orb.procCoefficient = 1f;
                orb.speed = 100;

                AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Missile_SFX, this.gameObject);
                OrbManager.instance.AddOrb(orb);
                base.characterBody.AddSpreadBloom(0.15f);
                characterBody.SetAimTimer(2f);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            writer.Write(HurtBoxReference.FromHurtBox(this.initialOrbTarget));
        }

        // Token: 0x06000E6F RID: 3695 RVA: 0x0003E6A4 File Offset: 0x0003C8A4
        public override void OnDeserialize(NetworkReader reader)
        {
            this.initialOrbTarget = reader.ReadHurtBoxReference().ResolveHurtBox();
        }
    }
}