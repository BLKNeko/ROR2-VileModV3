using BepInEx;
using VileMod.Modules.Survivors;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using RoR2.UI;
using UnityEngine;
using UnityEngine.UI;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

//rename this namespace
namespace VileMod
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "SoundAPI",
        "UnlockableAPI"
    })]

    public class VilePlugin : BaseUnityPlugin
    {
        // if you don't change these you're giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.BLKNeko.VileModV3";
        public const string MODNAME = "VileModV3";
        public const string MODVERSION = "3.0.0";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string DEVELOPER_PREFIX = "BLKNeko";

        public static VilePlugin instance;


        //public static float BarValue = 0.01f;

        //public static bool isvisible = false;

        //public static bool needtocheck = true;

        //public static GameObject BarObject;
        // 1 = 0,00212

        //public static GameObject BarObjectBG;


        //private HUD hud = null;

        private void Awake()
        {
            instance = this;

            Log.Init(Logger);
            Modules.Assets.Initialize(); // load assets and read config
            Modules.Config.ReadConfig();
            Modules.States.RegisterStates(); // register states for networking
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            Modules.Tokens.AddTokens(); // register name tokens
            Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

            // survivor initialization
            new MyCharacter().Initialize();

            // now make a content pack and add it- this part will change with the next update
            new Modules.ContentPacks().Initialize();

            Hook();

            //On.RoR2.UI.HUD.Awake -= VileHeatGaugeHud;
        }


        /*

        public void VileHeatGaugeHud(On.RoR2.UI.HUD.orig_Awake orig, RoR2.UI.HUD self)
        {
            orig(self); // Don't forget to call this, or the vanilla / other mods' codes will not execute!
            hud = self;
            // hud.mainContainer.transform // This will return the main container. You should put your UI elements under it or its children!
            // Rest of the code is to go here

            //ANCHOR MAX = rectTransform.anchorMax = new Vector2(0.782f, 0.72f);



            //---------------BG--------------

            GameObject myObjectbg = new GameObject("bg");
            //GameObject myObject = Modules.Assets.MoraleBar;
            VilePlugin.BarObjectBG = myObjectbg;


            VilePlugin.BarObjectBG.transform.SetParent(hud.mainContainer.transform);
            RectTransform rectTransform2 = myObjectbg.AddComponent<RectTransform>();
            //rectTransform.anchorMin = Vector2.zero;
            //rectTransform.anchorMax = Vector2.one;
            rectTransform2.anchorMin = new Vector2(0.57f, 0.7f);
            rectTransform2.anchorMax = new Vector2(0.782f, 0.72f);
            //rectTransform.sizeDelta = Vector2.zero;
            rectTransform2.sizeDelta = new Vector2(0.2f, 0.2f);
            //rectTransform.anchoredPosition = Vector2.zero;



            VilePlugin.BarObjectBG.AddComponent<Image>();
            VilePlugin.BarObjectBG.GetComponent<Image>().sprite = Modules.Assets.BarSprite;
            //HaseoPlugin.BarObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("textures/itemicons/texBearIcon");
            VilePlugin.BarObjectBG.GetComponent<Image>().color = new Color(0, 0, 0, 250);
            //HaseoPlugin.BarObject.GetComponent<Image>().type = Image.Type.Filled;
            //HaseoPlugin.BarObject.GetComponent<Image>().fillMethod = Image.FillMethod.Horizontal;
            //HaseoPlugin.BarObject.GetComponent<Image>().fillOrigin = 0;
            // HaseoPlugin.BarObject.GetComponent<Image>().fillAmount = BarValue;




            //-----------------MORALE-------------

            GameObject myObject = new GameObject("bar");
            //GameObject myObject = Modules.Assets.MoraleBar;
            VilePlugin.BarObject = myObject;


            VilePlugin.BarObject.transform.SetParent(hud.mainContainer.transform);
            RectTransform rectTransform = myObject.AddComponent<RectTransform>();
            //rectTransform.anchorMin = Vector2.zero;
            //rectTransform.anchorMax = Vector2.one;
            rectTransform.anchorMin = new Vector2(0.57f, 0.7f);

            //rectTransform.anchorMax = new Vector2(0.782f, 0.72f);

            rectTransform.anchorMax = new Vector2(1f, 1f);

            //rectTransform.sizeDelta = Vector2.zero;

            //rectTransform.sizeDelta = new Vector2(0.2f, 0.2f);
            rectTransform.sizeDelta = new Vector2(1f, 1f);

            //rectTransform.anchoredPosition = Vector2.zero;



            VilePlugin.BarObject.AddComponent<Image>();
            VilePlugin.BarObject.GetComponent<Image>().sprite = Modules.Assets.BarSprite;
            //HaseoPlugin.BarObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("textures/itemicons/texBearIcon");
            VilePlugin.BarObject.GetComponent<Image>().color = new Color(0, 188, 255, 100);
            VilePlugin.BarObject.GetComponent<Image>().type = Image.Type.Filled;
            VilePlugin.BarObject.GetComponent<Image>().fillMethod = Image.FillMethod.Horizontal;
            VilePlugin.BarObject.GetComponent<Image>().fillOrigin = 0;
            VilePlugin.BarObject.GetComponent<Image>().fillAmount = BarValue;

            Debug.Log("Haseo Bar Object:");
            Debug.Log(VilePlugin.BarObject);
            Debug.Log("Haseo Bar Object Image component:");
            Debug.Log(VilePlugin.BarObject.GetComponent<Image>());
            Debug.Log("Haseo Bar Object sprite:");
            Debug.Log(VilePlugin.BarObject.GetComponent<Image>().sprite);
            Debug.Log("Haseo Bar Object Type:");
            Debug.Log(VilePlugin.BarObject.GetComponent<Image>().type);
            Debug.Log("Haseo Bar Object fillmothod:");
            Debug.Log(VilePlugin.BarObject.GetComponent<Image>().fillMethod);
            Debug.Log("Haseo Bar Object fillorigin:");
            Debug.Log(VilePlugin.BarObject.GetComponent<Image>().fillOrigin);
            Debug.Log("Haseo Bar Object fillamont:");
            Debug.Log(VilePlugin.BarObject.GetComponent<Image>().fillAmount);










            //myObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("textures/itemicons/texBearIcon");



            orig(self);
        }

        private void Update()
        {
            if (VilePlugin.BarObject)
            {
                VilePlugin.BarObject.GetComponent<Image>().fillAmount = BarValue;

                //HaseoPlugin.CounterObject.GetComponent<Text>().text = (BarValue * 100).ToString() + "% / 100%";
            }

            if (needtocheck)
            {
                if (isvisible)
                    On.RoR2.UI.HUD.Awake += VileHeatGaugeHud;
                else
                    On.RoR2.UI.HUD.Awake -= VileHeatGaugeHud;

                needtocheck = false;
            }

        }




        private void OnDestroy()
        {
            needtocheck = false;
            isvisible = false;
            On.RoR2.UI.HUD.Awake -= VileHeatGaugeHud;
        }

        */


        private void Hook()
        {
            // run hooks here, disabling one is as simple as commenting out the line
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;

            //On.RoR2.PreGameRuleVoteController.Awake += TiraHudGameOver3;
        }


        /*
        private void TiraHudGameOver3(On.RoR2.PreGameRuleVoteController.orig_Awake orig, PreGameRuleVoteController self)
        {
            orig(self);

            if (self)
            {
                Debug.Log("SAI DEMONIO! 0^0 3");
                On.RoR2.UI.HUD.Awake -= VileHeatGaugeHud;
            }
        }
        */

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            // a simple stat hook, adds armor after stats are recalculated
            if (self)
            {
                if (self.HasBuff(Modules.Buffs.armorBuff))
                {
                    self.armor += 300f;
                }
            }


            if (self)
            {
                if (self.HasBuff(Modules.Buffs.VileFuryBuff))
                {
                    self.moveSpeed *= 1.3f;
                    self.attackSpeed *= 1.25f;
                    self.damage *= 1.4f;
                    self.regen *= 1.4f;

                }
            }


        }
    }
}