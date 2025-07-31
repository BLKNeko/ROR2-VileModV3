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
    public class MameqController : UnitMoveController
    {

        public override void Start()
        {
            base.Start();

            minDistance = 1f;
            maxDistance = 35f;
            frontSafeDistance = 10f;
            backToDistance = 7f;
            rotationSpeed = 5f;
            shouldAnimate = false;

            moveSpeed += (ownerBody.moveSpeed * 2f) + 1f;
            damageCoefficient = 1f;
            FireCooldown = 0.4f; // Tempo de recarga entre disparos

            enemyCheckRadius = 0.1f;

            firePoint = projectileController.ghost.gameObject.transform;

            groundOffset = 6f; // Quanto acima do chão o projetil deve ficar

            shouldFollowGroundOffset = true; // Deve seguir o offset do chão
            shouldRotateY = true; // Deve rotacionar no eixo Y

        }


        public override void FireAttack()
        {

            //Nothing
            //Should Just Follow, beccause BUFF should be applied on spawn

        }

    }
}