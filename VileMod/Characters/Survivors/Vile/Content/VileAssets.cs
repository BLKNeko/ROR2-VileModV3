using RoR2;
using UnityEngine;
using VileMod.Modules;
using System;
using RoR2.Projectile;
using VileMod.Survivors.Vile.Components;
using R2API;
using VileMod.Characters.Survivors.Vile.Components.UnitsComponents;
using VileMod.Characters.Survivors.Vile.Components;

namespace VileMod.Survivors.Vile
{
    public static class VileAssets
    {
        // particle effects
        public static GameObject swordSwingEffect;
        public static GameObject swordHitImpactEffect;

        public static GameObject bombExplosionEffect;

        public static GameObject rideExplosionEffect;
        public static GameObject vileExplosionEffect;

        public static GameObject bdriveEffect;

        public static GameObject gFallEffect;
        public static GameObject hFallEffect;
        public static GameObject cFallEffect;

        public static GameObject SDREffect;

        // networked hit sounds
        public static NetworkSoundEventDef swordHitSoundEvent;

        //UI
        public static GameObject BarPanel;

        //Material
        public static Material nightmareVMaterial;

        //MESH
        public static Mesh VH_Mesh_M;

        //projectiles
        public static GameObject bombProjectilePrefab;
        public static GameObject vileShotgunIcePrefab;
        public static GameObject vileEletricSparkPrefab;
        public static GameObject BumpityBombProjectile;
        public static GameObject FrontRunnerFireBallProjectile;
        public static GameObject CerberusPhantonFMJProjectile;
        public static GameObject NapalmBombProjectile;
        public static GameObject NapalmBombletsProjectile;
        public static GameObject ShockSphereProjectile;
        public static GameObject CYPlasmaProjectile;
        public static GameObject GShotProjectile;
        public static GameObject GVMissileProjectile;
        public static GameObject FlameRoundProjectile;
        public static GameObject FlameRoundBombProjectile;

        //Tracers
        public static GameObject vileGreenTracerPrefab;
        public static GameObject vileCyanTracerPrefab;
        public static GameObject vileBlueTracerPrefab;

        public static GameObject vileSDRTracerPrefab;
        public static GameObject vileSDRHitPrefab;

        //UnitsProjectiles
        

        public static GameObject unitMettaurcurePrefab;
        public static GameObject unitBigBitPrefab;
        public static GameObject unitSpikyPrefab;

        public static GameObject unitPreonEPrefab;
        public static GameObject unitMettaurCommanderPrefab;
        public static GameObject unitGunvoltPrefab;

        public static GameObject unitNightmareVPrefab;
        public static GameObject unitTogericsPrefab;
        public static GameObject unitMameqPrefab;


        //Icons
        public static Sprite VileSkinIcon;
        public static Sprite VileMK2SkinIcon;

        public static Sprite VilePassiveIcon;


        public static Sprite CherryBlastSkillIcon;
        public static Sprite ZipZapperSkillIcon;
        public static Sprite Triple7SkillIcon;
        public static Sprite DistanceNeedlerSkillIcon;

        public static Sprite ShotgunIceSkillIcon;

        public static Sprite BurningDriveSkillIcon;

        public static Sprite CallGoliathSkillIcon;
        public static Sprite ResumeGoliathSkillIcon;
        public static Sprite ExitGoliathSkillIcon;

        public static Sprite CallHawkSkillIcon;
        public static Sprite ResumeHawkSkillIcon;
        public static Sprite ExitHawkSkillIcon;

        public static Sprite CallCyclopsSkillIcon;
        public static Sprite ResumeCyclopsSkillIcon;
        public static Sprite ExitCyclopsSkillIcon;

        public static Sprite UnitBigBitSkillIcon;
        public static Sprite UnitMetComSkillIcon;
        public static Sprite UnitMetCurSkillIcon;
        public static Sprite UnitNightmareVSkillIcon;
        public static Sprite UnitPreonESkillIcon;


        public static Sprite BumpityBoomSkillIcon;
        

        public static Sprite CerberusPhantonSkillIcon;
        public static Sprite CYShotSkillIcon;
        public static Sprite CYPunchSkillIcon;

        public static Sprite EletricSparkSkillIcon;
        public static Sprite FlameRoundSkillIcon;
        public static Sprite SDRSkillIcon;
        

        public static Sprite FrontRunnerSkillIcon;
        public static Sprite GDashPunchSkillIcon;
        public static Sprite GPunchSkillIcon;
        public static Sprite GShotSkillIcon;
        public static Sprite HBarrageSkillIcon;
        public static Sprite HGunSkillIcon;
        public static Sprite NapalmBombSkillIcon;

        public static Sprite OverHeatDebuffIcon;
        public static Sprite PrimaryHeatBuffIcon;
        public static Sprite PrimaryIceBuffIcon;
        public static Sprite PrimaryShockBuffIcon;
        public static Sprite PrimaryFireBuffIcon;

        public static Sprite RepairRideArmorSkillIcon;
        public static Sprite RideArmorEnabledBuffcon;
        

        public static Sprite RideDashSkillIcon;

        public static Sprite UnitGunVoltSkillIcon;

        public static Sprite UnitMMQSkillIcon;

        public static Sprite UnitSpikySkillIcon;
        public static Sprite UnitTogericsSkillIcon;


        //Emote
        public static GameObject VileEmotePrefab;
        public static Sprite VileEmoteIcon;

        private static AssetBundle _assetBundle;

        public static void Init(AssetBundle assetBundle)
        {

            _assetBundle = assetBundle;

            swordHitSoundEvent = Content.CreateAndAddNetworkSoundEventDef("HenrySwordHit");

            CreateEffects();

            CreateProjectiles();

            CreateIcons();

            VH_Mesh_M = _assetBundle.LoadAsset<Mesh>("VH_Mesh_M");

        }

        private static void CreateIcons()
        {
            VileSkinIcon = _assetBundle.LoadAsset<Sprite>("VileSkinIcon");
            VileMK2SkinIcon = _assetBundle.LoadAsset<Sprite>("VileMK2SkinIcon");

            VilePassiveIcon = _assetBundle.LoadAsset<Sprite>("VileFuryIcon");

            CherryBlastSkillIcon = _assetBundle.LoadAsset<Sprite>("CherryBlastSkillIcon");
            ZipZapperSkillIcon = _assetBundle.LoadAsset<Sprite>("ZipZapperSkillIcon");
            Triple7SkillIcon = _assetBundle.LoadAsset<Sprite>("Triple7SkillIcon");
            DistanceNeedlerSkillIcon = _assetBundle.LoadAsset<Sprite>("DistanceNeedlerSkillIcon");
            ShotgunIceSkillIcon = _assetBundle.LoadAsset<Sprite>("ShotgunIceSkillIcon");

            BurningDriveSkillIcon = _assetBundle.LoadAsset<Sprite>("BurningDrive2SkillIcon");

            CallGoliathSkillIcon = _assetBundle.LoadAsset<Sprite>("CallGoliathSkillIcon");
            ResumeGoliathSkillIcon = _assetBundle.LoadAsset<Sprite>("ResumeGoliathSkillIcon");
            ExitGoliathSkillIcon = _assetBundle.LoadAsset<Sprite>("ExitGoliathSkillIcon");

            CallHawkSkillIcon = _assetBundle.LoadAsset<Sprite>("CallHawkSkillIcon");
            ResumeHawkSkillIcon = _assetBundle.LoadAsset<Sprite>("ResumeHawkSkillIcon");
            ExitHawkSkillIcon = _assetBundle.LoadAsset<Sprite>("ExitHawkSkillIcon");

            CallCyclopsSkillIcon = _assetBundle.LoadAsset<Sprite>("CallCyclopsSkillIcon");
            ResumeCyclopsSkillIcon = _assetBundle.LoadAsset<Sprite>("ResumeCyclopsSkillIcon");
            ExitCyclopsSkillIcon = _assetBundle.LoadAsset<Sprite>("ExitCyclopsSkillIcon");

            UnitBigBitSkillIcon = _assetBundle.LoadAsset<Sprite>("UnitBigBitSkillIcon");
            UnitMetComSkillIcon = _assetBundle.LoadAsset<Sprite>("UnitMetComSkillIcon");
            UnitMetCurSkillIcon = _assetBundle.LoadAsset<Sprite>("UnitMetCurSkillIcon");
            UnitNightmareVSkillIcon = _assetBundle.LoadAsset<Sprite>("UnitNightmareVSkillIcon");
            UnitPreonESkillIcon = _assetBundle.LoadAsset<Sprite>("UnitPreonESkillIcon");

            UnitGunVoltSkillIcon = _assetBundle.LoadAsset<Sprite>("UnitGunVoltSkillIcon");
            UnitMMQSkillIcon = _assetBundle.LoadAsset<Sprite>("UnitMMQSkillIcon");
            UnitSpikySkillIcon = _assetBundle.LoadAsset<Sprite>("UnitSpikySkillIcon");
            UnitTogericsSkillIcon = _assetBundle.LoadAsset<Sprite>("UnitTogericsSkillIcon");

            BumpityBoomSkillIcon = _assetBundle.LoadAsset<Sprite>("BumpityBoomSkillIcon");
            
            CerberusPhantonSkillIcon = _assetBundle.LoadAsset<Sprite>("CerberusPhantonSkillIcon");
            CYPunchSkillIcon = _assetBundle.LoadAsset<Sprite>("CYPunchSkillIcon");
            CYShotSkillIcon = _assetBundle.LoadAsset<Sprite>("CYShotSkillIcon");

            EletricSparkSkillIcon = _assetBundle.LoadAsset<Sprite>("EletricSparkSkillIcon");
            FlameRoundSkillIcon = _assetBundle.LoadAsset<Sprite>("FlameRoundSkillIcon");
            SDRSkillIcon = _assetBundle.LoadAsset<Sprite>("SDRSkillIcon");
            
            FrontRunnerSkillIcon = _assetBundle.LoadAsset<Sprite>("FrontRunnerSkillIcon");
            GDashPunchSkillIcon = _assetBundle.LoadAsset<Sprite>("GDashPunchSkillIcon");
            GPunchSkillIcon = _assetBundle.LoadAsset<Sprite>("GPunchSkillIcon");
            GShotSkillIcon = _assetBundle.LoadAsset<Sprite>("GShotSkillIcon");
            HBarrageSkillIcon = _assetBundle.LoadAsset<Sprite>("HBarrageSkillIcon");
            HGunSkillIcon = _assetBundle.LoadAsset<Sprite>("HGunSkillIcon");

            NapalmBombSkillIcon = _assetBundle.LoadAsset<Sprite>("NapalmBombSkillIcon");

            OverHeatDebuffIcon = _assetBundle.LoadAsset<Sprite>("OverHeatDebuffIcon");
            PrimaryHeatBuffIcon = _assetBundle.LoadAsset<Sprite>("PrimaryHeatBuffIcon");
            PrimaryIceBuffIcon = _assetBundle.LoadAsset<Sprite>("PrimaryIceBuffIcon");
            PrimaryShockBuffIcon = _assetBundle.LoadAsset<Sprite>("PrimaryShockBuffIcon");
            PrimaryFireBuffIcon = _assetBundle.LoadAsset<Sprite>("PrimaryFireBuffIcon");

            RepairRideArmorSkillIcon = _assetBundle.LoadAsset<Sprite>("RepairRideArmorSkillIcon");
            RideArmorEnabledBuffcon = _assetBundle.LoadAsset<Sprite>("RideArmorEnabledBuffIcon");
            
            RideDashSkillIcon = _assetBundle.LoadAsset<Sprite>("RideDashSkillIcon");


            VileEmoteIcon = _assetBundle.LoadAsset<Sprite>("VileEmoteIcon");

            


        }


        #region effects
        private static void CreateEffects()
        {
            CreateBombExplosionEffect();
            CreateRideExplosionEffect();
            CreateVileExplosionEffect();

            swordSwingEffect = _assetBundle.LoadEffect("HenrySwordSwingEffect", true);
            swordHitImpactEffect = _assetBundle.LoadEffect("ImpactHenrySlash");

            VileEmotePrefab = _assetBundle.LoadAsset<GameObject>("Vile_Humanoid");


            gFallEffect = _assetBundle.LoadEffect("GFallVFX");
            hFallEffect = _assetBundle.LoadEffect("HFallVFX");
            cFallEffect = _assetBundle.LoadEffect("CFallVFX");

            bdriveEffect = _assetBundle.LoadEffect("BurningDriveVFX");

            SDREffect = _assetBundle.LoadEffect("VSDRFlamesVFX", true);

            BarPanel = _assetBundle.LoadAsset<GameObject>("BarPanel");

            nightmareVMaterial = _assetBundle.LoadMaterial("matNightmareVirusEffect");

            vileGreenTracerPrefab = CreateColoredTracerPrefab("TracerBanditPistol", "VGreenTacer", new Color(0.2f, 1f, 0.2f, 1f), 180f, 5f);
            vileCyanTracerPrefab = CreateColoredTracerPrefab("TracerBanditPistol", "VCyanTacer", new Color(0.4f, 0.8f, 1f, 1f), 170f, 5f);
            vileBlueTracerPrefab = CreateColoredTracerPrefab("TracerBanditPistol", "VBlueTacer", new Color(0.1f, 0.2f, 0.8f, 1f), 150f, 5f);

            vileSDRTracerPrefab = CreateColoredTracerPrefab("TracerBanditPistol", "VSDRTacer", new Color(0.4f, 0.8f, 1f, 1f), 100f, 0f);
            //vileSDRHitPrefab = CreateColoredTracerPrefab("FireMeatBallExplosion", "VSDRHit", new Color(0.4f, 0.8f, 1f, 1f), 170f, 5f);

            Content.CreateAndAddEffectDef(vileGreenTracerPrefab);
            Content.CreateAndAddEffectDef(vileCyanTracerPrefab);
            Content.CreateAndAddEffectDef(vileBlueTracerPrefab);
            Content.CreateAndAddEffectDef(vileSDRTracerPrefab);


        }

        private static void CreateRideExplosionEffect()
        {
            rideExplosionEffect = _assetBundle.LoadEffect("RideArmorExplosionVFX", VileStaticValues.Play_Vile_Ride_Armor_Explosion, false);

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

        private static void CreateVileExplosionEffect()
        {
            vileExplosionEffect = _assetBundle.LoadEffect("VileExplosionVFX", VileStaticValues.Play_Vile_Death_Explosion, false);

            if (!rideExplosionEffect)
                return;

            ShakeEmitter shakeEmitter = rideExplosionEffect.AddComponent<ShakeEmitter>();
            shakeEmitter.amplitudeTimeDecay = true;
            shakeEmitter.duration = 1f;
            shakeEmitter.radius = 200f;
            shakeEmitter.scaleShakeRadiusWithLocalScale = false;

            shakeEmitter.wave = new Wave
            {
                amplitude = 2f,
                frequency = 50f,
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
            

            CreateBumpityBoomProjectile();
            CreateNapalmBombletsProjectile();
            CreateNapalmBombProjectile();

            CreateFrontRunnerProjectile();
            CreateCerberusPhantonProjectile();

            
            GShotPhantonProjectile();

            CreateCYPlasmaProjectile();
            CreateShockSphereProjectile();

            CreateGVMissileProjectile();

            CreateFlameRoundProjectile();
            CreateFlameRoundBombProjectile();

            CreateUnitPreonEProjectile();
            CreateUnitMettaurcureProjectile();
            CreateUnitBigBitProjectile();
            CreateUnitNightmareVProjectile();
            CreateUnitMetComProjectile();
            CreateUnitMameqProjectile();
            CreateUnitSpikyProjectile();
            CreateTogericsProjectile();
            CreateUnitGunVoltProjectile();



            Content.AddProjectilePrefab(bombProjectilePrefab);
            Content.AddProjectilePrefab(vileShotgunIcePrefab);
            Content.AddProjectilePrefab(vileEletricSparkPrefab);
            

            Content.AddProjectilePrefab(BumpityBombProjectile);
            Content.AddProjectilePrefab(NapalmBombletsProjectile);
            Content.AddProjectilePrefab(NapalmBombProjectile);

            Content.AddProjectilePrefab(FrontRunnerFireBallProjectile);
            Content.AddProjectilePrefab(CerberusPhantonFMJProjectile);

            Content.AddProjectilePrefab(GShotProjectile);

            Content.AddProjectilePrefab(CYPlasmaProjectile);
            Content.AddProjectilePrefab(ShockSphereProjectile);

            Content.AddProjectilePrefab(GVMissileProjectile);

            Content.AddProjectilePrefab(FlameRoundProjectile);
            Content.AddProjectilePrefab(FlameRoundBombProjectile);

            Content.AddProjectilePrefab(unitPreonEPrefab);
            Content.AddProjectilePrefab(unitMettaurcurePrefab);
            Content.AddProjectilePrefab(unitBigBitPrefab);
            Content.AddProjectilePrefab(unitNightmareVPrefab);
            Content.AddProjectilePrefab(unitMettaurCommanderPrefab);
            Content.AddProjectilePrefab(unitMameqPrefab);
            Content.AddProjectilePrefab(unitSpikyPrefab);
            Content.AddProjectilePrefab(unitTogericsPrefab);
            Content.AddProjectilePrefab(unitGunvoltPrefab);

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

        

        #region Units

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

        private static void CreateUnitGunVoltProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            unitGunvoltPrefab = Asset.CloneProjectilePrefab("FMJ", "GunVoltProjectile");

            unitGunvoltPrefab.GetComponent<ProjectileSimple>().lifetime = 60f;

            unitGunvoltPrefab.AddComponent<GunVoltController>();

            ProjectileController unitGunvoltController = unitGunvoltPrefab.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("GunVolt") != null)
                unitGunvoltController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("GunVolt");

            unitGunvoltController.startSound = "";
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

        private static void CreateUnitMameqProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            unitMameqPrefab = Asset.CloneProjectilePrefab("FMJ", "MameQProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(unitMameqPrefab.GetComponent<ProjectileImpactExplosion>());
            //ProjectileImpactExplosion bombImpactExplosion = bombProjectilePrefab.AddComponent<ProjectileImpactExplosion>();

            unitMameqPrefab.GetComponent<ProjectileSimple>().lifetime = 60f;

            unitMameqPrefab.AddComponent<MameqController>();

            ProjectileController unitMameqController = unitMameqPrefab.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("MAMEQ") != null)
                unitMameqController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("MAMEQ");

            unitMameqController.startSound = "";
        }

        private static void CreateUnitSpikyProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            unitSpikyPrefab = Asset.CloneProjectilePrefab("FMJ", "SpikyProjecile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(unitSpikyPrefab.GetComponent<ProjectileImpactExplosion>());
            //UnityEngine.Object.Destroy(shieldBoomerangProjectilePrefab.GetComponent<ProjectileSimple>());
            UnityEngine.Object.Destroy(unitSpikyPrefab.GetComponent<ProjectileStickOnImpact>());

            var simple = unitSpikyPrefab.GetComponent<ProjectileSimple>();

            if (simple)
            {
                simple.lifetime = 41f;
            }


            unitSpikyPrefab.AddComponent<SpikyController>();



            ProjectileController spikyController = unitSpikyPrefab.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("Spiky") != null)
                spikyController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("Spiky");

            spikyController.startSound = "";
        }

        private static void CreateTogericsProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            unitTogericsPrefab = Asset.CloneProjectilePrefab("FMJ", "TogericsProjectile");

            unitTogericsPrefab.AddComponent<TogericsController>(); // Adicionar o script


            // just setting the numbers to 1 as the entitystate will take care of those
            unitTogericsPrefab.GetComponent<ProjectileDamage>().damage = 1f;
            unitTogericsPrefab.GetComponent<ProjectileController>().procCoefficient = 1f;
            unitTogericsPrefab.GetComponent<ProjectileDamage>().damageType = DamageType.BleedOnHit;
            unitTogericsPrefab.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Bleed;


            unitTogericsPrefab.GetComponent<ProjectileSimple>().lifetime = 30f;

            ProjectileController TogericsController = unitTogericsPrefab.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("Togerics") != null)
                TogericsController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("Togerics");

            TogericsController.startSound = "";
            TogericsController.shouldPlaySounds = false;
        }


        #endregion


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

        private static void CreateBumpityBoomProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            BumpityBombProjectile = Asset.CloneProjectilePrefab("CommandoGrenadeProjectile", "BumpityBoomProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            //UnityEngine.Object.Destroy(BumpityBombProjectile.GetComponent<ProjectileImpactExplosion>());
            //ProjectileImpactExplosion bumpityImpactExplosion = BumpityBombProjectile.AddComponent<ProjectileImpactExplosion>();

            //bumpityImpactExplosion.blastRadius = 16f;
            //bumpityImpactExplosion.blastDamageCoefficient = 1f;
            //bumpityImpactExplosion.falloffModel = BlastAttack.FalloffModel.None;
            //bumpityImpactExplosion.destroyOnEnemy = true;
            //bumpityImpactExplosion.lifetime = 12f;
            //bumpityImpactExplosion.impactEffect = bombExplosionEffect;
            //bumpityImpactExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            //bumpityImpactExplosion.timerAfterImpact = true;
            //bumpityImpactExplosion.lifetimeAfterImpact = 0.1f;

            // just setting the numbers to 1 as the entitystate will take care of those
            BumpityBombProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            BumpityBombProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            BumpityBombProjectile.GetComponent<ProjectileDamage>().damageType |= DamageType.Stun1s;
            BumpityBombProjectile.GetComponent<ProjectileDamage>().damageType |= DamageTypeCombo.GenericSecondary;

            ProjectileController bumpityController = BumpityBombProjectile.GetComponent<ProjectileController>();

            //if (_assetBundle.LoadAsset<GameObject>("HenryBombGhost") != null)
            //    bombController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("HenryBombGhost");

            //bombController.startSound = "";
        }

        private static void CreateNapalmBombletsProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            NapalmBombletsProjectile = Asset.CloneProjectilePrefab("CryoCanisterBombletsProjectile", "NapalmBombletsProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            //UnityEngine.Object.Destroy(NapalmBombProjectile.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion napalmbombletsImpactExplosion = NapalmBombletsProjectile.GetComponent<ProjectileImpactExplosion>();

            //napalmImpactExplosion.blastRadius = 16f;
            //napalmImpactExplosion.blastDamageCoefficient = 1f;
            //napalmImpactExplosion.falloffModel = BlastAttack.FalloffModel.None;
            napalmbombletsImpactExplosion.destroyOnEnemy = true;
            napalmbombletsImpactExplosion.destroyOnWorld = true;
            napalmbombletsImpactExplosion.lifetime = 2f;
            napalmbombletsImpactExplosion.impactEffect = BumpityBombProjectile.GetComponent<ProjectileImpactExplosion>().impactEffect;
            //napalmImpactExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            napalmbombletsImpactExplosion.timerAfterImpact = true;
            napalmbombletsImpactExplosion.lifetimeAfterImpact = 0.1f;

            // just setting the numbers to 1 as the entitystate will take care of those
            NapalmBombletsProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            NapalmBombletsProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            NapalmBombletsProjectile.GetComponent<ProjectileDamage>().damageType |= DamageType.Freeze2s;
            NapalmBombletsProjectile.GetComponent<ProjectileDamage>().damageType |= DamageTypeCombo.GenericSecondary;

            ProjectileController napalmbombletsController = NapalmBombletsProjectile.GetComponent<ProjectileController>();

            //if (_assetBundle.LoadAsset<GameObject>("HenryBombGhost") != null)
            //    bombController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("HenryBombGhost");

            //bombController.startSound = "";
        }

        private static void CreateNapalmBombProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            NapalmBombProjectile = Asset.CloneProjectilePrefab("CryoCanisterProjectile", "NapalmBombProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            //UnityEngine.Object.Destroy(NapalmBombProjectile.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion napalmImpactExplosion = NapalmBombProjectile.GetComponent<ProjectileImpactExplosion>();

            //napalmImpactExplosion.blastRadius = 16f;
            //napalmImpactExplosion.blastDamageCoefficient = 1f;
            //napalmImpactExplosion.falloffModel = BlastAttack.FalloffModel.None;
            napalmImpactExplosion.destroyOnEnemy = true;
            napalmImpactExplosion.lifetime = 10f;
            napalmImpactExplosion.impactEffect = BumpityBombProjectile.GetComponent<ProjectileImpactExplosion>().impactEffect;
            //napalmImpactExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            napalmImpactExplosion.timerAfterImpact = true;
            napalmImpactExplosion.lifetimeAfterImpact = 0.1f;

            napalmImpactExplosion.childrenInheritDamageType = true;
            napalmImpactExplosion.childrenProjectilePrefab = NapalmBombletsProjectile;

            // just setting the numbers to 1 as the entitystate will take care of those
            NapalmBombProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            NapalmBombProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            NapalmBombProjectile.GetComponent<ProjectileDamage>().damageType |= DamageType.Freeze2s;
            NapalmBombProjectile.GetComponent<ProjectileDamage>().damageType |= DamageTypeCombo.GenericSecondary;

            ProjectileController napalmController = NapalmBombProjectile.GetComponent<ProjectileController>();

            //if (_assetBundle.LoadAsset<GameObject>("HenryBombGhost") != null)
            //    bombController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("HenryBombGhost");

            //bombController.startSound = "";
        }

        private static void CreateFlameRoundProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            FlameRoundProjectile = Asset.CloneProjectilePrefab("FMJ", "FlameRoundProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            //UnityEngine.Object.Destroy(NapalmBombProjectile.GetComponent<ProjectileImpactExplosion>());

            // just setting the numbers to 1 as the entitystate will take care of those
            FlameRoundProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            FlameRoundProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            FlameRoundProjectile.GetComponent<ProjectileDamage>().damageType |= DamageType.IgniteOnHit;
            FlameRoundProjectile.GetComponent<ProjectileDamage>().damageType |= DamageTypeCombo.GenericUtility;

            ProjectileController FRController = FlameRoundProjectile.GetComponent<ProjectileController>();

            FlameRoundProjectile.AddComponent<FlameRoundController>();

            // Adiciona movimentação simples
            ProjectileSimple simple = FlameRoundProjectile.GetComponent<ProjectileSimple>();
            if (!simple)
                simple = FlameRoundProjectile.AddComponent<ProjectileSimple>();

            simple.desiredForwardSpeed = 8f; // velocidade em frente
            simple.lifetime = 5f;             // tempo até desaparecer

            // Isso é útil para que ele vá para frente na direção em que foi spawnado
            simple.updateAfterFiring = true;


            if (_assetBundle.LoadAsset<GameObject>("FlameRoundSVFX") != null)
                FRController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("FlameRoundSVFX");

            FRController.startSound = "";
        }

        private static void CreateFlameRoundBombProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            FlameRoundBombProjectile = Asset.CloneProjectilePrefab("MageFireBombProjectile", "FlameRoundBombProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            //UnityEngine.Object.Destroy(NapalmBombProjectile.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion FGBImpactExplosion = FlameRoundBombProjectile.GetComponent<ProjectileImpactExplosion>();

            //napalmImpactExplosion.blastRadius = 16f;
            //napalmImpactExplosion.blastDamageCoefficient = 1f;
            //napalmImpactExplosion.falloffModel = BlastAttack.FalloffModel.None;
            FGBImpactExplosion.destroyOnEnemy = true;
            FGBImpactExplosion.lifetime = 10f;
            //FGBImpactExplosion.impactEffect = BumpityBombProjectile.GetComponent<ProjectileImpactExplosion>().impactEffect;
            //napalmImpactExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            FGBImpactExplosion.timerAfterImpact = true;
            FGBImpactExplosion.lifetimeAfterImpact = 0.1f;

            FGBImpactExplosion.fireChildren = true;
            FGBImpactExplosion.childrenInheritDamageType = true;
            FGBImpactExplosion.childrenCount = 1;
            FGBImpactExplosion.childrenProjectilePrefab = FlameRoundProjectile;

            // just setting the numbers to 1 as the entitystate will take care of those
            FlameRoundBombProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            FlameRoundBombProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            FlameRoundBombProjectile.GetComponent<ProjectileDamage>().damageType |= DamageType.IgniteOnHit;
            FlameRoundBombProjectile.GetComponent<ProjectileDamage>().damageType |= DamageTypeCombo.GenericUtility;

            ProjectileController FGBController = FlameRoundBombProjectile.GetComponent<ProjectileController>();

            //if (_assetBundle.LoadAsset<GameObject>("HenryBombGhost") != null)
            //    bombController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("HenryBombGhost");

            //bombController.startSound = "";
        }

        private static void CreateFrontRunnerProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            FrontRunnerFireBallProjectile = Asset.CloneProjectilePrefab("MageFireBombProjectile", "FrontRunnerProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            //UnityEngine.Object.Destroy(BumpityBombProjectile.GetComponent<ProjectileImpactExplosion>());
            //ProjectileImpactExplosion bumpityImpactExplosion = BumpityBombProjectile.AddComponent<ProjectileImpactExplosion>();

            //bumpityImpactExplosion.blastRadius = 16f;
            //bumpityImpactExplosion.blastDamageCoefficient = 1f;
            //bumpityImpactExplosion.falloffModel = BlastAttack.FalloffModel.None;
            //bumpityImpactExplosion.destroyOnEnemy = true;
            //bumpityImpactExplosion.lifetime = 12f;
            //bumpityImpactExplosion.impactEffect = bombExplosionEffect;
            //bumpityImpactExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            //bumpityImpactExplosion.timerAfterImpact = true;
            //bumpityImpactExplosion.lifetimeAfterImpact = 0.1f;

            // just setting the numbers to 1 as the entitystate will take care of those
            FrontRunnerFireBallProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            FrontRunnerFireBallProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            FrontRunnerFireBallProjectile.GetComponent<ProjectileDamage>().damageType |= DamageType.IgniteOnHit;
            FrontRunnerFireBallProjectile.GetComponent<ProjectileDamage>().damageType |= DamageTypeCombo.GenericSecondary;

            ProjectileController frontRunnerController = FrontRunnerFireBallProjectile.GetComponent<ProjectileController>();

            //if (_assetBundle.LoadAsset<GameObject>("HenryBombGhost") != null)
            //    bombController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("HenryBombGhost");

            //bombController.startSound = "";
        }

        private static void CreateCerberusPhantonProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            CerberusPhantonFMJProjectile = Asset.CloneProjectilePrefab("FMJ", "CerberusPhantonProjectile");

            // just setting the numbers to 1 as the entitystate will take care of those
            CerberusPhantonFMJProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            CerberusPhantonFMJProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            CerberusPhantonFMJProjectile.GetComponent<ProjectileDamage>().damageType |= DamageType.Generic;
            CerberusPhantonFMJProjectile.GetComponent<ProjectileDamage>().damageType |= DamageTypeCombo.GenericSpecial;

            ProjectileController CerberusPhantonController = CerberusPhantonFMJProjectile.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("CerberusPhantonProjectille") != null)
                CerberusPhantonController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("CerberusPhantonProjectille");

            CerberusPhantonController.startSound = "";
        }

        private static void GShotPhantonProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            GShotProjectile = Asset.CloneProjectilePrefab("FMJ", "GShotProjectile");

            // just setting the numbers to 1 as the entitystate will take care of those
            GShotProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            GShotProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            GShotProjectile.GetComponent<ProjectileDamage>().damageType |= DamageType.Generic;
            GShotProjectile.GetComponent<ProjectileDamage>().damageType |= DamageTypeCombo.GenericSecondary;

        }

        private static void CreateCYPlasmaProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            CYPlasmaProjectile = Asset.CloneProjectilePrefab("FMJ", "CYPlasmaProjectile");



            // just setting the numbers to 1 as the entitystate will take care of those
            CYPlasmaProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            CYPlasmaProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            CYPlasmaProjectile.GetComponent<ProjectileDamage>().damageType |= DamageTypeCombo.GenericSecondary;
            CYPlasmaProjectile.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Luminous;

            CYPlasmaProjectile.GetComponent<ProjectileDamage>().damageType.AddModdedDamageType(VileCustomDamageType.PlasmaSphereDamage);

            ProjectileController cyPlasmaController = CYPlasmaProjectile.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("UltimateBusterChargeProjectille") != null)
                cyPlasmaController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("UltimateBusterChargeProjectille");

            //cyPlasmaController.startSound = "";
        }

        private static void CreateShockSphereProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            ShockSphereProjectile = Asset.CloneProjectilePrefab("FMJ", "ShockSphereProjectile");

            ShockSphereProjectile.AddComponent<XShockSphereComponent>(); // Adicionar o script


            // just setting the numbers to 1 as the entitystate will take care of those
            ShockSphereProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            ShockSphereProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            ShockSphereProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Shock5s;
            ShockSphereProjectile.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Luminous;

            ProjectileController ShockSphereController = ShockSphereProjectile.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("ShockSphere") != null)
                ShockSphereController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("ShockSphere");

            ShockSphereController.startSound = "";
            ShockSphereController.shouldPlaySounds = false;
        }

        private static void CreateGVMissileProjectile()
        {

            GVMissileProjectile = Asset.CloneProjectilePrefab("MissileProjectile", "GVMissileProjectile");


        }


        #endregion projectiles
    }
}
