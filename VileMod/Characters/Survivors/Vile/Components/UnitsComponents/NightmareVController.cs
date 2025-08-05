using HG;
using RoR2;
using RoR2.Projectile;
using System;
using System.Linq;
using UnityEngine;
using static Rewired.ComponentControls.Effects.RotateAroundAxis;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem.PlaybackState;
using VileMod.Survivors.Vile;
using R2API;
using VileMod.Survivors.Vile.Components;

namespace VileMod.Characters.Survivors.Vile.Components.UnitsComponents
{
    public class NightmareVController : UnitMoveController
    {

        public override void Start()
        {
            base.Start();

            minDistance = 1f;
            maxDistance = 40f;
            backToDistance = 20f;
            frontSafeDistance = 15f;

            moveSpeed += ownerBody.moveSpeed * 1.5f;
            damageCoefficient = VileStaticValues.UnitNightmareVDamageCoefficient;
            FireCooldown = 1f; // Tempo de recarga entre disparos
            shouldMoveTowardEnemy = true; // Deve se mover em direção ao inimigo

            enemyCheckRadius = 50f;
            firePoint = projectileController.ghost.gameObject.transform;

            groundOffset = 1f; // Quanto acima do chão o projetil deve ficar


        }


        public override void FireAttack()
        {

            SetState(false, false, true); // shooting

            if (ownerBody.hasAuthority)
            {
                if (overlapAttack != null)
                {

                    //Util.PlaySound(Sounds.xChargeShot, base.gameObject);

                    // Carrega efeito de impacto
                    GameObject effectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/DaggerImpact");
                    EffectIndex effectIndex = EffectCatalog.FindEffectIndexFromPrefab(effectPrefab);

                    BlastAttack CSBlastAttack = new BlastAttack();
                    CSBlastAttack.attacker = ownerBody.gameObject;
                    CSBlastAttack.inflictor = gameObject;
                    CSBlastAttack.teamIndex = TeamComponent.GetObjectTeam(ownerBody.gameObject);
                    CSBlastAttack.baseDamage = damageCoefficient * ownerBody.damage;
                    CSBlastAttack.baseForce = 500f;
                    CSBlastAttack.position = transform.position;
                    CSBlastAttack.radius = 3f;
                    CSBlastAttack.bonusForce = new Vector3(1f, 1f, 1f);
                    CSBlastAttack.damageType |= DamageType.Stun1s;
                    CSBlastAttack.damageType |= DamageType.BypassArmor;
                    CSBlastAttack.damageType |= DamageType.PoisonOnHit;
                    CSBlastAttack.damageType |= DamageType.WeakOnHit;
                    CSBlastAttack.damageColorIndex = DamageColorIndex.Default;
                    CSBlastAttack.procCoefficient = 1f;
                    CSBlastAttack.procChainMask = default;
                    CSBlastAttack.crit = ownerBody.RollCrit();
                    CSBlastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                    CSBlastAttack.impactEffect = effectIndex;

                    CSBlastAttack.AddModdedDamageType(VileCustomDamageType.NightmareDamage);

                    //CSBlastAttack.Fire();

                    //Debug.Log($"NightmareVController: FireAttack - Damage: {CSBlastAttack.baseDamage}, Position: {CSBlastAttack.position}, Radius: {CSBlastAttack.radius}, DamageType: {CSBlastAttack.damageType}");

                    BlastAttack.Result result = CSBlastAttack.Fire();

                    // Aplica debuff apenas se algum inimigo foi atingido
                    //if (result.hitCount > 0 && enemyHurtbox)
                    //{
                    //    enemyHurtbox.healthComponent.body.AddBuff(VileBuffs.nightmareVirusDebuff);
                    //}

                }


            }

            //enemyHurtbox.healthComponent.body.AddBuff(VileBuffs.nightmareVirusDebuff);

        }


    }
}