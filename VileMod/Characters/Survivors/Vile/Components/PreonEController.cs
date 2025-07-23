using HG;
using RoR2;
using RoR2.Projectile;
using System;
using System.Linq;
using UnityEngine;
using static Rewired.ComponentControls.Effects.RotateAroundAxis;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem.PlaybackState;

namespace VileMod.Survivors.Vile.Components
{
    public class PreonEController : UnitMoveController
    {

        public override void Start()
        {
            base.Start();

            moveSpeed += ownerBody.moveSpeed * 1.5f;
            damageCoeficient = 2f;
            FireCooldown = 0.5f; // Tempo de recarga entre disparos

            enemyCheckRadius = 35f;
            firePoint = FindChildByName(projectileController.ghost.gameObject.transform, "PreonEShootPoint");

            groundOffset = 0.1f; // Quanto acima do chão o projetil deve ficar

            EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/TeleportOutBoom"), new EffectData
            {
                origin = transform.position,
                rotation = transform.rotation
            }, transmit: true);

        }


        public override void FireAttack()
        {

            new BulletAttack
            {
                bulletCount = 1,
                aimVector = shootDir,
                origin = firePoint.position,
                damage = damageCoeficient * ownerBody.damage,
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageType.Generic,
                falloffModel = BulletAttack.FalloffModel.None,
                maxDistance = 1000f,
                force = 800f,
                hitMask = LayerIndex.CommonMasks.bullet,
                minSpread = 0f,
                maxSpread = 0f,
                isCrit = false,
                owner = gameObject,
                smartCollision = true,
                procChainMask = default,
                procCoefficient = 1f,
                radius = 0.75f,
                sniper = false,
                stopperMask = LayerIndex.CommonMasks.bullet,
                weapon = null,
                tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/tracers/TracerBanditShotgun"),
                spreadPitchScale = 1f,
                spreadYawScale = 1f,
                queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FireShotgun.hitEffectPrefab,
            }.Fire();

        }

        void OnDestroy()
        {
            EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/TeleportOutBoom"), new EffectData
            {
                origin = transform.position,
                rotation = transform.rotation
            }, transmit: true);
        }

    }
}