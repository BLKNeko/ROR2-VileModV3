using RoR2;
using UnityEngine;
using VileMod.Modules;
using System;
using RoR2.Projectile;
using VileMod.Survivors.Vile.Components;
using R2API;

namespace VileMod.Survivors.Vile
{
    public static class VileAssets
    {
        // particle effects
        public static GameObject swordSwingEffect;
        public static GameObject swordHitImpactEffect;

        public static GameObject bombExplosionEffect;

        public static GameObject rideExplosionEffect;

        // networked hit sounds
        public static NetworkSoundEventDef swordHitSoundEvent;

        //UI
        public static GameObject BarPanel;

        //projectiles
        public static GameObject bombProjectilePrefab;
        public static GameObject vileShotgunIcePrefab;
        public static GameObject vileEletricSparkPrefab;

        public static GameObject unitPreonEPrefab;
        public static GameObject unitPreonEPrefabTest;

        private static AssetBundle _assetBundle;

        public static void Init(AssetBundle assetBundle)
        {

            _assetBundle = assetBundle;

            swordHitSoundEvent = Content.CreateAndAddNetworkSoundEventDef("HenrySwordHit");

            CreateEffects();

            CreateProjectiles();

            //CreateUnitTest();
        }

        private static void CreateUnitTest()
        {
            //unitPreonEPrefabTest = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterMasters/Turret1Master"),
            //                                        "PreonEMasterPrefab", true);
            //unitPreonEPrefabTest.GetComponent<CharacterMaster>().bodyPrefab = _assetBundle.LoadAsset<GameObject>("PreonE");

            //Content.AddMasterPrefab(unitPreonEPrefabTest);

            //unitPreonEPrefabTest  = _assetBundle.LoadAsset<GameObject>("PreonE");
            //unitPreonEPrefabTest.AddComponent<PreonEController>();

            // Clonar o corpo da torreta (ou Commando se quiser que pareça mais um personagem)
            //GameObject baseBody = LegacyResourcesAPI.Load<GameObject>("prefabs/characterbodies/EngiTurretBody");
            //unitPreonEPrefabTest = PrefabAPI.InstantiateClone(baseBody, "PreonE", true);
            ////AllyBodyPrefab = _assetBundle.LoadAsset<GameObject>("EXETurret");

            //// Pega o ModelLocator do prefab
            //ModelLocator modelLocator = unitPreonEPrefabTest.GetComponent<ModelLocator>();

            //// Destrói o modelo antigo
            //GameObject oldModel = modelLocator.modelTransform.gameObject;
            //UnityEngine.Object.DestroyImmediate(oldModel);

            //// Instancia seu novo modelo (deve ser um prefab seu carregado nos assets)
            //GameObject newModel = UnityEngine.Object.Instantiate(_assetBundle.LoadAsset<GameObject>("PreonE"), unitPreonEPrefabTest.transform);

            //// Atualiza o modelLocator
            //modelLocator.modelTransform = newModel.transform;
            //modelLocator.modelBaseTransform = newModel.transform; // ou algum filho específico, se quiser

            //// Acessa o SkillLocator da torreta
            //SkillLocator skillLocator = unitPreonEPrefabTest.GetComponent<SkillLocator>();

            //// Substitui a habilidade primária por uma que você já tem
            ////skillLocator.primary.skillFamily.variants[0].skillDef = MegamanEXESurvivor.BusterTurretSkillDef;


            //// Ajustar stats (opcional)
            //CharacterBody body = unitPreonEPrefabTest.GetComponent<CharacterBody>();
            //body.baseMaxHealth = 200f;
            //body.baseDamage = 15f;
            //body.baseMoveSpeed = 0f; // parado como uma torreta
            //body.baseAttackSpeed = 1.5f;
            //body.isChampion = false;

            //// Clonar o master da torreta
            //GameObject baseMaster = LegacyResourcesAPI.Load<GameObject>("prefabs/charactermasters/EngiTurretMaster");
            //unitPreonEPrefabTest = PrefabAPI.InstantiateClone(baseMaster, "MyAllyMaster", true);

            //// Definir que o master usa nosso body
            //CharacterMaster master = unitPreonEPrefabTest.GetComponent<CharacterMaster>();
            //master.bodyPrefab = unitPreonEPrefabTest;

            //Content.AddMasterPrefab(unitPreonEPrefabTest);
            // Registrar no catálogo
            //BodyCatalog.getAdditionalEntries += list => list.Add(unitPreonEPrefabTest);
            //MasterCatalog.getAdditionalEntries += list => list.Add(unitPreonEPrefabTest);


        }

        #region effects
        private static void CreateEffects()
        {
            CreateBombExplosionEffect();
            CreateRideExplosionEffect();

            swordSwingEffect = _assetBundle.LoadEffect("HenrySwordSwingEffect", true);
            swordHitImpactEffect = _assetBundle.LoadEffect("ImpactHenrySlash");

            BarPanel = _assetBundle.LoadAsset<GameObject>("BarPanel");

        }

        private static void CreateRideExplosionEffect()
        {
            rideExplosionEffect = _assetBundle.LoadEffect("RideArmorExplosionVFX", "HenryBombExplosion", false);

            if (!rideExplosionEffect)
                return;

            ShakeEmitter shakeEmitter = rideExplosionEffect.AddComponent<ShakeEmitter>();
            shakeEmitter.amplitudeTimeDecay = true;
            shakeEmitter.duration = 1.5f;
            shakeEmitter.radius = 400f;
            shakeEmitter.scaleShakeRadiusWithLocalScale = false;

            shakeEmitter.wave = new Wave
            {
                amplitude = 2f,
                frequency = 80f,
                cycleOffset = 0f
            };

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
            CreateVileEletricSpark();

            CreateUnitPreonEProjectile();

            Content.AddProjectilePrefab(bombProjectilePrefab);
            Content.AddProjectilePrefab(vileShotgunIcePrefab);
            Content.AddProjectilePrefab(vileEletricSparkPrefab);

            Content.AddProjectilePrefab(unitPreonEPrefab);
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

        private static void CreateUnitPreonEProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            unitPreonEPrefab = Asset.CloneProjectilePrefab("FMJ", "PreonEProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(unitPreonEPrefab.GetComponent<ProjectileImpactExplosion>());
            //ProjectileImpactExplosion bombImpactExplosion = bombProjectilePrefab.AddComponent<ProjectileImpactExplosion>();

            //bombImpactExplosion.blastRadius = 16f;
            //bombImpactExplosion.blastDamageCoefficient = 1f;
            //bombImpactExplosion.falloffModel = BlastAttack.FalloffModel.None;
            //bombImpactExplosion.destroyOnEnemy = true;
            //bombImpactExplosion.lifetime = 12f;
            //bombImpactExplosion.impactEffect = bombExplosionEffect;
            //bombImpactExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            //bombImpactExplosion.timerAfterImpact = true;
            //bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            unitPreonEPrefab.GetComponent<ProjectileSimple>().lifetime = 10f;

            unitPreonEPrefab.AddComponent<PreonEController>();

            ProjectileController unitPreonEController = unitPreonEPrefab.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("PreonE") != null)
                unitPreonEController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("PreonE");

            unitPreonEController.startSound = "";
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
            vileShotgunIcePrefab.GetComponent<ProjectileDamage>().damageType |= DamageTypeCombo.GenericUtility;

            // register it for networking
            //if (shotgunIceprefab) PrefabAPI.RegisterNetworkPrefab(shotgunIceprefab);

            ProjectileController shotgunIceController = vileShotgunIcePrefab.GetComponent<ProjectileController>();
            shotgunIceController.ghostPrefab = vileShotgunIcePrefab.GetComponent<ProjectileController>().ghostPrefab;

            //if (_assetBundle.LoadAsset<GameObject>("ShotgunIceGhost") != null) shotgunIceController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("ShotgunIceGhost");
            //shotgunIceController.ghostPrefab = shotgunIceprefab;

            shotgunIceController.startSound = "";

        }

        private static void CreateVileEletricSpark()
        {

            // clone FMJ's syringe projectile prefab here to use as our own projectile
            //shotgunIceprefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/projectiles/MageIceBombProjectile"), "Prefabs/Projectiles/ShotgIceProjectile", true, "C:\\Users\\test\\Documents\\ror2mods\\MegamanX\\MegamanX\\MegamanX\\MegamanX.cs", "RegisterCharacter", 155);

            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            vileEletricSparkPrefab = Asset.CloneProjectilePrefab("MageLightningBombProjectile", "VileEletricSparkProjectile");

            //UnityEngine.Object.Destroy(shotgunIceprefab.GetComponent<EffectComponent>());
            //UnityEngine.Object.Destroy(shotgunIceprefab.GetComponent<VFXAttributes>());

            // just setting the numbers to 1 as the entitystate will take care of those
            vileEletricSparkPrefab.GetComponent<ProjectileDamage>().damage = 1f;
            vileEletricSparkPrefab.GetComponent<ProjectileController>().procCoefficient = 1f;
            vileEletricSparkPrefab.GetComponent<ProjectileDamage>().damageType |= DamageType.Shock5s;
            vileEletricSparkPrefab.GetComponent<ProjectileDamage>().damageType |= DamageTypeCombo.GenericUtility;

            // register it for networking
            //if (shotgunIceprefab) PrefabAPI.RegisterNetworkPrefab(shotgunIceprefab);

            ProjectileController eletricSparkController = vileEletricSparkPrefab.GetComponent<ProjectileController>();
            eletricSparkController.ghostPrefab = vileEletricSparkPrefab.GetComponent<ProjectileController>().ghostPrefab;

            //if (_assetBundle.LoadAsset<GameObject>("ShotgunIceGhost") != null) shotgunIceController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("ShotgunIceGhost");
            //shotgunIceController.ghostPrefab = shotgunIceprefab;

            eletricSparkController.startSound = "";

        }


        #endregion projectiles
    }
}
