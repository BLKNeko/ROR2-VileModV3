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

namespace VileMod.Characters.Survivors.Vile.Components.UnitsComponents
{
    public class MameqController : UnitMoveController
    {

        private float vsfxTimerCooldown = 5f; // Tempo de recarga do efeito sonoro de movimento
        private float vsfxTimer;

        public override void Start()
        {
            base.Start();

            minDistance = 1f;
            maxDistance = 35f;
            frontSafeDistance = 10f;
            backToDistance = 7f;
            rotationSpeed = 5f;
            shouldAnimate = false;

            moveSpeed += ownerBody.moveSpeed * 2f + 1f;
            damageCoefficient = 1f;
            FireCooldown = 0.4f; // Tempo de recarga entre disparos

            enemyCheckRadius = 0.1f;

            firePoint = projectileController.ghost.gameObject.transform;

            groundOffset = 6f; // Quanto acima do chão o projetil deve ficar

            shouldFollowGroundOffset = true; // Deve seguir o offset do chão
            shouldRotateY = true; // Deve rotacionar no eixo Y

            vsfxTimer = vsfxTimerCooldown - 2f; // Inicia o temporizador para tocar o efeito sonoro de movimento após 2 segundos

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (vsfxTimer >= vsfxTimerCooldown)
            {
                vsfxTimer = 0f; // Reinicia o temporizador
                AkSoundEngine.PostEvent(VileStaticValues.Play_MMQVSFX, gameObject); // Toca o efeito sonoro de movimento
            }
            else
            {
                vsfxTimer += Time.fixedDeltaTime; // Incrementa o temporizador
            }
        }


        public override void FireAttack()
        {

            //Nothing
            //Should Just Follow, beccause BUFF should be applied on spawn

        }

    }
}