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
    public class MetComController : UnitMoveControllerMC
    {
        private CharacterModel model;

        public override void Start()
        {
            base.Start();

            moveSpeed += ownerBody.moveSpeed * 2f;
            healCoefficient = 2f;
            FireCooldown = 4f; // Tempo de recarga entre disparos

            allyCheckRadius = 15f;

            groundOffset = 0.1f; // Quanto acima do chão o projetil deve ficar

            isShield = true;

            model = ownerBody.GetComponent<ModelLocator>().modelTransform.gameObject.GetComponent<CharacterModel>();

            EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/TeleportOutBoom"), new EffectData
            {
                origin = transform.position,
                rotation = transform.rotation
            }, transmit: true);

        }


        public override void StartHeal(CharacterBody body)
        {

            if (body != null)
            {
                body.healthComponent.RechargeShield(healCoefficient * ownerBody.damage);

                if (body.HasBuff(VileBuffs.RideArmorEnabledBuff))
                {

                    //Debug.Log($"Body RideArmorComp: {body.GetComponent<VileRideArmorComponent>()}");

                    if (body.GetComponent<VileRideArmorComponent>())
                    {
                        body.GetComponent<VileRideArmorComponent>().AddShieldRA((healCoefficient * ownerBody.damage) * 0.5f);
                    }

                }

                if (ownerBody.transform)
                {
                    TemporaryOverlayInstance temporaryOverlayInstance = TemporaryOverlayManager.AddOverlay(ownerBody.transform.gameObject);
                    temporaryOverlayInstance.duration = 1f;
                    temporaryOverlayInstance.animateShaderAlpha = true;
                    temporaryOverlayInstance.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    temporaryOverlayInstance.destroyComponentOnEnd = true;
                    temporaryOverlayInstance.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matMercEnergized");
                    temporaryOverlayInstance.inspectorCharacterModel = model;
                    temporaryOverlayInstance.AddToCharacterModel(model);
                }


            }

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