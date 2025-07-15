using EntityStates;
using VileMod.Survivors.Vile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.Survivors.Vile.Components;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class CherryBlast : BaseSkillState
    {
        public static float damageCoefficient = HenryStaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.6f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 400f;
        public static float recoil = 3f;
        public static float range = 200f;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerBanditPistol");
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
        private string muzzleString = "Weapon";

        private VileComponent VC;

        public override void OnEnter()
        {
            base.OnEnter();

            animator = GetModelAnimator();
            characterBody.SetAimTimer(2f);
            characterBody.isSprinting = false;
            VC = GetComponent<VileComponent>();

            shootDelay = 1.5f;
            maxShootDelay = 1.5f;
            minShootDelay = 0.15f;
            spinLevel = VC.GetBaseHeatValue();
            stopwatch = 999f; // for immediate fire

            if (NetworkServer.active)
            {
                if (!characterBody.HasBuff(VileBuffs.PrimaryHeatBuff))
                {
                    characterBody.AddBuff(VileBuffs.PrimaryHeatBuff);
                }
            }

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

                Debug.Log("spinLevel: " + spinLevel);

                PlayCrossfade("Gesture, Override", "CannonShoot", "attackSpeed", shootDelay, 0.05f);
            }

            // Encerrar estado quando soltar botão
            if (!inputBank.skill1.down && base.fixedAge >= 0.1f)
            {
                //this.outer.SetNextStateToMain();
                CherryBlastEnd CBE = new CherryBlastEnd();
                outer.SetNextState(CBE);
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
                    minSpread = Mathf.Lerp(1f, 0.1f, spinLevel),
                    maxSpread = Mathf.Lerp(2f, 0.3f, spinLevel),
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

                // Recoil dinâmico
                float recoilFactor = Mathf.Lerp(0.1f, 1.5f, spinLevel);
                AddRecoil(-0.5f * recoilFactor, -1f * recoilFactor, 0.5f * recoilFactor, 1f * recoilFactor);

                // Som e tiro
                //Util.PlaySound(Sounds.vileCherryBlast, gameObject);
                bullet.Fire();
            }
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}