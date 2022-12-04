using BepInEx.Configuration;
using VileMod.Modules.Characters;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace VileMod.Modules.Survivors
{
    internal class MyCharacter : SurvivorBase
    {
        //used when building your character using the prefabs you set up in unity
        //don't upload to thunderstore without changing this
        public override string prefabBodyName => "VileV3";

        public const string VILEV3_PREFIX = VilePlugin.DEVELOPER_PREFIX + "_VILEV3_BODY_";

        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => VILEV3_PREFIX;

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            bodyName = "VileV3Body",
            bodyNameToken = VILEV3_PREFIX + "NAME",
            subtitleNameToken = VILEV3_PREFIX + "SUBTITLE",

            characterPortrait = Modules.Assets.VIcon,
            bodyColor = new Color(0.35f, 0.05f, 0.4f),

            crosshair = Resources.Load<GameObject>("Prefabs/Crosshair/SMGCrosshair"),
            podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            //maxHealth = 110f,
            //healthRegen = 1.5f,
            //armor = 0f,
            //jumpCount = 1,

            armor = 30f,
            armorGrowth = 1.8f,
            shieldGrowth = 0.25f,
            damage = 25f,
            healthGrowth = 25f,
            healthRegen = 1.8f,
            jumpCount = 1,
            maxHealth = 150f,
            attackSpeed = 0.85f,
            jumpPowerGrowth = 0.2f,
            jumpPower = 25,
            moveSpeed = 5.5f
        };

        public override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[] 
        {
            /*
                new CustomRendererInfo
                {
                    childName = "SwordModel",
                    material = Materials.CreateHopooMaterial("matHenry"),
                },
                new CustomRendererInfo
                {
                    childName = "GunModel",
                },
                new CustomRendererInfo
                {
                    childName = "Model",
                }
            */

            
                new CustomRendererInfo
                {
                    childName = "BodyMesh_C",
                material = Materials.CreateHopooMaterial("MatVile"),
                },
                new CustomRendererInfo
                {
                    childName = "BodyMesh_M",
                    material = Materials.CreateHopooMaterial("MatVile"),
                },
                new CustomRendererInfo
                {
                    childName = "HandMesh_L_C",
                    material = Materials.CreateHopooMaterial("MatVile"),
                },
                new CustomRendererInfo
                {
                    childName = "HandMesh_L_M",
                    material = Materials.CreateHopooMaterial("MatVile"),
                },
                new CustomRendererInfo
                {
                    childName = "HandMesh_R_C",
                    material = Materials.CreateHopooMaterial("MatVile"),
                },
                new CustomRendererInfo
                {
                    childName = "HandMesh_R_M",
                    material = Materials.CreateHopooMaterial("MatVile"),
                },
                new CustomRendererInfo
                {
                    childName = "WeaponMesh_M",
                    material = Materials.CreateHopooMaterial("MatVile")
                }

    };

        public override UnlockableDef characterUnlockableDef => null;

        //public override Type characterMainState => typeof(EntityStates.GenericCharacterMain);

        public override Type characterMainState => typeof(SkillStates.BaseStates.Fury);

        public override Type characterDeathState => typeof(SkillStates.BaseStates.DeathState);

        public override Type characterSpawnState => typeof(SkillStates.BaseStates.SpawnState);

        public override ItemDisplaysBase itemDisplays => new HenryItemDisplays();

                                                                          //if you have more than one character, easily create a config to enable/disable them like this
        public override ConfigEntry<bool> characterEnabledConfig => null; //Modules.Config.CharacterEnableConfig(bodyName);

        private static UnlockableDef masterySkinUnlockableDef;

        public override void InitializeCharacter()
        {
            base.InitializeCharacter();
        }

        public override void InitializeUnlockables()
        {
            //uncomment this when you have a mastery skin. when you do, make sure you have an icon too
            //masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Modules.Achievements.MasteryAchievement>();
        }

        public override void InitializeHitboxes()
        {
            //ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();

            //example of how to create a hitbox
            //Transform hitboxTransform = childLocator.FindChild("SwordHitbox");
            //Modules.Prefabs.SetupHitbox(prefabCharacterModel.gameObject, hitboxTransform, "Sword");

            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            GameObject model = childLocator.gameObject;

            Transform hitboxTransform = childLocator.FindChild("GroundBox");
            Modules.Prefabs.SetupHitbox(model, hitboxTransform, "GroundBox");
            hitboxTransform.localScale = new Vector3(4f, 4f, 4f);

        }

        public override void InitializeSkills()
        {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);
            string prefix = VilePlugin.DEVELOPER_PREFIX + "_VILEV3_BODY_";

            Modules.Skills.PassiveSetup(bodyPrefab);

            #region Primary

            SkillDef CherryBlastSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "CHERRYBLAST_NAME",
                skillNameToken = prefix + "CHERRYBLAST_NAME",
                skillDescriptionToken = prefix + "CHERRYBLAST_DESCRIPTION",
                skillIcon = Modules.Assets.VCB,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.CherryBlast)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 0,
                requiredStock = 0,
                stockToConsume = 0,
                //keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            Modules.Skills.AddPrimarySkills(bodyPrefab, CherryBlastSkillDef);

            //----------------

            SkillDef Triple7SkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "TRIPLE7_NAME",
                skillNameToken = prefix + "TRIPLE7_NAME",
                skillDescriptionToken = prefix + "TRIPLE7_DESCRIPTION",
                skillIcon = Modules.Assets.VT7,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Triple7)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 0,
                requiredStock = 0,
                stockToConsume = 0,
                //keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            Modules.Skills.AddPrimarySkills(bodyPrefab, Triple7SkillDef);

            #endregion

            #region Secondary
            SkillDef shootSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "BUMPITYBOOM_NAME",
                skillNameToken = prefix + "BUMPITYBOOM_NAME",
                skillDescriptionToken = prefix + "BUMPITYBOOM_DESCRIPTION",
                skillIcon = Modules.Assets.VBB,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.BumpityBoom)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 3,
                baseRechargeInterval = 6f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                // keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            Modules.Skills.AddSecondarySkills(bodyPrefab, shootSkillDef);

            //------------------------

            SkillDef FrontRunnerSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "FRONTRUNNER_NAME",
                skillNameToken = prefix + "FRONTRUNNER_NAME",
                skillDescriptionToken = prefix + "FRONTRUNNER_DESCRIPTION",
                skillIcon = Modules.Assets.VFR,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.FrontRunner)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 5,
                baseRechargeInterval = 7f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                // keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            Modules.Skills.AddSecondarySkills(bodyPrefab, FrontRunnerSkillDef);

            //------------------------

            SkillDef NapalmBombSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "NAPALMBOMB_NAME",
                skillNameToken = prefix + "NAPALMBOMB_NAME",
                skillDescriptionToken = prefix + "NAPALMBOMB_DESCRIPTION",
                skillIcon = Modules.Assets.VNB,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.NapalmBomb)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 2,
                baseRechargeInterval = 8f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                // keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            Modules.Skills.AddSecondarySkills(bodyPrefab, NapalmBombSkillDef);


            #endregion

            #region Utility
            SkillDef EletricSparkSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "ELETRICSPARK_NAME",
                skillNameToken = prefix + "ELETRICSPARK_NAME",
                skillDescriptionToken = prefix + "ELETRICSPARK_DESCRIPTION",
                skillIcon = Modules.Assets.VES,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.EletricSpark)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 9f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = true,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddUtilitySkills(bodyPrefab, EletricSparkSkillDef);

            // --------------------------

            SkillDef ShotgunIceSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "SHOTGUNICE_NAME",
                skillNameToken = prefix + "SHOTGUNICE_NAME",
                skillDescriptionToken = prefix + "SHOTGUNICE_DESCRIPTION",
                skillIcon = Modules.Assets.VSI,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ShotgunIce)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 2,
                baseRechargeInterval = 8f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = true,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddUtilitySkills(bodyPrefab, ShotgunIceSkillDef);


            #endregion

            #region Special
            SkillDef BurningDriveSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "BURNINGDRIVE_NAME",
                skillNameToken = prefix + "BURNINGDRIVE_NAME",
                skillDescriptionToken = prefix + "BURNINGDRIVE_DESCRIPTION",
                skillIcon = Modules.Assets.VBD,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.BurningDrive)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 7f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = true,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, BurningDriveSkillDef);

            //---------------------------------------

            SkillDef CerberusPhantonSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "CERBERUSPHANTOM_NAME",
                skillNameToken = prefix + "CERBERUSPHANTOM_NAME",
                skillDescriptionToken = prefix + "CERBERUSPHANTOM_DESCRIPTION",
                skillIcon = Modules.Assets.VCP,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.CerberusPhantom)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 3,
                baseRechargeInterval = 6f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, CerberusPhantonSkillDef);


            #endregion
        }

        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ChildLocator childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            string prefix = VilePlugin.DEVELOPER_PREFIX + "_VILEV3_BODY_";

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(prefix + "DEFAULT_SKIN_NAME",
                Modules.Assets.VSkin,
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
            //pass in meshes as they are named in your assetbundle
            //defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
            //    "meshHenrySword",
            //    "meshHenryGun",
            //    "meshHenry");

            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion
            
            //uncomment this when you have a mastery skin
            #region MasterySkin
            /*
            //creating a new skindef as we did before
            SkinDef masterySkin = Modules.Skins.CreateSkinDef(HenryPlugin.DEVELOPER_PREFIX + "_HENRY_BODY_MASTERY_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject,
                masterySkinUnlockableDef);

            //adding the mesh replacements as above. 
            //if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
                "meshHenrySwordAlt",
                null,//no gun mesh replacement. use same gun mesh
                "meshHenryAlt");

            //masterySkin has a new set of RendererInfos (based on default rendererinfos)
            //you can simply access the RendererInfos defaultMaterials and set them to the new materials for your skin.
            masterySkin.rendererInfos[0].defaultMaterial = Modules.Materials.CreateHopooMaterial("matHenryAlt");
            masterySkin.rendererInfos[1].defaultMaterial = Modules.Materials.CreateHopooMaterial("matHenryAlt");
            masterySkin.rendererInfos[2].defaultMaterial = Modules.Materials.CreateHopooMaterial("matHenryAlt");

            //here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GunModel"),
                    shouldActivate = false,
                }
            };
            //simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            skins.Add(masterySkin);
            */
            #endregion

            skinController.skins = skins.ToArray();
        }
    }
}