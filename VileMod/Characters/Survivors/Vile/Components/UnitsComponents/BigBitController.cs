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
using VileMod.Survivors.Vile;
using UnityEngine.Networking;

namespace VileMod.Characters.Survivors.Vile.Components.UnitsComponents
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

            moveSpeed += ownerBody.moveSpeed * 2f + 2f;
            damageCoefficient = VileStaticValues.UnitBigBitDamageCoefficient;
            FireCooldown = 0.4f; // Tempo de recarga entre disparos

            enemyCheckRadius = 50f;

            firePoint = projectileController.ghost.gameObject.transform;

            groundOffset = 8f; // Quanto acima do chão o projetil deve ficar

            shouldFollowGroundOffset = true; // Deve seguir o offset do chão
            shouldRotateY = true; // Deve rotacionar no eixo Y

        }


        public override void FireAttack()
        {
            //Debug.Log($"Unit BigBitFire");

            if (NetworkServer.active)
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
                    maxDistance = 300f,
                    force = 100f,
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
                    hitCallback = HitCallback, // Callback para impedir self-hit
                }.Fire();

                AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Simple_Bullet, this.gameObject);

            }

            

        }

        private bool HitCallback(BulletAttack bulletAttack, ref BulletHit hit)
        {
            //Debug.Log("HitCallback--");

            //Debug.Log($"bulletAttack: {bulletAttack}");
            //Debug.Log($"bulletAttack.owner: {bulletAttack.owner}");
            //Debug.Log($"bulletAttack.owner.GetComponent<TeamComponent>(): {bulletAttack.owner.GetComponent<TeamComponent>()}");
            //Debug.Log($"bulletAttack.owner.GetComponent<TeamComponent>().teamIndex: {bulletAttack.owner.GetComponent<TeamComponent>().teamIndex}");
            //Debug.Log($"hit.hitHurtBox: {hit.hitHurtBox}");
            //Debug.Log($"bulletAttack.filterCallback: {bulletAttack.filterCallback}");

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