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
using UnityEngine.Networking;
using VileMod.Survivors.Vile;

namespace VileMod.Characters.Survivors.Vile.Components.UnitsComponents
{
    public class GunVoltController : UnitMoveController
    {
        private Transform firePoint2;

        private float fireTimer;
        private float fireInterval = 0.2f;
        private int missilesFired;
        private int totalMissiles;

        private bool canAttack = false;

        public override void Start()
        {
            base.Start();

            moveSpeed += ownerBody.moveSpeed * 1.5f;
            damageCoefficient = VileStaticValues.UnitGunVoltDamageCoefficient;
            FireCooldown = 2f; // Tempo de recarga entre disparos

            enemyCheckRadius = 40f;
            firePoint = FindChildByName(projectileController.ghost.gameObject.transform, "HRC_em72_00_body:ND_missile_R");
            firePoint2 = FindChildByName(projectileController.ghost.gameObject.transform, "HRC_em72_00_body:ND_missile_L");
            shouldFilterByLOS = false; // Não filtrar por linha de visão

            groundOffset = 0.1f; // Quanto acima do chão o projetil deve ficar

            fireInterval = 0.5f - ownerBody.attackSpeed * 0.1f; // Adjust fire interval based on attack speed

            fireInterval = Mathf.Clamp(fireInterval, 0.2f, 0.4f);

            fireTimer = 0f;
            missilesFired = 0;


            totalMissiles = 2;

        }

        public override void FixedUpdate()
        {

            if (canAttack)
            {

                fireTimer -= Time.fixedDeltaTime;
                if (fireTimer <= 0f)
                {
                    SetState(false, false, true); // shooting
                    FireMissile(missilesFired);
                    missilesFired++;
                    fireTimer = fireInterval;
                }

                // Transição opcional quando terminar
                if (missilesFired >= totalMissiles)
                {
                    canAttack = false;
                    missilesFired = 0;
                }

            }
            


            base.FixedUpdate();
        }


        public override void FireAttack()
        {
            canAttack = true;

        }

        private void FireMissile(int index)
        {

            Transform effectT = (index % 2 == 0) ? this.firePoint : this.firePoint2;

            EffectManager.SpawnEffect(EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab, new EffectData
            {
                origin = effectT.position,
                rotation = Quaternion.LookRotation(shootDir),
                scale = 1f
            }, true);

            FireProjectileInfo VShotgunIceProjectille = new FireProjectileInfo();
            VShotgunIceProjectille.projectilePrefab = VileAssets.GVMissileProjectile;
            VShotgunIceProjectille.position = effectT.position;
            VShotgunIceProjectille.rotation = Util.QuaternionSafeLookRotation(Vector3.up);
            VShotgunIceProjectille.owner = ownerBody.gameObject;
            VShotgunIceProjectille.damage = damageCoefficient * ownerBody.damage;
            VShotgunIceProjectille.force = 800f;
            VShotgunIceProjectille.crit = ownerBody.RollCrit();
            //XBusterMediumProjectille.speedOverride = XBusterMediumProjectille.speedOverride * 0.8f;
            VShotgunIceProjectille.damageColorIndex = DamageColorIndex.Default;

            ProjectileManager.instance.FireProjectile(VShotgunIceProjectille);



            AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Missile_SFX, this.gameObject);

        }


    }
}