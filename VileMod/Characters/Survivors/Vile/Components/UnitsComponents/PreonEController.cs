using HG;
using RoR2;
using RoR2.Projectile;
using System;
using System.Linq;
using UnityEngine;
using static Rewired.ComponentControls.Effects.RotateAroundAxis;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem.PlaybackState;
using static RoR2.BulletAttack;

namespace VileMod.Characters.Survivors.Vile.Components.UnitsComponents
{
    public class PreonEController : UnitMoveController
    {

        public override void Start()
        {
            base.Start();

            moveSpeed += ownerBody.moveSpeed * 1.5f;
            damageCoefficient = 2f;
            FireCooldown = 0.5f; // Tempo de recarga entre disparos

            enemyCheckRadius = 35f;
            firePoint = FindChildByName(projectileController.ghost.gameObject.transform, "PreonEShootPoint");

            groundOffset = 0.1f; // Quanto acima do chão o projetil deve ficar

        }


        public override void FireAttack()
        {

            SetState(false, false, true); // shooting

            var attack = new BulletAttack
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
                isCrit = ownerBody.RollCrit(),
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
                // callback para impedir self-hit
                hitCallback = HitCallback,
            };

            attack.Fire();
            

        }

        private bool HitCallback(BulletAttack bulletAttack,ref BulletHit hit)
        {
            //Debug.Log("HitCallback--");

            if (hit.hitHurtBox != null && hit.hitHurtBox.healthComponent != null)
            {
                //Debug.Log($"Hit: {hit.hitHurtBox.healthComponent.body.gameObject.name}"); // Log do acerto

                
                CharacterBody hitBody = hit.hitHurtBox.healthComponent.body;
                if (hitBody.teamComponent.teamIndex == ownerBody.teamComponent.teamIndex) // é o time do próprio dono
                {
                    //Debug.Log($"Hit: {hitBody.gameObject.name} é o próprio dono, cancelando acerto."); // Log do cancelamento
                    return false; // cancela esse acerto
                }
            }
            //Debug.Log("NormalCall--");
            var result = BulletAttack.defaultHitCallback(bulletAttack, ref hit); // Chama o callback padrão para manter o comportamento normal
            return result; // permite normalmente
        }


    }
}