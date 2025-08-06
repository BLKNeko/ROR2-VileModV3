using RoR2;
using UnityEngine;

namespace VileMod.Survivors.Vile
{
    public static class VileBuffs
    {
        // armor buff gained during roll
        public static BuffDef armorBuff;

        public static BuffDef RideArmorEnabledBuff;

        public static BuffDef GoliathBuff;
        public static BuffDef HawkBuff;
        public static BuffDef HawkDashBuff;
        public static BuffDef CyclopsBuff;

        public static BuffDef PrimaryHeatBuff;

        public static BuffDef OverHeatDebuff;
        public static BuffDef nightmareVirusDebuff;


        public static BuffDef PrimaryIceBuff;
        public static BuffDef PrimaryFlameBuff;
        public static BuffDef PrimaryShockBuff;


        public static BuffDef MetComBuff;
        public static BuffDef MameqBuff;

        public static void Init(AssetBundle assetBundle)
        {
            armorBuff = Modules.Content.CreateAndAddBuff("HenryArmorBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

            RideArmorEnabledBuff = Modules.Content.CreateAndAddBuff("RideArmorEnabledBuff",
                VileAssets.RideArmorEnabledBuffcon,
                Color.white,
                false,
                false);

            GoliathBuff = Modules.Content.CreateAndAddBuff("GoliathBuff",
                VileAssets.CallGoliathSkillIcon,
                Color.white,
                false,
                false);

            HawkBuff = Modules.Content.CreateAndAddBuff("HawkBuff",
                VileAssets.CallHawkSkillIcon,
                Color.white,
                false,
                false);

            HawkDashBuff = Modules.Content.CreateAndAddBuff("HawkDashBuff",
                VileAssets.RideDashSkillIcon,
                Color.white,
                false,
                false);

            CyclopsBuff = Modules.Content.CreateAndAddBuff("CyclopsBuff",
                VileAssets.CallCyclopsSkillIcon,
                Color.white,
                false,
                false);

            PrimaryHeatBuff = Modules.Content.CreateAndAddBuff("PrimaryHeatBuff",
                VileAssets.PrimaryHeatBuffIcon,
                Color.white,
                false,
                false);

            PrimaryIceBuff = Modules.Content.CreateAndAddBuff("PrimaryIceBuff",
                VileAssets.PrimaryIceBuffIcon,
                Color.white,
                false,
                false);

            PrimaryFlameBuff = Modules.Content.CreateAndAddBuff("PrimaryFlameBuff",
                VileAssets.PrimaryFireBuffIcon,
                Color.white,
                false,
                false);

            PrimaryShockBuff = Modules.Content.CreateAndAddBuff("PrimaryShockBuff",
                VileAssets.PrimaryShockBuffIcon,
                Color.white,
                false,
                false);

            MetComBuff = Modules.Content.CreateAndAddBuff("MetComBuff",
                VileAssets.UnitMetComSkillIcon,
                Color.white,
                false,
                true);

            MameqBuff = Modules.Content.CreateAndAddBuff("MAME-QBuff",
                VileAssets.UnitMMQSkillIcon,
                Color.white,
                false,
                true);


            OverHeatDebuff = Modules.Content.CreateAndAddBuff("OverHeatDebuff",
                VileAssets.OverHeatDebuffIcon,
                Color.white,
                false,
                true);

            nightmareVirusDebuff = Modules.Content.CreateAndAddBuff("NightmareVirusDebuff",
                VileAssets.UnitNightmareVSkillIcon,
                Color.white,
                false,
                true);

        }
    }
}
