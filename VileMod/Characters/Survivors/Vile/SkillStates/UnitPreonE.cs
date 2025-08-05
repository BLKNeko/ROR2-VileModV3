using EntityStates;
using VileMod.Survivors.Vile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.Survivors.Vile.Components;
using RoR2.Projectile;
using VileMod.Modules;
using ExtraSkillSlots;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class UnitPreonE : BaseSkillState
    {
        public static float damageCoefficient = VileStaticValues.UnitPreonEDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.8f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 1000f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        private VileComponent VC;
        private VileBoltComponent VBC;
        private int boltCost;
        private ExtraSkillLocator extraSkillLocator;
        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "Muzzle";
            boltCost = VileStaticValues.UnitPreonEBoltCost;
            VC = GetComponent<VileComponent>();
            VBC = GetComponent<VileBoltComponent>();
            extraSkillLocator = GetComponent<ExtraSkillLocator>();

            if (VBC.GetBoltValue() < boltCost)
            {
                //Play sound
                AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Error, this.gameObject);

                Chat.AddMessage(
                    $"<color=#FFA500>You need at least " +
                    $"<color=#C0C0C0>{boltCost}</color> " +
                    $"<color=#A020F0>Vile</color> <color=#C0C0C0>Bolts</color> to call " +
                    $"<color=#00BFFF>Preon Elite Unit</color>! You currently have " +
                    $"<color=#C0C0C0>{VBC.GetBoltValue()}</color> " +
                    $"<color=#A020F0>Vile</color> <color=#C0C0C0>Bolts</color>.</color>"
                );

                //Reset cooldown
                outer.SetNextStateToMain();
                extraSkillLocator.extraSecond.Reset();
                return;
            }

            VBC.ChangeBoltValue(-boltCost);

            Fire();

        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        private void Fire()
        {
            if (!hasFired)
            {
                hasFired = true;

                if (isAuthority)
                {

                    characterBody.AddSpreadBloom(0.8f);
                    EffectManager.SimpleMuzzleFlash(EntityStates.Mage.Weapon.IceNova.impactEffectPrefab, gameObject, muzzleString, true);
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    if (VileConfig.enableVoiceBool.Value)
                    {
                        AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Come_Here, this.gameObject);
                    }

                    Ray aimRay = GetAimRay();

                    Vector3 spawnPos = aimRay.origin + aimRay.direction * 3f;

                    // Faz um raycast para baixo
                    if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hitInfo, 100f, LayerIndex.world.mask))
                    {
                        spawnPos = hitInfo.point;
                        spawnPos.y += 0.5f;
                    }

                    FireProjectileInfo ShotgunProjectille = new FireProjectileInfo();
                    ShotgunProjectille.projectilePrefab = VileAssets.unitPreonEPrefab;
                    ShotgunProjectille.position = spawnPos;
                    ShotgunProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    ShotgunProjectille.owner = gameObject;
                    ShotgunProjectille.damage = damageCoefficient;
                    ShotgunProjectille.force = force;
                    ShotgunProjectille.crit = RollCrit();
                    ShotgunProjectille.damageColorIndex = DamageColorIndex.Default;
                    ShotgunProjectille.speedOverride = 0f;

                    ProjectileManager.instance.FireProjectile(ShotgunProjectille);

                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}