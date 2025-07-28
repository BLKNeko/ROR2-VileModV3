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

        //Material
        public static Material nightmareVMaterial;

        //projectiles
        public static GameObject bombProjectilePrefab;
        public static GameObject vileShotgunIcePrefab;
        public static GameObject vileEletricSparkPrefab;

        //Tracers
        public static GameObject vileGreenTracerPrefab;
        public static GameObject vileCyanTracerPrefab;
        public static GameObject vileBlueTracerPrefab;

        //UnitsProjectiles
        public static GameObject unitPreonEPrefab;
        public static GameObject unitPreonEPrefabTest;

        public static GameObject unitMettaurcurePrefab;

        public static GameObject unitBigBitPrefab;

        public static GameObject unitNightmareVPrefab;

        public static GameObject unitMettaurCommanderPrefab;

        //Icons
        public static Sprite VileSkinIcon;
        public static Sprite VileMK2SkinIcon;


        public static Sprite CherryBlastSkillIcon;

        public static Sprite ShotgunIceSkillIcon;

        public static Sprite BurningDriveSkillIcon;

        public static Sprite CallGoliathSkillIcon;
        public static Sprite ResumeGoliathSkillIcon;
        public static Sprite ExitGoliathSkillIcon;

        public static Sprite CallHawkSkillIcon;
        public static Sprite ResumeHawkSkillIcon;
        public static Sprite ExitHawkSkillIcon;

        private static AssetBundle _assetBundle;

        public static void Init(AssetBundle assetBundle)
        {

            _assetBundle = assetBundle;

            swordHitSoundEvent = Content.CreateAndAddNetworkSoundEventDef("HenrySwordHit");

            CreateEffects();

            CreateProjectiles();

            CreateIcons();

        }

        private static void CreateIcons()
        {
            VileSkinIcon = _assetBundle.LoadAsset<Sprite>("VileSkinIcon");
            VileMK2SkinIcon = _assetBundle.LoadAsset<Sprite>("VileMK2SkinIcon");

            CherryBlastSkillIcon = _assetBundle.LoadAsset<Sprite>("CherryBlastSkillIcon");
            ShotgunIceSkillIcon = _assetBundle.LoadAsset<Sprite>("ShotgunIceSkillIcon");
            BurningDriveSkillIcon = _assetBundle.LoadAsset<Sprite>("BurningDriveSkillIcon");

            CallGoliathSkillIcon = _assetBundle.LoadAsset<Sprite>("CallGoliathSkillIcon");
            ResumeGoliathSkillIcon = _assetBundle.LoadAsset<Sprite>("ResumeGoliathSkillIcon");
            ExitGoliathSkillIcon = _assetBundle.LoadAsset<Sprite>("ExitGoliathSkillIcon");

            CallHawkSkillIcon = _assetBundle.LoadAsset<Sprite>("CallHawkSkillIcon");
            ResumeHawkSkillIcon = _assetBundle.LoadAsset<Sprite>("ResumeHawkSkillIcon");
            ExitHawkSkillIcon = _assetBundle.LoadAsset<Sprite>("ExitHawkSkillIcon");

        }


        #region effects
        private static void CreateEffects()
        {
            CreateBombExplosionEffect();
            CreateRideExplosionEffect();

            swordSwingEffect = _assetBundle.LoadEffect("HenrySwordSwingEffect", true);
            swordHitImpactEffect = _assetBundle.LoadEffect("ImpactHenrySlash");

            BarPanel = _assetBundle.LoadAsset<GameObject>("BarPanel");

            nightmareVMaterial = _assetBundle.LoadMaterial("NightmareVMaterial");

            vileGreenTracerPrefab = CreateColoredTracerPrefab("TracerBanditPistol", "VGreenTacer", new Color(0.2f, 1f, 0.2f, 1f), 180f, 5f);
            vileCyanTracerPrefab = CreateColoredTracerPrefab("TracerBanditPistol", "VCyanTacer", new Color(0.4f, 0.8f, 1f, 1f), 170f, 5f);
            vileBlueTracerPrefab = CreateColoredTracerPrefab("TracerBanditPistol", "VBlueTacer", new Color(0.1f, 0.2f, 0.8f, 1f), 150f, 5f);

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

        public static GameObject CreateColoredTracerPrefab(string originalTracerPath, string newname, Color newColor, float tspeed, float tlenght)
        {

            GameObject clone = Asset.CloneTracer(originalTracerPath, newname);
            Debug.Log($"[TracerUtils] Cloned tracer prefab: {clone.name} from {originalTracerPath}");

            // LineRenderers
            foreach (var lr in clone.GetComponentsInChildren<LineRenderer>())
            {
                // ⚠️ Salvar os pontos antes de alterar qualquer coisa
                Vector3[] positions = new Vector3[lr.positionCount];
                lr.GetPositions(positions);

                lr.startColor = newColor;
                lr.endColor = newColor;

                if (lr.material != null)
                {
                    lr.material = UnityEngine.Object.Instantiate(lr.material);
                    if (lr.material.HasProperty("_Color")) lr.material.SetColor("_Color", newColor);
                    if (lr.material.HasProperty("_TintColor")) lr.material.SetColor("_TintColor", newColor);
                }

                // 🔁 Restaurar os pontos para evitar deformação
                lr.positionCount = positions.Length;
                lr.SetPositions(positions);

            }

            // TrailRenderers
            foreach (var tr in clone.GetComponentsInChildren<TrailRenderer>())
            {

                tr.startColor = newColor;
                tr.endColor = newColor;

                if (tr.material != null)
                {
                    tr.material = UnityEngine.Object.Instantiate(tr.material);
                    if (tr.material.HasProperty("_Color")) tr.material.SetColor("_Color", newColor);
                    if (tr.material.HasProperty("_TintColor")) tr.material.SetColor("_TintColor", newColor);
                }
            }

            //// ParticleSystems
            foreach (var ps in clone.GetComponentsInChildren<ParticleSystem>())
            {
                var main = ps.main;
                main.startColor = newColor;

                // Tentar mudar a cor do material, se possível
                var psRenderer = ps.GetComponent<ParticleSystemRenderer>();
                if (psRenderer != null && psRenderer.material != null)
                {
                    psRenderer.material = UnityEngine.Object.Instantiate(psRenderer.material);
                    if (psRenderer.material.HasProperty("_Color")) psRenderer.material.SetColor("_Color", newColor);
                    if (psRenderer.material.HasProperty("_TintColor")) psRenderer.material.SetColor("_TintColor", newColor);
                }
            }

            //Tracer configs
            Tracer tracer = clone.GetComponent<Tracer>();
            if (tracer != null)
            {
                tracer.speed = tspeed;
                tracer.length = tlenght;
            }

            return clone;
        }

        #region projectiles
        private static void CreateProjectiles()
        {
            CreateBombProjectile();
            CreateVileShotgunIce();
            CreateVileEletricSpark();

            CreateUnitPreonEProjectile();
            CreateUnitMettaurcureProjectile();
            CreateUnitBigBitProjectile();
            CreateUnitNightmareVProjectile();
            CreateUnitMetComProjectile();

            Content.AddProjectilePrefab(bombProjectilePrefab);
            Content.AddProjectilePrefab(vileShotgunIcePrefab);
            Content.AddProjectilePrefab(vileEletricSparkPrefab);

            Content.AddProjectilePrefab(unitPreonEPrefab);
            Content.AddProjectilePrefab(unitMettaurcurePrefab);
            Content.AddProjectilePrefab(unitBigBitPrefab);
            Content.AddProjectilePrefab(unitNightmareVPrefab);
            Content.AddProjectilePrefab(unitMettaurCommanderPrefab);
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

            unitPreonEPrefab.GetComponent<ProjectileSimple>().lifetime = 60f;

            unitPreonEPrefab.AddComponent<PreonEController>();

            ProjectileController unitPreonEController = unitPreonEPrefab.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("PreonE") != null)
                unitPreonEController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("PreonE");

            unitPreonEController.startSound = "";
        }

        private static void CreateUnitMettaurcureProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            unitMettaurcurePrefab = Asset.CloneProjectilePrefab("FMJ", "MettaurcureProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(unitMettaurcurePrefab.GetComponent<ProjectileImpactExplosion>());
            //ProjectileImpactExplosion bombImpactExplosion = bombProjectilePrefab.AddComponent<ProjectileImpactExplosion>();

            unitMettaurcurePrefab.GetComponent<ProjectileSimple>().lifetime = 60f;

            unitMettaurcurePrefab.AddComponent<MetCurController>();

            ProjectileController unitMetCurController = unitMettaurcurePrefab.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("Mettaurcure") != null)
                unitMetCurController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("Mettaurcure");

            unitMetCurController.startSound = "";
        }

        private static void CreateUnitBigBitProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            unitBigBitPrefab = Asset.CloneProjectilePrefab("FMJ", "BigBitProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(unitBigBitPrefab.GetComponent<ProjectileImpactExplosion>());
            //ProjectileImpactExplosion bombImpactExplosion = bombProjectilePrefab.AddComponent<ProjectileImpactExplosion>();

            unitBigBitPrefab.GetComponent<ProjectileSimple>().lifetime = 60f;

            unitBigBitPrefab.AddComponent<BigBitController>();

            ProjectileController unitBigBitController = unitBigBitPrefab.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("BigBit") != null)
                unitBigBitController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("BigBit");

            unitBigBitController.startSound = "";
        }

        private static void CreateUnitNightmareVProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            unitNightmareVPrefab = Asset.CloneProjectilePrefab("FMJ", "NightmareVProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(unitNightmareVPrefab.GetComponent<ProjectileImpactExplosion>());
            //ProjectileImpactExplosion bombImpactExplosion = bombProjectilePrefab.AddComponent<ProjectileImpactExplosion>();

            unitNightmareVPrefab.GetComponent<ProjectileSimple>().lifetime = 60f;

            unitNightmareVPrefab.AddComponent<NightmareVController>();

            ProjectileController unitNightmareVController = unitNightmareVPrefab.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("NightmareVirus") != null)
                unitNightmareVController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("NightmareVirus");

            unitNightmareVController.startSound = "";
        }

        private static void CreateUnitMetComProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            unitMettaurCommanderPrefab = Asset.CloneProjectilePrefab("FMJ", "MettaurCommanderProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(unitMettaurCommanderPrefab.GetComponent<ProjectileImpactExplosion>());
            //ProjectileImpactExplosion bombImpactExplosion = bombProjectilePrefab.AddComponent<ProjectileImpactExplosion>();

            unitMettaurCommanderPrefab.GetComponent<ProjectileSimple>().lifetime = 60f;

            unitMettaurCommanderPrefab.AddComponent<MetComController>();

            ProjectileController unitMetComController = unitMettaurCommanderPrefab.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("MetCom") != null)
                unitMetComController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("MetCom");

            unitMetComController.startSound = "";
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
