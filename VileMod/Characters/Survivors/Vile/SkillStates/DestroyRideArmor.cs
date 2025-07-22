using EntityStates;
using VileMod.Survivors.Vile;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using VileMod.Survivors.Vile.Components;

namespace VileMod.Survivors.Vile.SkillStates
{
    public class DestroyRideArmor : BaseSkillState
    {

        public static float baseDuration = 1f;
        public static float force = 1500f;
        public static float damageCoefficient = HenryStaticValues.gunDamageCoefficient;
        private float duration;
        private VileComponent VC;


        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration;
            characterBody.SetAimTimer(1f);

            //PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);

            VC = GetComponent<VileComponent>();


            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 1.5f * duration);
                characterBody.AddTimedBuff(RoR2Content.Buffs.Intangible, 1.5f * duration);
                characterBody.AddTimedBuff(RoR2Content.Buffs.Immune, 1.5f * duration);

                if (characterBody.HasBuff(VileBuffs.GoliathBuff))
                {
                    characterBody.RemoveBuff(VileBuffs.GoliathBuff);
                }

                if (characterBody.HasBuff(VileBuffs.HawkBuff))
                {
                    characterBody.RemoveBuff(VileBuffs.HawkBuff);
                }

                if (characterBody.HasBuff(VileBuffs.RideArmorEnabledBuff))
                {
                    characterBody.RemoveBuff(VileBuffs.RideArmorEnabledBuff);
                }
            }

            Debug.Log("Destroying Ride Armor state entered");

            EffectManager.SimpleMuzzleFlash(VileAssets.rideExplosionEffect, gameObject, "BasePos", true);

            VC.DestroyRideArmor();

            if (isAuthority)
            {
                Ray aimRay = GetAimRay();
                //AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                //Util.PlaySound(Sounds.xChargeShot, base.gameObject);

                BlastAttack CSBlastAttack = new BlastAttack();
                CSBlastAttack.attacker = base.gameObject;
                CSBlastAttack.inflictor = base.gameObject;
                CSBlastAttack.teamIndex = TeamComponent.GetObjectTeam(base.gameObject);
                CSBlastAttack.baseDamage = damageCoefficient * damageStat;
                CSBlastAttack.baseForce = force;
                CSBlastAttack.position = base.characterBody.corePosition;
                CSBlastAttack.radius = 10f;
                CSBlastAttack.bonusForce = new Vector3(1f, 1f, 1f);
                CSBlastAttack.damageType |= DamageType.Stun1s;
                CSBlastAttack.damageType |= DamageType.BypassArmor;
                CSBlastAttack.damageType |= DamageType.IgniteOnHit;
                CSBlastAttack.damageType |= DamageTypeCombo.GenericSpecial;
                CSBlastAttack.damageColorIndex = DamageColorIndex.Default;

                CSBlastAttack.Fire();
            }

            characterMotor.Jump(2f, 2f, true);

            Vector3 backward = characterDirection.forward * -1f; // Direção para trás do personagem
            Vector3 pushDirection = (backward + Vector3.up).normalized; // para cima + para trás
            float pushForce = 50f;

            characterBody.characterMotor.velocity += pushDirection * pushForce;

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

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}