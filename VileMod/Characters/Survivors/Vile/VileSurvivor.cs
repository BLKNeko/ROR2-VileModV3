using BepInEx.Configuration;
using VileMod.Modules;
using VileMod.Modules.Characters;
using VileMod.Survivors.Vile.Components;
using VileMod.Survivors.Vile.SkillStates;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using RoR2.UI;
using MegamanXMod.Survivors.X.SkillStates;
using EntityStates;
using R2API;
using RoR2.Projectile;
using VileMod.Modules.BaseStates;
using EmotesAPI;

namespace VileMod.Survivors.Vile
{
    public class VileSurvivor : SurvivorBase<VileSurvivor>
    {
        //used to load the assetbundle for this character. must be unique
        public override string assetBundleName => "vilebundle"; //if you do not change this, you are giving permission to deprecate the mod

        //the name of the prefab we will create. conventionally ending in "Body". must be unique
        public override string bodyName => "VileBody"; //if you do not change this, you get the point by now

        //name of the ai master for vengeance and goobo. must be unique
        public override string masterName => "VileMonsterMaster"; //if you do not

        //the names of the prefabs you set up in unity that we will use to build your character
        public override string modelPrefabName => "mdlVile";
        public override string displayPrefabName => "VileDisplay";

        public const string VILE_PREFIX = VilePlugin.DEVELOPER_PREFIX + "_VILE_";

        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => VILE_PREFIX;

        //Emote
        internal bool setupEmoteSkeleton = false;
        private float vileTakeDamageValue = 0f;
        private CharacterMaster vileMaster;


        //GOLIATH SKILL DEFS
        internal static SkillDef enterGoliathSkillDef;
        internal static SkillDef exitGoliathSkillDef;
        internal static SkillDef resumeGoliathSkillDef;

        internal static SkillDef goliathPunchComboSkillDef;
        internal static SkillDef goliathDashPunchSkillDef;
        internal static SkillDef goliathShootSkillDef;

        //HAWK SKILL DEFS
        internal static SkillDef enterHawkSkillDef;
        internal static SkillDef exitHawkSkillDef;
        internal static SkillDef resumeHawkSkillDef;

        internal static HuntressTrackerSkillDef hawkGunSkillDef;
        internal static HuntressTrackerSkillDef hawkGunBarrageSkillDef;
        internal static SkillDef hawkDashSkillDef;

        //CYCLOPS SKILL DEFS
        internal static SkillDef enterCyclopsSkillDef;
        internal static SkillDef exitCyclopsSkillDef;
        internal static SkillDef resumeCyclopsSkillDef;

        internal static SkillDef CyclopsPunchSkillDef;
        internal static SkillDef CyclopsShotSkillDef;
        internal static SkillDef CyclopsDashSkillDef;

        //RIDE ARMOR GENERAL
        internal static SkillDef rideRapairSkillDef;
        internal static SkillDef destroyRideArmorSkillDef;

        //UNITS
        internal static SkillDef unitMettaurcureSkillDef;

        internal static SkillDef unitMettaurCommanderSkillDef;

        internal static SkillDef unitBigBitSkillDef;

        internal static SkillDef unitPreonESkillDef;

        internal static SkillDef unitNightmareVSkillDef;

        internal static SkillDef unitMameQSkillDef;

        internal static SkillDef unitSpikySkillDef;

        internal static SkillDef unitTogericsSkillDef;

        internal static SkillDef unitGunVoltSkillDef;

        //PRIMARY SKILLS DEFS
        internal static SkillDef cherryBlastSkillDef;
        internal static SkillDef zipZapperSkillDef;
        internal static SkillDef triple7SkillDef;
        internal static SkillDef distanceNeedlerSkillDef;

        //SECONDARY SKILL DEFS
        internal static SkillDef vileBumbpityBoomSkillDef;
        internal static SkillDef vileNapalmBombSkillDef;
        internal static SkillDef vileFrontRunnerSkillDef;

        //UTILITY SKILL DEFS
        internal static SkillDef vileShotgunIceSkillDef;
        internal static SkillDef vileEletricSparkSkillDef;
        internal static SkillDef vileFlameRoundSkillDef;

        //SPECIAL SKILL DEFS
        internal static SkillDef vileBurningDriveSkillDef;
        internal static SkillDef vileCerberusPhantonSkillDef;
        internal static SkillDef vileSDRSkillDef;


        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = VILE_PREFIX + "NAME",
            subtitleNameToken = VILE_PREFIX + "SUBTITLE",

            characterPortrait = assetBundle.LoadAsset<Texture>("VileTexIcon"),
            bodyColor = new Color(0.35f, 0.05f, 0.4f),
            sortPosition = 100,

            crosshair = Asset.LoadCrosshair("SMG"),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            armor = 30f,
            armorGrowth = 1.8f,
            shield = 25f,
            shieldGrowth = 2f,
            damage = 18f,
            healthGrowth = 25f,
            healthRegen = 1.8f,
            jumpCount = 1,
            maxHealth = 150f,
            attackSpeed = 0.85f,
            jumpPowerGrowth = 0.2f,
            jumpPower = 25,
            moveSpeed = 5.5f
            

            
        };

        public override CustomRendererInfo[] customRendererInfos => new CustomRendererInfo[]
        {
                new CustomRendererInfo
                {
                    childName = "VBodyMesh",
                    //material = assetBundle.LoadMaterial("matHenry"),
                },
                new CustomRendererInfo
                {
                    childName = "VEH",
                },
                new CustomRendererInfo
                {
                    childName = "VH_Mesh_C",
                },
                new CustomRendererInfo
                {
                    childName = "VH_Mesh_M",
                },
                new CustomRendererInfo
                {
                    childName = "VH_Mesh_S",
                },
                new CustomRendererInfo
                {
                    childName = "VH_VLC_Mesh",
                },
                new CustomRendererInfo
                {
                    childName = "VH_VLMKC_Mesh",
                },
                new CustomRendererInfo
                {
                    childName = "HAWK",
                },
                new CustomRendererInfo
                {
                    childName = "HK_Mesh",
                },
                new CustomRendererInfo
                {
                    childName = "HK_VLC_Mesh",
                },
                new CustomRendererInfo
                {
                    childName = "HK_VLMKC_Mesh",
                },
                new CustomRendererInfo
                {
                    childName = "CY",
                },
                new CustomRendererInfo
                {
                    childName = "CY_Mesh",
                },
                new CustomRendererInfo
                {
                    childName = "CY_VLC_Mesh",
                },
                new CustomRendererInfo
                {
                    childName = "CY_VLMKC_Mesh",
                }
        };

        public override UnlockableDef characterUnlockableDef => VileUnlockables.characterUnlockableDef;
        
        public override ItemDisplaysBase itemDisplays => new VileItemDisplays();

        //set in base classes
        public override AssetBundle assetBundle { get; protected set; }

        public override GameObject bodyPrefab { get; protected set; }
        public override CharacterBody prefabCharacterBody { get; protected set; }
        public override GameObject characterModelObject { get; protected set; }
        public override CharacterModel prefabCharacterModel { get; protected set; }
        public override GameObject displayPrefab { get; protected set; }

        public override void Initialize()
        {
            //uncomment if you have multiple characters
            //ConfigEntry<bool> characterEnabled = Config.CharacterEnableConfig("Survivors", "Vile");

            //if (!characterEnabled.Value)
            //    return;

            base.Initialize();
        }

        public override void InitializeCharacter()
        {
            //need the character unlockable before you initialize the survivordef
            VileUnlockables.Init();

            //Custom DamageTypes
            VileCustomDamageType.RegisterDamageTypes();

            base.InitializeCharacter();

            VileConfig.Init();
            VileStates.Init();
            VileTokens.Init();

            VileAssets.Init(assetBundle);
            VileBuffs.Init(assetBundle);

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            AdditionalBodySetup();

            

            AddHooks();
        }

        private void AdditionalBodySetup()
        {
            AddHitboxes();
            bodyPrefab.AddComponent<VileComponent>();
            bodyPrefab.AddComponent<VileFury>();
            bodyPrefab.AddComponent<VileHeatUIController>();
            bodyPrefab.AddComponent<VileBoltComponent>();
            bodyPrefab.AddComponent<VileRideArmorComponent>();
            bodyPrefab.AddComponent<HuntressTracker>();
            HuntressTracker huntressTracker = bodyPrefab.GetComponent<HuntressTracker>();
            if (huntressTracker)
            {
                huntressTracker.maxTrackingDistance = 500f;
                huntressTracker.maxTrackingAngle = 4f;
            }
            //anything else here
            bodyPrefab.GetComponent<CharacterBody>().bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage; 
        }

        public void AddHitboxes()
        {
            //example of how to create a HitBoxGroup. see summary for more details
            //Prefabs.SetupHitBoxGroup(characterModelObject, "SwordGroup", "SwordHitbox");

            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            GameObject model = childLocator.gameObject;


            Transform hitboxTransform = childLocator.FindChild("GoliathHitbox");
            Prefabs.SetupHitBoxGroup(model, "GoliathHitbox", "GoliathHitbox");
            //hitboxTransform.localScale = new Vector3(5.2f, 5.2f, 5.2f);
            hitboxTransform.localScale = new Vector3(10f, 10f, 10f);

            Transform hitboxTransform2 = childLocator.FindChild("VileCenterHitbox");
            Prefabs.SetupHitBoxGroup(model, "VileCenterHitbox", "VileCenterHitbox");
            //hitboxTransform.localScale = new Vector3(5.2f, 5.2f, 5.2f);
            hitboxTransform2.localScale = new Vector3(6f, 6f, 6f);

        }

        public override void InitializeEntityStateMachines() 
        {
            //clear existing state machines from your cloned body (probably commando)
            //omit all this if you want to just keep theirs
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            //the main "Body" state machine has some special properties
            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(EntityStates.GenericCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            bodyPrefab.GetComponent<CharacterDeathBehavior>().deathState = new EntityStates.SerializableEntityStateType(typeof(VileDeathState));
            bodyPrefab.GetComponent<EntityStateMachine>().initialStateType = new EntityStates.SerializableEntityStateType(typeof(VileSpawnState));
            //if you set up a custom main characterstate, set it up here
            //don't forget to register custom entitystates in your HenryStates.cs

            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
        }

        #region skills
        public override void InitializeSkills()
        {
            //remove the genericskills from the commando body we cloned
            Skills.ClearGenericSkills(bodyPrefab);
            Skills.CreateFirstExtraSkillFamily(bodyPrefab);
            Skills.CreateSecondExtraSkillFamily(bodyPrefab);
            Skills.CreateThirdExtraSkillFamily(bodyPrefab);
            Skills.CreateFourthExtraSkillFamily(bodyPrefab);


            CreateSkillDefs();
            

            //add our own
            AddPassiveSkill();
            AddPrimarySkills();
            AddSecondarySkills();
            AddUtiitySkills();
            AddSpecialSkills();

            AddExtraFirstSkills();
            AddExtraSecondSkills();
            AddExtraThirdSkills();
            AddExtraFourthSkills();
        }

        private void CreateSkillDefs()
        {
            #region Goliath

            enterGoliathSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "EnterGoliath",
                skillNameToken = VILE_PREFIX + "RIDE_ENTER_GOLIATH_NAME",
                skillDescriptionToken = VILE_PREFIX + "RIDE_ENTER_GOLIATH_DESCRIPTION",
                skillIcon = VileAssets.CallGoliathSkillIcon,
                keywordTokens = new[] { VILE_PREFIX + "VILE_KEYWORD_VBOLT" },

                activationState = new EntityStates.SerializableEntityStateType(typeof(EnterGoliath)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 100f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = true,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            exitGoliathSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ExitGoliath",
                skillNameToken = VILE_PREFIX + "RIDE_EXIT_GOLIATH_NAME",
                skillDescriptionToken = VILE_PREFIX + "RIDE_EXIT_GOLIATH_DESCRIPTION",
                skillIcon = VileAssets.ExitGoliathSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(ExitGoliath)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 10f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = true,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            resumeGoliathSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ResumeGoliath",
                skillNameToken = VILE_PREFIX + "RIDE_RESUME_GOLIATH_NAME",
                skillDescriptionToken = VILE_PREFIX + "RIDE_RESUME_GOLIATH_DESCRIPTION",
                skillIcon = VileAssets.ResumeGoliathSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(ResumeGoliath)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 10f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = true,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            goliathPunchComboSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "GoliathPunchCombo",
                skillNameToken = VILE_PREFIX + "RIDE_GOLIATH_PUNCH_COMBO_NAME",
                skillDescriptionToken = VILE_PREFIX + "RIDE_GOLIATH_PUNCH_COMBO_DESCRIPTION",
                skillIcon = VileAssets.GPunchSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(GPunch0)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 0f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            goliathShootSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "GoliathShoot",
                skillNameToken = VILE_PREFIX + "RIDE_GOLIATH_SHOOT_NAME",
                skillDescriptionToken = VILE_PREFIX + "RIDE_GOLIATH_SHOOT_DESCRIPTION",
                skillIcon = VileAssets.GShotSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(GShot)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 10f,
                baseMaxStock = 2,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            goliathDashPunchSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "GoliathDashPunch",
                skillNameToken = VILE_PREFIX + "RIDE_GOLIATH_DASH_PUNCH_NAME",
                skillDescriptionToken = VILE_PREFIX + "RIDE_GOLIATH_DASH_PUNCH_DESCRIPTION",
                skillIcon = VileAssets.GDashPunchSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(GDashPunch)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 15f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            #endregion

            rideRapairSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "RepairRideArmor",
                skillNameToken = VILE_PREFIX + "RIDE_REPAIR_NAME",
                skillDescriptionToken = VILE_PREFIX + "RIDE_REPAIR_DESCRIPTION",
                skillIcon = VileAssets.RepairRideArmorSkillIcon,
                keywordTokens = new[] { VILE_PREFIX + "VILE_KEYWORD_VBOLT" },

                activationState = new EntityStates.SerializableEntityStateType(typeof(RepairRideArmor)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 30f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = true,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            destroyRideArmorSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "DestroyRideArmor",
                skillNameToken = VILE_PREFIX + "RIDE_DESTROY_NAME",
                skillDescriptionToken = VILE_PREFIX + "RIDE_DESTROY_DESCRIPTION",

                activationState = new EntityStates.SerializableEntityStateType(typeof(DestroyRideArmor)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 60f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = true,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            #region HAWK



            enterHawkSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "EnterHawk",
                skillNameToken = VILE_PREFIX + "RIDE_ENTER_HAWK_NAME",
                skillDescriptionToken = VILE_PREFIX + "RIDE_ENTER_HAWK_DESCRIPTION",


                skillIcon = VileAssets.CallHawkSkillIcon,
                keywordTokens = new[] { VILE_PREFIX + "VILE_KEYWORD_VBOLT" },

                activationState = new EntityStates.SerializableEntityStateType(typeof(EnterHawk)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 100f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = true,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            exitHawkSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ExitHawk",
                skillNameToken = VILE_PREFIX + "RIDE_EXIT_HAWK_NAME",
                skillDescriptionToken = VILE_PREFIX + "RIDE_EXIT_HAWK_DESCRIPTION",

                skillIcon = VileAssets.ExitHawkSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(ExitHawk)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 10f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = true,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            resumeHawkSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ResumeHawk",
                skillNameToken = VILE_PREFIX + "RIDE_RESUME_HAWK_NAME",
                skillDescriptionToken = VILE_PREFIX + "RIDE_RESUME_HAWK_DESCRIPTION",

                skillIcon = VileAssets.ResumeHawkSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(ResumeHawk)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 10f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = true,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            hawkGunSkillDef = Skills.CreateSkillDef<HuntressTrackerSkillDef>(new SkillDefInfo
            {
                skillName = "HawkGun",
                skillNameToken = VILE_PREFIX + "RIDE_HAWK_GUN_NAME",
                skillDescriptionToken = VILE_PREFIX + "RIDE_HAWK_GUN_DESCRIPTION",

                skillIcon = VileAssets.HGunSkillIcon,
                // keywordTokens = new[] { MEGAMAN_x_PREFIX + "X_KEYWORD_CHARGE" },

                activationState = new SerializableEntityStateType(typeof(HGun1)),
                activationStateMachineName = "Weapon",
                interruptPriority = InterruptPriority.Skill,

                baseRechargeInterval = 0f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
                
                
            });

            hawkGunBarrageSkillDef = Skills.CreateSkillDef<HuntressTrackerSkillDef>(new SkillDefInfo
            {
                skillName = "HawkGunBarrage",
                skillNameToken = VILE_PREFIX + "RIDE_HAWK_GUN_BARRAGE_NAME",
                skillDescriptionToken = VILE_PREFIX + "RIDE_HAWK_GUN_BARRAGE_DESCRIPTION",

                skillIcon = VileAssets.HBarrageSkillIcon,
                // keywordTokens = new[] { MEGAMAN_x_PREFIX + "X_KEYWORD_CHARGE" },

                activationState = new SerializableEntityStateType(typeof(HGunBarrage)),
                activationStateMachineName = "Weapon",
                interruptPriority = InterruptPriority.Skill,

                baseRechargeInterval = 10f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,


            });

            hawkDashSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HawkDash",
                skillNameToken = VILE_PREFIX + "RIDE_HAWK_DASH_NAME",
                skillDescriptionToken = VILE_PREFIX + "RIDE_HAWK_DASH_DESCRIPTION",

                skillIcon = VileAssets.RideDashSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(HawkDash)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 10f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });



            #endregion

            #region Cyclops

            enterCyclopsSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "EnterCyclops",
                skillNameToken = VILE_PREFIX + "RIDE_ENTER_CYCLOPS_NAME",
                skillDescriptionToken = VILE_PREFIX + "RIDE_ENTER_CYCLOPS_DESCRIPTION",

                skillIcon = VileAssets.CallCyclopsSkillIcon,
                keywordTokens = new[] { VILE_PREFIX + "VILE_KEYWORD_VBOLT" },

                activationState = new EntityStates.SerializableEntityStateType(typeof(EnterCyclops)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 100f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = true,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            exitCyclopsSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ExitCyclops",
                skillNameToken = VILE_PREFIX + "RIDE_EXIT_CYCLOPS_NAME",
                skillDescriptionToken = VILE_PREFIX + "RIDE_EXIT_CYCLOPS_DESCRIPTION",

                skillIcon = VileAssets.ExitCyclopsSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(ExitCyclops)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 10f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = true,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            resumeCyclopsSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ResumeCyclops",
                skillNameToken = VILE_PREFIX + "RIDE_RESUME_CYCLOPS_NAME",
                skillDescriptionToken = VILE_PREFIX + "RIDE_RESUME_CYCLOPS_DESCRIPTION",

                skillIcon = VileAssets.ResumeCyclopsSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(ResumeCyclops)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 10f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = true,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            CyclopsPunchSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "CyclopsPunch",
                skillNameToken = VILE_PREFIX + "RIDE_CYCLOPS_PUNCH_NAME",
                skillDescriptionToken = VILE_PREFIX + "RIDE_CYCLOPS_PUNCH_DESCRIPTION",

                skillIcon = VileAssets.CYPunchSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(CYPunchN)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 0f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            CyclopsShotSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "CyclopsShot",
                skillNameToken = VILE_PREFIX + "RIDE_CYCLOPS_SHOT_NAME",
                skillDescriptionToken = VILE_PREFIX + "RIDE_CYCLOPS_SHOT_DESCRIPTION",

                skillIcon = VileAssets.CYShotSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(CYShot)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 10f,
                baseMaxStock = 3,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            CyclopsDashSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "CyclopsDash",
                skillNameToken = VILE_PREFIX + "RIDE_CYCLOPS_DASH_NAME",
                skillDescriptionToken = VILE_PREFIX + "RIDE_CYCLOPS_DASH_DESCRIPTION",

                skillIcon = VileAssets.RideDashSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(CYDash)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 10f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            #endregion

            #region Units

            unitMettaurcureSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Mettaurcure",
                skillNameToken = VILE_PREFIX + "UNIT_METTAURCURE_NAME",
                skillDescriptionToken = VILE_PREFIX + "UNIT_METTAURCURE_DESCRIPTION",
                skillIcon = VileAssets.UnitMetCurSkillIcon,
                keywordTokens = new[] { VILE_PREFIX + "VILE_KEYWORD_VBOLT" },

                activationState = new SerializableEntityStateType(typeof(UnitMettaurcure)),
                activationStateMachineName = "Weapon",
                interruptPriority = InterruptPriority.Skill,

                baseRechargeInterval = 75f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = true,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,


            });

            unitMettaurCommanderSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "MettaurCommander",
                skillNameToken = VILE_PREFIX + "UNIT_METTAURCOMMANDER_NAME",
                skillDescriptionToken = VILE_PREFIX + "UNIT_METTAURCOMMANDER_DESCRIPTION",
                skillIcon = VileAssets.UnitMetComSkillIcon,
                keywordTokens = new[] { VILE_PREFIX + "VILE_KEYWORD_VBOLT" },

                activationState = new SerializableEntityStateType(typeof(UnitMettaurCommander)),
                activationStateMachineName = "Weapon",
                interruptPriority = InterruptPriority.Skill,

                baseRechargeInterval = 80f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = true,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,


            });

            unitGunVoltSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "GunVolt",
                skillNameToken = VILE_PREFIX + "UNIT_GUNVOLT_NAME",
                skillDescriptionToken = VILE_PREFIX + "UNIT_GUNVOLT_DESCRIPTION",
                skillIcon = VileAssets.UnitGunVoltSkillIcon,
                keywordTokens = new[] { VILE_PREFIX + "VILE_KEYWORD_VBOLT" },

                activationState = new SerializableEntityStateType(typeof(UnitGunVolt)),
                activationStateMachineName = "Weapon",
                interruptPriority = InterruptPriority.Skill,

                baseRechargeInterval = 80f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = true,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,


            });

            unitBigBitSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "BigBit",
                skillNameToken = VILE_PREFIX + "UNIT_BIGBIT_NAME",
                skillDescriptionToken = VILE_PREFIX + "UNIT_BIGBIT_DESCRIPTION",
                skillIcon = VileAssets.UnitBigBitSkillIcon,
                keywordTokens = new[] { VILE_PREFIX + "VILE_KEYWORD_VBOLT" },

                activationState = new SerializableEntityStateType(typeof(UnitBigBit)),
                activationStateMachineName = "Weapon",
                interruptPriority = InterruptPriority.Skill,

                baseRechargeInterval = 75f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = true,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,


            });

            unitPreonESkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "PreonE",
                skillNameToken = VILE_PREFIX + "UNIT_PREONE_NAME",
                skillDescriptionToken = VILE_PREFIX + "UNIT_PREONE_DESCRIPTION",
                skillIcon = VileAssets.UnitPreonESkillIcon,
                keywordTokens = new[] { VILE_PREFIX + "VILE_KEYWORD_VBOLT" },

                activationState = new SerializableEntityStateType(typeof(UnitPreonE)),
                activationStateMachineName = "Weapon",
                interruptPriority = InterruptPriority.Skill,

                baseRechargeInterval = 80f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = true,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,


            });

            unitNightmareVSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "NightmareVirus",
                skillNameToken = VILE_PREFIX + "UNIT_NIGHTMAREVIRUS_NAME",
                skillDescriptionToken = VILE_PREFIX + "UNIT_NIGHTMAREVIRUS_DESCRIPTION",
                skillIcon = VileAssets.UnitNightmareVSkillIcon,
                keywordTokens = new[] { VILE_PREFIX + "VILE_KEYWORD_VBOLT" },

                activationState = new SerializableEntityStateType(typeof(UnitNightmareV)),
                activationStateMachineName = "Weapon",
                interruptPriority = InterruptPriority.Skill,

                baseRechargeInterval = 90f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = true,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,


            });

            unitMameQSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "MAME-Q",
                skillNameToken = VILE_PREFIX + "UNIT_MAMEQ_NAME",
                skillDescriptionToken = VILE_PREFIX + "UNIT_MAMEQ_DESCRIPTION",
                skillIcon = VileAssets.UnitMMQSkillIcon,
                keywordTokens = new[] { VILE_PREFIX + "VILE_KEYWORD_VBOLT" },

                activationState = new SerializableEntityStateType(typeof(UnitMameQ)),
                activationStateMachineName = "Weapon",
                interruptPriority = InterruptPriority.Skill,

                baseRechargeInterval = 90f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = true,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,


            });

            unitSpikySkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Spiky",
                skillNameToken = VILE_PREFIX + "UNIT_SPIKY_NAME",
                skillDescriptionToken = VILE_PREFIX + "UNIT_SPIKY_DESCRIPTION",
                skillIcon = VileAssets.UnitSpikySkillIcon,
                keywordTokens = new[] { VILE_PREFIX + "VILE_KEYWORD_VBOLT" },

                activationState = new SerializableEntityStateType(typeof(UnitSpiky)),
                activationStateMachineName = "Weapon",
                interruptPriority = InterruptPriority.Skill,

                baseRechargeInterval = 75f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = true,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,


            });

            unitTogericsSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Togerics",
                skillNameToken = VILE_PREFIX + "UNIT_TOGERICS_NAME",
                skillDescriptionToken = VILE_PREFIX + "UNIT_TOGERICS_DESCRIPTION",
                skillIcon = VileAssets.UnitTogericsSkillIcon,
                keywordTokens = new[] { VILE_PREFIX + "VILE_KEYWORD_VBOLT" },

                activationState = new SerializableEntityStateType(typeof(UnitTogerics)),
                activationStateMachineName = "Weapon",
                interruptPriority = InterruptPriority.Skill,

                baseRechargeInterval = 90f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = true,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,


            });

            #endregion

            #region Primary

            cherryBlastSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "CherryBlast",
                skillNameToken = VILE_PREFIX + "PRIMARY_CHERRY_BLAST_NAME",
                skillDescriptionToken = VILE_PREFIX + "PRIMARY_CHERRY_BLAST_DESCRIPTION",
                skillIcon = VileAssets.CherryBlastSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(CherryBlastStart)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 0f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            zipZapperSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ZipZapper",
                skillNameToken = VILE_PREFIX + "PRIMARY_ZIPZAPPER_NAME",
                skillDescriptionToken = VILE_PREFIX + "PRIMARY_ZIPZAPPER_DESCRIPTION",
                skillIcon = VileAssets.ZipZapperSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(ZipZapperStart)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 0f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            triple7SkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Triple7",
                skillNameToken = VILE_PREFIX + "PRIMARY_TRIPLE7_NAME",
                skillDescriptionToken = VILE_PREFIX + "PRIMARY_TRIPLE7_DESCRIPTION",
                skillIcon = VileAssets.Triple7SkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(Triple7Start)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 0f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            distanceNeedlerSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "DistanceNeedler",
                skillNameToken = VILE_PREFIX + "PRIMARY_DISTANCE_NEEDLER_NAME",
                skillDescriptionToken = VILE_PREFIX + "PRIMARY_DISTANCE_NEEDLER_DESCRIPTION",
                skillIcon = VileAssets.DistanceNeedlerSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(DistanceNeedlerStart)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 0f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            #endregion

            #region Secondary

            vileBumbpityBoomSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "BumpityBoom",
                skillNameToken = VILE_PREFIX + "SECONDARY_BUMBPITY_BOOM_NAME",
                skillDescriptionToken = VILE_PREFIX + "SECONDARY_BUMBPITY_BOOM_DESCRIPTION",
                skillIcon = VileAssets.BumpityBoomSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(VBumpityBoom)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 5f,
                baseMaxStock = 3,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            vileNapalmBombSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "NapalmBomb",
                skillNameToken = VILE_PREFIX + "SECONDARY_NAPALM_BOMB_NAME",
                skillDescriptionToken = VILE_PREFIX + "SECONDARY_NAPALM_BOMB_DESCRIPTION",
                skillIcon = VileAssets.NapalmBombSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(VNapalmBomb)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 5f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            vileFrontRunnerSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "FrontRunner",
                skillNameToken = VILE_PREFIX + "SECONDARY_FRONT_RUNNER_NAME",
                skillDescriptionToken = VILE_PREFIX + "SECONDARY_FRONT_RUNNER_DESCRIPTION",
                skillIcon = VileAssets.FrontRunnerSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(VFrontRunner)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 8f,
                baseMaxStock = 3,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            #endregion

            #region Utility

            vileShotgunIceSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "VShotgunIce",
                skillNameToken = VILE_PREFIX + "UTILITY_SHOTGUN_ICE_NAME",
                skillDescriptionToken = VILE_PREFIX + "UTILITY_SHOTGUN_ICE_DESCRIPTION",
                skillIcon = VileAssets.ShotgunIceSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(VShotgunIce)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 8f,
                baseMaxStock = 2,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            vileEletricSparkSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "VEletricSpark",
                skillNameToken = VILE_PREFIX + "UTILITY_ELECTRIC_SPARK_NAME",
                skillDescriptionToken = VILE_PREFIX + "UTILITY_ELECTRIC_SPARK_DESCRIPTION",
                skillIcon = VileAssets.EletricSparkSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(VEletricSpark)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 9f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            vileFlameRoundSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "VFlameRound",
                skillNameToken = VILE_PREFIX + "UTILITY_FLAME_ROUND_NAME",
                skillDescriptionToken = VILE_PREFIX + "UTILITY_FLAME_ROUND_DESCRIPTION",
                skillIcon = VileAssets.FlameRoundSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(VFlameRound)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 10f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });


            #endregion

            #region Special

            vileBurningDriveSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "VBurningDrive",
                skillNameToken = VILE_PREFIX + "SPECIAL_BURNING_DRIVE_NAME",
                skillDescriptionToken = VILE_PREFIX + "SPECIAL_BURNING_DRIVE_DESCRIPTION",
                skillIcon = VileAssets.BurningDriveSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(VBurningDrive)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 7f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            vileCerberusPhantonSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "CerberusPhanton",
                skillNameToken = VILE_PREFIX + "SPECIAL_CERBERUS_PHANTON_NAME",
                skillDescriptionToken = VILE_PREFIX + "SPECIAL_CERBERUS_PHANTON_DESCRIPTION",
                skillIcon = VileAssets.CerberusPhantonSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(VCerberusPhanton)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 7f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            vileSDRSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "SeaDragonRage",
                skillNameToken = VILE_PREFIX + "SPECIAL_SEA_DRAGON_RAGE_NAME",
                skillDescriptionToken = VILE_PREFIX + "SPECIAL_SEA_DRAGON_RAGE_DESCRIPTION",
                skillIcon = VileAssets.SDRSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(VSDR)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 12f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            #endregion

        }

        //skip if you don't have a passive
        //also skip if this is your first look at skills
        private void AddPassiveSkill()
        {
            //option 1. fake passive icon just to describe functionality we will implement elsewhere
            bodyPrefab.GetComponent<SkillLocator>().passiveSkill = new SkillLocator.PassiveSkill
            {
                enabled = true,
                skillNameToken = VILE_PREFIX + "VILE_PASSIVE_NAME",
                skillDescriptionToken = VILE_PREFIX + "VILE_PASSIVE_DESCRIPTION",
                //keywordToken = "KEYWORD_STUNNING",
                icon = VileAssets.VilePassiveIcon,
            };

            //option 2. a new SkillFamily for a passive, used if you want multiple selectable passives
            //GenericSkill passiveGenericSkill = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "PassiveSkill");
            //SkillDef passiveSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            //{
            //    skillName = "HenryPassive",
            //    skillNameToken = VILE_PREFIX + "PASSIVE_NAME",
            //    skillDescriptionToken = VILE_PREFIX + "PASSIVE_DESCRIPTION",
            //    keywordTokens = new string[] { "KEYWORD_AGILE" },
            //    skillIcon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),

            //    //unless you're somehow activating your passive like a skill, none of the following is needed.
            //    //but that's just me saying things. the tools are here at your disposal to do whatever you like with

            //    //activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shoot)),
            //    //activationStateMachineName = "Weapon1",
            //    //interruptPriority = EntityStates.InterruptPriority.Skill,

            //    //baseRechargeInterval = 1f,
            //    //baseMaxStock = 1,

            //    //rechargeStock = 1,
            //    //requiredStock = 1,
            //    //stockToConsume = 1,

            //    //resetCooldownTimerOnUse = false,
            //    //fullRestockOnAssign = true,
            //    //dontAllowPastMaxStocks = false,
            //    //mustKeyPress = false,
            //    //beginSkillCooldownOnSkillEnd = false,

            //    //isCombatSkill = true,
            //    //canceledFromSprinting = false,
            //    //cancelSprintingOnActivation = false,
            //    //forceSprintDuringState = false,

            //});
            //Skills.AddSkillsToFamily(passiveGenericSkill.skillFamily, passiveSkillDef1);
        }

        //if this is your first look at skilldef creation, take a look at Secondary first
        private void AddPrimarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Primary);

            //the primary skill is created using a constructor for a typical primary
            //it is also a SteppedSkillDef. Custom Skilldefs are very useful for custom behaviors related to casting a skill. see ror2's different skilldefs for reference
            //SteppedSkillDef primarySkillDef1 = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
            //    (
            //        "HenrySlash",
            //        VILE_PREFIX + "PRIMARY_SLASH_NAME",
            //        VILE_PREFIX + "PRIMARY_SLASH_DESCRIPTION",
            //        assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
            //        new EntityStates.SerializableEntityStateType(typeof(SkillStates.GPunch0)),
            //        "Weapon",
            //        true
            //    ));
            ////custom Skilldefs can have additional fields that you can set manually
            //primarySkillDef1.stepCount = 2;
            //primarySkillDef1.stepGraceDuration = 0.5f;

            //Skills.AddPrimarySkills(bodyPrefab, primarySkillDef1);
            //Skills.AddPrimarySkills(bodyPrefab, goliathPunchComboSkillDef);
            Skills.AddPrimarySkills(bodyPrefab, cherryBlastSkillDef);
            Skills.AddPrimarySkills(bodyPrefab, zipZapperSkillDef);
            Skills.AddPrimarySkills(bodyPrefab, triple7SkillDef);
            Skills.AddPrimarySkills(bodyPrefab, distanceNeedlerSkillDef);
        }

        private void AddSecondarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Secondary);

            //here is a basic skill def with all fields accounted for
            SkillDef secondarySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryGun",
                skillNameToken = VILE_PREFIX + "SECONDARY_GUN_NAME",
                skillDescriptionToken = VILE_PREFIX + "SECONDARY_GUN_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shoot)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 1f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,

            });

            //Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef1);
            Skills.AddSecondarySkills(bodyPrefab, vileBumbpityBoomSkillDef);
            Skills.AddSecondarySkills(bodyPrefab, vileNapalmBombSkillDef);
            Skills.AddSecondarySkills(bodyPrefab, vileFrontRunnerSkillDef);
        }

        private void AddUtiitySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Utility);

            //here's a skilldef of a typical movement skill.
            SkillDef utilitySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryRoll",
                skillNameToken = VILE_PREFIX + "UTILITY_ROLL_NAME",
                skillDescriptionToken = VILE_PREFIX + "UTILITY_ROLL_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUtilityIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Roll)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 4f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = true,
            });

            Skills.AddUtilitySkills(bodyPrefab, vileEletricSparkSkillDef);
            Skills.AddUtilitySkills(bodyPrefab, vileShotgunIceSkillDef);
            Skills.AddUtilitySkills(bodyPrefab, vileFlameRoundSkillDef);
        }

        private void AddSpecialSkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Special);

            //a basic skill. some fields are omitted and will just have default values
            SkillDef specialSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryBomb",
                skillNameToken = VILE_PREFIX + "SPECIAL_BOMB_NAME",
                skillDescriptionToken = VILE_PREFIX + "SPECIAL_BOMB_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ThrowBomb)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon2", interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 10f,

                isCombatSkill = true,
                mustKeyPress = false,
            });

            //Skills.AddSpecialSkills(bodyPrefab, specialSkillDef1);
            Skills.AddSpecialSkills(bodyPrefab, vileBurningDriveSkillDef);
            Skills.AddSpecialSkills(bodyPrefab, vileCerberusPhantonSkillDef);
            Skills.AddSpecialSkills(bodyPrefab, vileSDRSkillDef);
        }

        private void AddExtraFirstSkills()
        {

            Skills.AddFirstExtraSkill(bodyPrefab, unitMettaurcureSkillDef);
            Skills.AddFirstExtraSkill(bodyPrefab, unitBigBitSkillDef);
            Skills.AddFirstExtraSkill(bodyPrefab, unitSpikySkillDef);
        }

        private void AddExtraSecondSkills()
        {

            Skills.AddSecondExtraSkill(bodyPrefab, unitPreonESkillDef);
            Skills.AddSecondExtraSkill(bodyPrefab, unitMettaurCommanderSkillDef);
            Skills.AddSecondExtraSkill(bodyPrefab, unitGunVoltSkillDef);
            
        }

        private void AddExtraThirdSkills()
        {

            Skills.AddThirdExtraSkill(bodyPrefab, unitNightmareVSkillDef);
            Skills.AddThirdExtraSkill(bodyPrefab, unitTogericsSkillDef);
            Skills.AddThirdExtraSkill(bodyPrefab, unitMameQSkillDef);
        }

        private void AddExtraFourthSkills()
        {

            Skills.AddFourthExtraSkill(bodyPrefab, enterGoliathSkillDef);
            Skills.AddFourthExtraSkill(bodyPrefab, enterHawkSkillDef);
            Skills.AddFourthExtraSkill(bodyPrefab, enterCyclopsSkillDef);
        }

        #endregion skills

        #region skins
        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ChildLocator childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            SkinDef defaultSkin = Skins.CreateSkinDef("DEFAULT_SKIN",
                VileAssets.VileSkinIcon,
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
            //pass in meshes as they are named in your assetbundle
            //currently not needed as with only 1 skin they will simply take the default meshes
            //uncomment this when you have another skin
            defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
                "VileBodyMesh",
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);

            defaultSkin.rendererInfos[0].defaultMaterial = assetBundle.LoadMaterial("matVile");


            //here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            defaultSkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("VEH"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("HAWK"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("CY"),
                    shouldActivate = false,
                }
            };

            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion

            #region MK2Skin

            //creating a new skindef as we did before
            SkinDef mk2Skin = Modules.Skins.CreateSkinDef(VILE_PREFIX + "MK2_SKIN_NAME",
                VileAssets.VileMK2SkinIcon,
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //adding the mesh replacements as above. 
            //if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            mk2Skin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
                "VileMK2BodyMesh",
                null,//no gun mesh replacement. use same gun mesh
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);

            //masterySkin has a new set of RendererInfos (based on default rendererinfos)
            //you can simply access the RendererInfos' materials and set them to the new materials for your skin.
            mk2Skin.rendererInfos[0].defaultMaterial = assetBundle.LoadMaterial("matVileMK2");


            //here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            mk2Skin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("VEH"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("HAWK"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("CY"),
                    shouldActivate = false,
                }
            };
            //simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            skins.Add(mk2Skin);

            #endregion

            //uncomment this when you have a mastery skin
            #region MasterySkin

            ////creating a new skindef as we did before
            //SkinDef masterySkin = Modules.Skins.CreateSkinDef(VILE_PREFIX + "MASTERY_SKIN_NAME",
            //    assetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
            //    defaultRendererinfos,
            //    prefabCharacterModel.gameObject,
            //    HenryUnlockables.masterySkinUnlockableDef);

            ////adding the mesh replacements as above. 
            ////if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            //masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshHenrySwordAlt",
            //    null,//no gun mesh replacement. use same gun mesh
            //    "meshHenryAlt");

            ////masterySkin has a new set of RendererInfos (based on default rendererinfos)
            ////you can simply access the RendererInfos' materials and set them to the new materials for your skin.
            //masterySkin.rendererInfos[0].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            //masterySkin.rendererInfos[1].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            //masterySkin.rendererInfos[2].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");

            ////here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            //masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            //{
            //    new SkinDef.GameObjectActivation
            //    {
            //        gameObject = childLocator.FindChildGameObject("GunModel"),
            //        shouldActivate = false,
            //    }
            //};
            ////simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            //skins.Add(masterySkin);

            #endregion

            skinController.skins = skins.ToArray();
        }
        #endregion skins

        //Character Master is what governs the AI of your character when it is not controlled by a player (artifact of vengeance, goobo)
        public override void InitializeCharacterMaster()
        {
            //you must only do one of these. adding duplicate masters breaks the game.

            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            //Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Merc");

            //how to set up AI in code
            VileAI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }

        private void AddHooks()
        {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            GlobalEventManager.onServerDamageDealt += OnServerDamageDealt;
            On.RoR2.CharacterModel.Awake += CharacterModel_Awake;
            On.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
        }

        private void CharacterModel_Awake(On.RoR2.CharacterModel.orig_Awake orig, CharacterModel self)
        {
            orig(self);
            if (self)
            {

                if (self.gameObject.name == "VileDisplay(Clone)")
                {
                    AkSoundEngine.PostEvent(VileStaticValues.Play_Vile_Hahahaha, self.gameObject);

                }

            }


        }

        private void SurvivorCatalog_Init(On.RoR2.SurvivorCatalog.orig_Init orig)
        {
            orig();
            if (!setupEmoteSkeleton)
            {
                setupEmoteSkeleton = true;
                foreach (var item in SurvivorCatalog.allSurvivorDefs)
                {
                    if (item.bodyPrefab.name == bodyName)
                    {
                        var skele = VileAssets.VileEmotePrefab;
                        //Debug.Log("Before Emote: " + item.bodyPrefab.transform.position);
                        CustomEmotesAPI.ImportArmature(item.bodyPrefab, skele, true);
                        CustomEmotesAPI.CreateNameTokenSpritePair(VILE_PREFIX + "NAME", VileAssets.VileEmoteIcon);
                        //skele.GetComponentInChildren<BoneMapper>().scale = 1.05f;
                        //item.bodyPrefab.GetComponentInChildren<BoneMapper>().scale = 0.5f;
                        //skele.GetComponentInChildren<BoneMapper>().scale = 0.5f;
                        //Debug.Log("after Emote: " + item.bodyPrefab.transform.position);
                        //Debug.Log("skele pos: " + skele.transform.position);
                    }
                }
            }
        }

        private void OnServerDamageDealt(DamageReport report)
        {
            // Checa se esse dano tinha o seu DamageType
            if (DamageAPI.HasModdedDamageType(report.damageInfo, VileCustomDamageType.PlasmaSphereDamage))
            {
                // Aqui você aplica efeitos especiais, DOT, explosões, buffs, etc
                //Chat.AddMessage($"{report.victimBody.baseNameToken} levou meu dano customizado!");

                // Cria o ShockSphere
                ProjectileManager.instance.FireProjectile(new FireProjectileInfo
                {
                    projectilePrefab = VileAssets.ShockSphereProjectile,
                    position = report.victimBody.transform.position,
                    rotation = Quaternion.identity,
                    owner = report.attacker,
                    damage = 1f,
                    force = 200f,
                    crit = Util.CheckRoll(report.attackerBody.crit),
                    speedOverride = 0f,
                    damageColorIndex = DamageColorIndex.Luminous,
                });

            }

            if (DamageAPI.HasModdedDamageType(report.damageInfo, VileCustomDamageType.NightmareDamage))
            {
                // Aqui você aplica efeitos especiais, DOT, explosões, buffs, etc
                //Chat.AddMessage($"{report.victimBody.baseNameToken} levou meu dano customizado!");

                report.victimBody.AddBuff(VileBuffs.nightmareVirusDebuff);

                if (report.victimBody.transform)
                {
                    CharacterModel model = report.victimBody.GetComponent<ModelLocator>().modelTransform.gameObject.GetComponent<CharacterModel>();

                    model.baseRendererInfos[0].defaultMaterial = VileAssets.nightmareVMaterial;

                    //TemporaryOverlayInstance temporaryOverlayInstance = TemporaryOverlayManager.AddOverlay(sender.transform.gameObject);
                    //temporaryOverlayInstance.duration = 1f;
                    //temporaryOverlayInstance.animateShaderAlpha = true;
                    //temporaryOverlayInstance.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    //temporaryOverlayInstance.destroyComponentOnEnd = true;
                    //temporaryOverlayInstance.originalMaterial = VileAssets.nightmareVMaterial;
                    //temporaryOverlayInstance.inspectorCharacterModel = model;
                    //temporaryOverlayInstance.AddToCharacterModel(model);
                }

            }


        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {

            if (sender.HasBuff(VileBuffs.armorBuff))
            {
                args.armorAdd += 300;
            }

            if (sender.HasBuff(VileBuffs.VileFuryBuff))
            {
                args.attackSpeedMultAdd += 0.25f;
                args.jumpPowerMultAdd += 0.2f;
                args.moveSpeedMultAdd += 0.25f;
                args.damageMultAdd += 0.4f;
                args.regenMultAdd += 0.25f;

            }

            if (sender.HasBuff(VileBuffs.GoliathBuff))
            {
                args.armorAdd += 150;
                args.armorAdd += sender.baseArmor * 4f;
                args.healthMultAdd += 3f;
                args.damageMultAdd += 1f;
                args.attackSpeedMultAdd -= 0.1f;
                args.regenMultAdd += 1f;
                args.jumpPowerMultAdd -= 0.2f;
                args.moveSpeedMultAdd -= 0.15f;
                args.shieldMultAdd += 3f;
                args.critDamageMultAdd += 1f;
            }

            if (sender.HasBuff(VileBuffs.HawkBuff))
            {
                args.armorAdd += 80;
                args.armorAdd += sender.baseArmor * 2f;
                args.healthMultAdd += 1.5f;
                args.damageMultAdd += 0.4f;
                args.attackSpeedMultAdd += 0.25f;
                args.regenMultAdd += 1f;
                args.jumpPowerMultAdd += 0.2f;
                args.moveSpeedMultAdd += 0.2f;
                args.shieldMultAdd += 1f;
                args.critDamageMultAdd += 2f;
            }

            if (sender.HasBuff(VileBuffs.HawkDashBuff))
            {
                args.attackSpeedMultAdd += 0.25f;
                args.jumpPowerMultAdd += 0.2f;
                args.moveSpeedMultAdd += 0.25f;

            }

            if (sender.HasBuff(VileBuffs.CyclopsBuff))
            {
                args.armorAdd += 100;
                args.armorAdd += sender.baseArmor * 3f;
                args.healthMultAdd += 2f;
                args.damageMultAdd += 0.6f;
                args.regenMultAdd += 1f;
                args.jumpPowerMultAdd -= 0.1f;
                args.moveSpeedMultAdd -= 0.05f;
                args.shieldMultAdd += 2f;
                args.critDamageMultAdd += 1f;
            }

            if (sender.HasBuff(VileBuffs.MetComBuff))
            {
                args.shieldMultAdd += 3f;
            }

            if (sender.HasBuff(VileBuffs.MameqBuff))
            {
                args.armorAdd += 50;
                args.baseMoveSpeedAdd += 0.2f;
                args.critAdd += 0.25f;
                args.damageMultAdd += 2f;
            }

            if (sender.HasBuff(VileBuffs.nightmareVirusDebuff))
            {
                args.armorAdd -= 100;
                args.damageMultAdd -= 0.8f;
                args.attackSpeedMultAdd -= 0.4f;
                args.regenMultAdd -= 1f;
                args.jumpPowerMultAdd -= 0.3f;
                args.moveSpeedMultAdd -= 0.5f;
                

            }

        }
    }
}