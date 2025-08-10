using EntityStates;
using VileMod.Survivors.Vile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.Survivors.Vile.Components;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class Triple7 : BaseSkillState
    {
        public static float damageCoefficient = VileStaticValues.Triple7DamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.4f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 100f;
        public static float recoil = 5f;
        public static float range = 80f;
        //public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerBanditPistol");
        public static GameObject tracerEffectPrefab;
        public static GameObject hitEffectPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/Hitspark1");


        public static int buffSkillIndex;
        public static bool heat;
        public static float Chilldelay;

        private float shootDelay;
        private float maxShootDelay;
        private float minShootDelay;
        private float stopwatch;
        private float spinLevel;
        private Animator animator;
        private string muzzleString = "VLVulcanMuzz";
        private string playbackRateParam = "ShootGun.playbackRate";

        private VileComponent VC;

        public override void OnEnter()
        {
            base.OnEnter();

            animator = GetModelAnimator();
            characterBody.SetAimTimer(2f);
            characterBody.isSprinting = false;
            VC = GetComponent<VileComponent>();

            shootDelay = 0.5f;
            maxShootDelay = 0.5f;
            minShootDelay = 0.04f;
            spinLevel = VC.GetBaseHeatValue();
            stopwatch = 999f; // for immediate fire

            tracerEffectPrefab = VileAssets.vileCyanTracerPrefab;

        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            // Atualiza rotação da arma
            if (inputBank.skill1.down)
                spinLevel += Time.fixedDeltaTime * 0.25f;

            spinLevel = Mathf.Clamp01(spinLevel);
            shootDelay = Mathf.Lerp(maxShootDelay, minShootDelay, spinLevel);

            // Tempo entre disparos
            stopwatch += Time.fixedDeltaTime;

            if (inputBank.skill1.down && stopwatch >= shootDelay)
            {
                FireBullet();
                stopwatch = 0f;
                characterBody.SetAimTimer(1f);
                //characterBody.isSprinting = false;

                //Debug.Log("spinLevel: " + spinLevel);

                //PlayCrossfade("LeftArm, Override", "VulcanLoop", playbackRateParam, shootDelay * 0.7f, 0.1f * shootDelay);

                if (characterBody.HasBuff(VileBuffs.OverHeatDebuff))
                {
                    //CherryBlastEnd CBE = new CherryBlastEnd();
                    AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Overheat_SFX, this.gameObject);
                    outer.SetNextState(new Triple7End());
                }

            }

            // Encerrar estado quando soltar botão
            if (!inputBank.skill1.down && base.fixedAge >= 0.1f)
            {
                //this.outer.SetNextStateToMain();
                //CherryBlastEnd CBE = new CherryBlastEnd();
                outer.SetNextState(new Triple7End());
            }
        }

        private void FireBullet()
        {
            Ray aimRay = GetAimRay();

            if (base.isAuthority)
            {
                BulletAttack bullet = new BulletAttack
                {
                    owner = gameObject,
                    weapon = null,
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    minSpread = Mathf.Lerp(1f, 2f, spinLevel),
                    maxSpread = Mathf.Lerp(2f, 7f, spinLevel),
                    damage = damageCoefficient * damageStat,
                    damageType = DamageTypeCombo.GenericPrimary,
                    force = force,
                    falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                    maxDistance = range,
                    radius = 0.75f,
                    smartCollision = false,
                    stopperMask = LayerIndex.CommonMasks.bullet,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    procChainMask = default,
                    procCoefficient = procCoefficient,
                    tracerEffectPrefab = tracerEffectPrefab,
                    hitEffectPrefab = hitEffectPrefab,
                    damageColorIndex = DamageColorIndex.Default,
                    isCrit = RollCrit(),
                    bulletCount = 1,
                    muzzleName = muzzleString,
                    spreadPitchScale = 0.1f,
                    spreadYawScale = 1f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal
                };

                // Definir tipo de dano
                //bullet.damageType = buffSkillIndex switch
                //{
                //    0 => Util.CheckRoll(5f, characterBody.master) ? DamageType.SlowOnHit : DamageType.Generic,
                //    1 => Util.CheckRoll(8f, characterBody.master) ? DamageType.Stun1s : DamageType.Generic,
                //    2 => Util.CheckRoll(8f, characterBody.master) ? DamageType.Shock5s : DamageType.Generic,
                //    3 => Util.CheckRoll(8f, characterBody.master) ? DamageType.IgniteOnHit : DamageType.Generic,
                //    _ => DamageType.Generic
                //};

                if (characterBody.HasBuff(VileBuffs.PrimaryIceBuff))
                {
                    bullet.damageType |= DamageType.Freeze2s;

                    VC.SetElementValues(-0.07f, 0f, 0f, false, true, true);
                }

                if (characterBody.HasBuff(VileBuffs.PrimaryShockBuff))
                {
                    bullet.damageType |= DamageType.Shock5s;

                    VC.SetElementValues(0f, -0.1f, 0f, true, false, true);
                }

                if (characterBody.HasBuff(VileBuffs.PrimaryFlameBuff))
                {
                    bullet.damageType |= DamageType.IgniteOnHit;

                    VC.SetElementValues(0f, 0f, -0.08f, true, true, false);
                }



                // Recoil dinâmico
                float recoilFactor = Mathf.Lerp(0.1f, 1f, spinLevel * 2f);
                AddRecoil(-0.5f * recoilFactor, -1f * recoilFactor, 0.5f * recoilFactor, 1f * recoilFactor);
                characterBody.AddSpreadBloom(recoilFactor * 2f);

                //ExtraHeat
                VC.SetExtraHeatValues(0.025f);

                //Effect
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);

                // Som e tiro
                AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Cherry_Blast, this.gameObject);
                bullet.Fire();
            }
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}