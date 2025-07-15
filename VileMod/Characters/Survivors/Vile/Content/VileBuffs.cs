using RoR2;
using UnityEngine;

namespace VileMod.Survivors.Vile
{
    public static class VileBuffs
    {
        // armor buff gained during roll
        public static BuffDef armorBuff;

        public static BuffDef GoliathBuff;

        public static BuffDef PrimaryHeatBuff;


        public static BuffDef PrimaryIceBuff;
        public static BuffDef PrimaryFlameBuff;
        public static BuffDef PrimaryShockBuff;

        public static void Init(AssetBundle assetBundle)
        {
            armorBuff = Modules.Content.CreateAndAddBuff("HenryArmorBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

            GoliathBuff = Modules.Content.CreateAndAddBuff("GoliathBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

            PrimaryHeatBuff = Modules.Content.CreateAndAddBuff("PrimaryHeatBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

            PrimaryIceBuff = Modules.Content.CreateAndAddBuff("PrimaryIceBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

            PrimaryFlameBuff = Modules.Content.CreateAndAddBuff("PrimaryFlameBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

            PrimaryShockBuff = Modules.Content.CreateAndAddBuff("PrimaryShockBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

        }
    }
}
