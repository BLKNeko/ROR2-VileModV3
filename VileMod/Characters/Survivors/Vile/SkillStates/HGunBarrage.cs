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
    public class HGunBarrage : BaseSkillState
    {
        public static float damageCoefficient = HenryStaticValues.gunDamageCoefficient;
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

            fireInterval = 0.2f - attackSpeedStat * 0.1f; // Adjust fire interval based on attack speed

            fireInterval = Mathf.Clamp(fireInterval, 0.1f, 0.2f);

            fireTimer = 0f;
            missilesFired = 0;

            //totalMissiles = Mathf.RoundToInt(10f + VC.GetBaseHeatValue() * 5f + VC.GetBaseOverHeatValue() * 10f);
            totalMissiles = Mathf.RoundToInt(10f + (characterBody.level / 2) + (damageStat / 10f));


        }

        public override void OnExit()
        {

            base.OnExit();
        }

        protected virtual GenericDamageOrb CreateArrowOrb()
        {
            return new HomingTorpedoOrb();
        }

        //private void FireHT()
        //{
        //    if (!this.hasFired)
        //    {

        //        this.hasFired = true;

        //        if (NetworkServer.active)
        //        {

                    

        //            //PlayAnimation("Gesture, Override", "XBusterChargeAttack", "attackSpeed", this.duration);

        //            //Debug.Log("Create arrow:" + CreateArrowOrb());

        //            //if (XConfig.enableVoiceBool.Value)
        //            //{
        //            //    AkSoundEngine.PostEvent(XStaticValues.X_HomingTorpedo_VSFX, this.gameObject);
        //            //}
        //            //AkSoundEngine.PostEvent(XStaticValues.X_HomingTorpedo_SFX, this.gameObject);

        //            GenericDamageOrb genericDamageOrb = this.CreateArrowOrb();
        //            genericDamageOrb.damageValue = damageCoefficient * damageStat;
        //            genericDamageOrb.isCrit = RollCrit();
        //            genericDamageOrb.teamIndex = TeamComponent.GetObjectTeam(base.gameObject);
        //            genericDamageOrb.attacker = base.gameObject;
        //            genericDamageOrb.procCoefficient = procCoefficient;
        //            genericDamageOrb.damageType |= DamageType.Generic;
        //            genericDamageOrb.damageType |= DamageType.Stun1s;
        //            genericDamageOrb.damageType |= DamageTypeCombo.GenericSecondary;
        //            genericDamageOrb.damageColorIndex = DamageColorIndex.Default;

        //            //genericDamageOrb.damageType = DamageType.ApplyMercExpose;

        //            //Debug.Log("GenereciDamageOrb:" + genericDamageOrb);



        //            for (int i = 0; i < missleAmount; i++)
        //            {
        //                HurtBox hurtBox = this.initialOrbTarget;
        //                if (hurtBox)
        //                {
        //                    Transform muzzleTransform = this.childLocator.FindChild(this.muzzleString);
        //                    Transform muzzleTransform2 = this.childLocator.FindChild(this.muzzleString2);

        //                    if(i % 2 == 0)
        //                    {
        //                        if (muzzleTransform)
        //                        {
        //                            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab, base.gameObject, this.muzzleString, true);

        //                            GenericDamageOrb orb = new GenericDamageOrb
        //                            {
        //                                origin = muzzleTransform.position,
        //                                damageValue = damageStat * 1f,
        //                                isCrit = RollCrit(),
        //                                teamIndex = TeamComponent.GetObjectTeam(base.gameObject),
        //                                attacker = base.gameObject,
        //                                target = hurtBox,
        //                                damageColorIndex = DamageColorIndex.Default,
        //                                procChainMask = default,
        //                                procCoefficient = 1f,
        //                                speed = 100f
        //                            };

        //                            OrbManager.instance.AddOrb(orb);
        //                            base.characterBody.AddSpreadBloom(0.15f);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (muzzleTransform2)
        //                        {
        //                            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab, base.gameObject, this.muzzleString2, true);

        //                            GenericDamageOrb orb = new GenericDamageOrb
        //                            {
        //                                origin = muzzleTransform2.position,
        //                                damageValue = damageStat * 1f,
        //                                isCrit = RollCrit(),
        //                                teamIndex = TeamComponent.GetObjectTeam(base.gameObject),
        //                                attacker = base.gameObject,
        //                                target = hurtBox,
        //                                damageColorIndex = DamageColorIndex.Default,
        //                                procChainMask = default,
        //                                procCoefficient = 1f,
        //                                speed = 100f
        //                            };

        //                            OrbManager.instance.AddOrb(orb);
        //                            base.characterBody.AddSpreadBloom(0.15f);
        //                        }
        //                    }

                            
        //                }
        //            }

        //            //Debug.Log("HurbBox2:" + hurtBox);



                    
        //        }

                    
                
                
        //    }
        //}



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