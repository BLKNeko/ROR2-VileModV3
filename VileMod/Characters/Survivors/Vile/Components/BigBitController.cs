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
    public class BigBitController : UnitMoveController
    {

        public override void Start()
        {
            base.Start();

            minDistance = 1f;
            maxDistance = 50f;
            frontSafeDistance = 15f;
            backToDistance = 10f;
            rotationSpeed = 5f;

            moveSpeed += (ownerBody.moveSpeed * 2f) + 2f;
            damageCoefficient = 1f;
            FireCooldown = 0.4f; // Tempo de recarga entre disparos

            enemyCheckRadius = 50f;

            firePoint = projectileController.ghost.gameObject.transform;

            groundOffset = 8f; // Quanto acima do chão o projetil deve ficar

            shouldFollowGroundOffset = true; // Deve seguir o offset do chão
            shouldRotateY = true; // Deve rotacionar no eixo Y

        }


        public override void FireAttack()
        {

            new BulletAttack
            {
                bulletCount = 1,
                aimVector = shootDir,
                origin = firePoint.position,
                damage = damageCoefficient * ownerBody.damage,
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

    }
}