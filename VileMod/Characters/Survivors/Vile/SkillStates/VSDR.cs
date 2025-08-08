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
    public class VSDR : BaseSkillState
    {
        public static float damageCoefficient = VileStaticValues.VSeaDragonRageDamageCoefficient;
        public static float procCoefficient = 0.4f;
        public static float baseDuration = 1f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 300f;
        public static float recoil = 3f;
        public static float range = 55f;
        public static GameObject tracerEffectPrefab = VileAssets.vileSDRTracerPrefab;
        public static GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/HitsparkFMJ");

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
        private BulletAttack FireWave2BulletAttack;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();

            muzzleString = "VLCannonMuzz";

            this.modelTransform = base.GetModelTransform();
            this.animator = base.GetModelAnimator();
            VC = GetComponent<VileComponent>();

            if (this.modelTransform)
            {
                this.childLocator = this.modelTransform.GetComponent<ChildLocator>();
            }

            if (base.characterBody)
            {
                base.characterBody.SetAimTimer(this.duration);
            }

            //childLocator.FindChildGameObject("VSDRFlamesVFX").SetActive(true);

            fireInterval = 0.06f - attackSpeedStat * 0.01f; // Adjust fire interval based on attack speed

            fireInterval = Mathf.Clamp(fireInterval, 0.01f, 0.06f);

            fireTimer = 0f;
            missilesFired = 0;

            //totalMissiles = Mathf.RoundToInt(10f + VC.GetBaseHeatValue() * 5f + VC.GetBaseOverHeatValue() * 10f);
            totalMissiles = Mathf.RoundToInt(60f + (characterBody.level) + (damageStat / 5f));

            if (VileConfig.enableVoiceBool.Value)
            {
                AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Attack, this.gameObject);
            }

            PlayCrossfade("Gesture, Override", "VileCannonHold", playbackRateParam, duration * 0.4f, 0.1f * duration);


        }

        public override void OnExit()
        {
            //childLocator.FindChildGameObject("VSDRFlamesVFX").SetActive(false);

            PlayCrossfade("Gesture, Override", "BufferEmpty", playbackRateParam, duration * 0.1f, 0.4f * duration);

            base.OnExit();
        }

        protected virtual GenericDamageOrb CreateArrowOrb()
        {
            return new HomingTorpedoOrb();
        }



        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!isAuthority || missilesFired >= totalMissiles)
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
            if (isAuthority)
            {

                characterBody.SetAimTimer(2f);
                characterBody.AddSpreadBloom(0.8f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);
                //PlayAnimation("Gesture, Override", "XBusterAttack", "attackSpeed", this.duration);

                Ray aimRay = GetAimRay();
                //AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                if (index == 0)
                    AkSoundEngine.PostEvent(VileStaticValues.Play_Fire_SFX, this.gameObject);

                if (index % 10 == 0 && index > 10 && index < 90)
                    AkSoundEngine.PostEvent(VileStaticValues.Play_Fire_SFX, this.gameObject);

                FireWave2BulletAttack = new BulletAttack();
                FireWave2BulletAttack.bulletCount = 1;
                FireWave2BulletAttack.aimVector = aimRay.direction;
                FireWave2BulletAttack.origin = aimRay.origin;
                FireWave2BulletAttack.damage = damageCoefficient * damageStat;
                FireWave2BulletAttack.damageColorIndex = DamageColorIndex.Default;
                FireWave2BulletAttack.damageType = DamageTypeCombo.GenericSpecial | DamageType.Freeze2s;
                FireWave2BulletAttack.falloffModel = BulletAttack.FalloffModel.None;
                FireWave2BulletAttack.maxDistance = range;
                FireWave2BulletAttack.force = force;
                FireWave2BulletAttack.hitMask = LayerIndex.CommonMasks.bullet;
                FireWave2BulletAttack.minSpread = 0f;
                FireWave2BulletAttack.maxSpread = 0f;
                FireWave2BulletAttack.isCrit = RollCrit();
                FireWave2BulletAttack.owner = gameObject;
                FireWave2BulletAttack.muzzleName = muzzleString;
                FireWave2BulletAttack.smartCollision = true;
                FireWave2BulletAttack.procChainMask = default;
                FireWave2BulletAttack.procCoefficient = procCoefficient;
                FireWave2BulletAttack.radius = 2f;
                FireWave2BulletAttack.sniper = false;
                FireWave2BulletAttack.stopperMask = LayerIndex.CommonMasks.bullet;
                FireWave2BulletAttack.weapon = null;
                FireWave2BulletAttack.tracerEffectPrefab = tracerEffectPrefab;
                FireWave2BulletAttack.spreadPitchScale = 1f;
                FireWave2BulletAttack.spreadYawScale = 1f;
                FireWave2BulletAttack.queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
                FireWave2BulletAttack.hitEffectPrefab = hitEffectPrefab;

                FireWave2BulletAttack.Fire();

                if (index % 2 == 0 || index == 0)
                {
                    EffectManager.SpawnEffect(VileAssets.SDREffect, new EffectData
                    {
                        origin = childLocator.FindChildGameObject("VLCannonMuzz").transform.position,
                        rotation = Quaternion.LookRotation(aimRay.direction),
                        scale = 1f
                    }, true);
                }
                    

            }
        }
    

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }


    }
}