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
using VileMod.Survivors.Vile.Components;

namespace VileMod.Characters.Survivors.Vile.Components.UnitsComponents
{
    public class MetCurController : UnitMoveControllerMC
    {
        private CharacterModel model;

        public override void Start()
        {
            base.Start();

            moveSpeed += ownerBody.moveSpeed * 2f;
            healCoefficient = 2f;
            FireCooldown = 3f; // Tempo de recarga entre disparos

            allyCheckRadius = 35f;

            groundOffset = 0.1f; // Quanto acima do chão o projetil deve ficar

            model = ownerBody.GetComponent<ModelLocator>().modelTransform.gameObject.GetComponent<CharacterModel>();


        }


        public override void StartHeal(CharacterBody body)
        {

            if (body != null)
            {
                body.healthComponent.Heal(healCoefficient * ownerBody.damage, default, true);

                Debug.Log($"Body Name: {body.name}");
                Debug.Log($"Body Heal: {body.healthComponent.health} + {healCoefficient * ownerBody.damage} = {body.healthComponent.health + healCoefficient * ownerBody.damage}");

                if (body.HasBuff(VileBuffs.RideArmorEnabledBuff))
                {

                    //Debug.Log($"Body RideArmorComp: {body.GetComponent<VileRideArmorComponent>()}");

                    if (body.GetComponent<VileRideArmorComponent>())
                    {
                        body.GetComponent<VileRideArmorComponent>().RepairRideArmor(healCoefficient * ownerBody.damage);
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


    }
}