using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace VileMod.Modules
{
    internal static class Projectiles
    {
        internal static GameObject bombPrefab;

        internal static GameObject EletricSpark;
        internal static GameObject BumpityBombProjectile;
        internal static GameObject FrontRunnerFireBallProjectile;
        internal static GameObject CerberusPhantonFMJProjectile;
        internal static GameObject ShotgunIceProjectile;

        internal static void RegisterProjectiles()
        {
            // only separating into separate methods for my sanity
            //CreateBomb();
            CreateEletricSparkProjectile();
            CreateBumpityBombProjectile();
            CreateFrontRunnerProjectile();
            CreateCerberusPhantonProjectile();
            CreateShotgunIceProjectile();

            //AddProjectile(bombPrefab);
            AddProjectile(EletricSpark);
            AddProjectile(BumpityBombProjectile);
            AddProjectile(FrontRunnerFireBallProjectile);
            AddProjectile(CerberusPhantonFMJProjectile);
            AddProjectile(ShotgunIceProjectile);
        }

        internal static void AddProjectile(GameObject projectileToAdd)
        {
            Modules.Content.AddProjectilePrefab(projectileToAdd);
        }

        private static void CreateBomb()
        {
            bombPrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            ProjectileImpactExplosion bombImpactExplosion = bombPrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            bombImpactExplosion.blastRadius = 16f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            //bombImpactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("HenryBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            ProjectileController bombController = bombPrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("HenryBombGhost") != null) bombController.ghostPrefab = CreateGhostPrefab("HenryBombGhost");
            bombController.startSound = "";
        }

        private static void CreateEletricSparkProjectile()
        {

            // clone rex's syringe projectile prefab here to use as our own projectile
            EletricSpark = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/projectiles/MageLightningBombProjectile"), "Prefabs/Projectiles/ESparkProjectile", true, "C:\\Users\\test\\Documents\\ror2mods\\MegamanXVile\\MegamanXVile\\MegamanXVile\\MegamanXVile.cs", "RegisterCharacter", 155);

            // just setting the numbers to 1 as the entitystate will take care of those
            EletricSpark.GetComponent<ProjectileController>().procCoefficient = 1f;
            EletricSpark.GetComponent<ProjectileDamage>().damage = 1f;
            EletricSpark.GetComponent<ProjectileDamage>().damageType = DamageType.Shock5s;

            // register it for networking
            if (EletricSpark) PrefabAPI.RegisterNetworkPrefab(EletricSpark);

            ProjectileController ESController = EletricSpark.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("ESGhost") != null) ESController.ghostPrefab = CreateGhostPrefab("ESGhost");
            ESController.startSound = "";
        }

        private static void CreateBumpityBombProjectile()
        {

            BumpityBombProjectile = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/projectiles/CommandoGrenadeProjectile"), "Prefabs/Projectiles/BombProjectile", true, "C:\\Users\\test\\Documents\\ror2mods\\MegamanXVile\\MegamanXVile\\MegamanXVile\\MegamanXVile.cs", "RegisterCharacter", 155);

            // just setting the numbers to 1 as the entitystate will take care of those
            BumpityBombProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            BumpityBombProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            BumpityBombProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;

            // register it for networking
            if (BumpityBombProjectile) PrefabAPI.RegisterNetworkPrefab(BumpityBombProjectile);

            ProjectileController BBController = BumpityBombProjectile.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("BBGhost") != null) BBController.ghostPrefab = CreateGhostPrefab("BBGhost");
            BBController.startSound = "";
        }

        private static void CreateFrontRunnerProjectile()
        {

            // clone FMJ's syringe projectile prefab here to use as our own projectile
            FrontRunnerFireBallProjectile = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/projectiles/MageFireBombProjectile"), "Prefabs/Projectiles/BombProjectile", true, "C:\\Users\\test\\Documents\\ror2mods\\MegamanXVile\\MegamanXVile\\MegamanXVile\\MegamanXVile.cs", "RegisterCharacter", 155);

            // just setting the numbers to 1 as the entitystate will take care of those
            FrontRunnerFireBallProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            FrontRunnerFireBallProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            FrontRunnerFireBallProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;

            // register it for networking
            if (FrontRunnerFireBallProjectile) PrefabAPI.RegisterNetworkPrefab(FrontRunnerFireBallProjectile);

            ProjectileController FRController = FrontRunnerFireBallProjectile.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("FRGhost") != null) FRController.ghostPrefab = CreateGhostPrefab("FRGhost");
            FRController.startSound = "";
        }

        private static void CreateCerberusPhantonProjectile()
        {

            // clone FMJ's syringe projectile prefab here to use as our own projectile
            CerberusPhantonFMJProjectile = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/projectiles/FMJ"), "Prefabs/Projectiles/BombProjectile", true, "C:\\Users\\test\\Documents\\ror2mods\\MegamanXVile\\MegamanXVile\\MegamanXVile\\MegamanXVile.cs", "RegisterCharacter", 155);

            // just setting the numbers to 1 as the entitystate will take care of those
            CerberusPhantonFMJProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            CerberusPhantonFMJProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            CerberusPhantonFMJProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;

            // register it for networking
            if (CerberusPhantonFMJProjectile) PrefabAPI.RegisterNetworkPrefab(CerberusPhantonFMJProjectile);

            ProjectileController CPController = CerberusPhantonFMJProjectile.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("CPGhost") != null) CPController.ghostPrefab = CreateGhostPrefab("CPGhost");
            CPController.startSound = "";
        }

        private static void CreateShotgunIceProjectile()
        {

            // clone FMJ's syringe projectile prefab here to use as our own projectile
            ShotgunIceProjectile = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/projectiles/MageIceBombProjectile"), "Prefabs/Projectiles/BombProjectile", true, "C:\\Users\\test\\Documents\\ror2mods\\MegamanXVile\\MegamanXVile\\MegamanXVile\\MegamanXVile.cs", "RegisterCharacter", 155);

            // just setting the numbers to 1 as the entitystate will take care of those
            ShotgunIceProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            ShotgunIceProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            ShotgunIceProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Freeze2s;

            // register it for networking
            if (ShotgunIceProjectile) PrefabAPI.RegisterNetworkPrefab(ShotgunIceProjectile);

            ProjectileController SIController = ShotgunIceProjectile.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("SIGhost") != null) SIController.ghostPrefab = CreateGhostPrefab("SIGhost");
            SIController.startSound = "";
        }

        private static void InitializeImpactExplosion(ProjectileImpactExplosion projectileImpactExplosion)
        {
            projectileImpactExplosion.blastDamageCoefficient = 1f;
            projectileImpactExplosion.blastProcCoefficient = 1f;
            projectileImpactExplosion.blastRadius = 1f;
            projectileImpactExplosion.bonusBlastForce = Vector3.zero;
            projectileImpactExplosion.childrenCount = 0;
            projectileImpactExplosion.childrenDamageCoefficient = 0f;
            projectileImpactExplosion.childrenProjectilePrefab = null;
            projectileImpactExplosion.destroyOnEnemy = false;
            projectileImpactExplosion.destroyOnWorld = false;
            projectileImpactExplosion.falloffModel = RoR2.BlastAttack.FalloffModel.None;
            projectileImpactExplosion.fireChildren = false;
            projectileImpactExplosion.impactEffect = null;
            projectileImpactExplosion.lifetime = 0f;
            projectileImpactExplosion.lifetimeAfterImpact = 0f;
            projectileImpactExplosion.lifetimeRandomOffset = 0f;
            projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
            projectileImpactExplosion.timerAfterImpact = false;

            projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
        }

        private static GameObject CreateGhostPrefab(string ghostName)
        {
            GameObject ghostPrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
            if (!ghostPrefab.GetComponent<NetworkIdentity>()) ghostPrefab.AddComponent<NetworkIdentity>();
            if (!ghostPrefab.GetComponent<ProjectileGhostController>()) ghostPrefab.AddComponent<ProjectileGhostController>();

            Modules.Assets.ConvertAllRenderersToHopooShader(ghostPrefab);

            return ghostPrefab;
        }

        private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }
    }
}