using RoR2;
using UnityEngine;

namespace VileMod.Survivors.Vile
{
    public static class VileBuffs
    {
        // armor buff gained during roll
        public static BuffDef armorBuff;

        public static BuffDef GoliathBuff;

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

        }
    }
}
