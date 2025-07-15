using RoR2;
using UnityEngine;
using VileMod.Modules;
using System;
using RoR2.Projectile;

namespace VileMod.Survivors.Vile
{
    public static class VileAssets
    {
        // particle effects
        public static GameObject swordSwingEffect;
        public static GameObject swordHitImpactEffect;

        public static GameObject bombExplosionEffect;

        // networked hit sounds
        public static NetworkSoundEventDef swordHitSoundEvent;

        //UI
        public static GameObject BarPanel;

        //projectiles
        public static GameObject bombProjectilePrefab;
        public static GameObject vileShotgunIcePrefab;

        private static AssetBundle _assetBundle;

        public static void Init(AssetBundle assetBundle)
        {

            _assetBundle = assetBundle;

            swordHitSoundEvent = Content.CreateAndAddNetworkSoundEventDef("HenrySwordHit");

            CreateEffects();

            CreateProjectiles();
        }

        #region effects
        private static void CreateEffects()
        {
            CreateBombExplosionEffect();

            swordSwingEffect = _assetBundle.LoadEffect("HenrySwordSwingEffect", true);
            swordHitImpactEffect = _assetBundle.LoadEffect("ImpactHenrySlash");

            BarPanel = _assetBundle.LoadAsset<GameObject>("BarPanel");

        }

        private static void CreateBombExplosionEffect()
        {
            bombExplosionEffect = _assetBundle.LoadEffect("BombExplosionEffect", "HenryBombExplosion");

            if (!bombExplosionEffect)
                return;

            ShakeEmitter shakeEmitter = bombExplosionEffect.AddComponent<ShakeEmitter>();
            shakeEmitter.amplitudeTimeDecay = true;
            shakeEmitter.duration = 0.5f;
            shakeEmitter.radius = 200f;
            shakeEmitter.scaleShakeRadiusWithLocalScale = false;

            shakeEmitter.wave = new Wave
            {
                amplitude = 1f,
                frequency = 40f,
                cycleOffset = 0f
            };

        }
        #endregion effects

        #region projectiles
        private static void CreateProjectiles()
        {
            CreateBombProjectile();
            CreateVileShotgunIce();


            Content.AddProjectilePrefab(bombProjectilePrefab);
            Content.AddProjectilePrefab(vileShotgunIcePrefab);
        }

        private static void CreateBombProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            bombProjectilePrefab = Asset.CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(bombProjectilePrefab.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion bombImpactExplosion = bombProjectilePrefab.AddComponent<ProjectileImpactExplosion>();
            
            bombImpactExplosion.blastRadius = 16f;
            bombImpactExplosion.blastDamageCoefficient = 1f;
            bombImpactExplosion.falloffModel = BlastAttack.FalloffModel.None;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = bombExplosionEffect;
            bombImpactExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            ProjectileController bombController = bombProjectilePrefab.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("HenryBombGhost") != null)
                bombController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("HenryBombGhost");
            
            bombController.startSound = "";
        }


        private static void CreateVileShotgunIce()
        {

            // clone FMJ's syringe projectile prefab here to use as our own projectile
            //shotgunIceprefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/projectiles/MageIceBombProjectile"), "Prefabs/Projectiles/ShotgIceProjectile", true, "C:\\Users\\test\\Documents\\ror2mods\\MegamanX\\MegamanX\\MegamanX\\MegamanX.cs", "RegisterCharacter", 155);

            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            vileShotgunIcePrefab = Asset.CloneProjectilePrefab("MageIceBombProjectile", "VileShotgunIceProjectile");

            //UnityEngine.Object.Destroy(shotgunIceprefab.GetComponent<EffectComponent>());
            //UnityEngine.Object.Destroy(shotgunIceprefab.GetComponent<VFXAttributes>());

            // just setting the numbers to 1 as the entitystate will take care of those
            vileShotgunIcePrefab.GetComponent<ProjectileDamage>().damage = 1f;
            vileShotgunIcePrefab.GetComponent<ProjectileController>().procCoefficient = 1f;
            vileShotgunIcePrefab.GetComponent<ProjectileDamage>().damageType |= DamageType.Freeze2s;
            vileShotgunIcePrefab.GetComponent<ProjectileDamage>().damageType |= DamageTypeCombo.GenericSecondary;

            // register it for networking
            //if (shotgunIceprefab) PrefabAPI.RegisterNetworkPrefab(shotgunIceprefab);

            ProjectileController shotgunIceController = vileShotgunIcePrefab.GetComponent<ProjectileController>();
            shotgunIceController.ghostPrefab = vileShotgunIcePrefab.GetComponent<ProjectileController>().ghostPrefab;

            //if (_assetBundle.LoadAsset<GameObject>("ShotgunIceGhost") != null) shotgunIceController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("ShotgunIceGhost");
            //shotgunIceController.ghostPrefab = shotgunIceprefab;

            shotgunIceController.startSound = "";

        }


        #endregion projectiles
    }
}
