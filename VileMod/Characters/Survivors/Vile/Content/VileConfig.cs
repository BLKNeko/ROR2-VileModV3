using BepInEx.Configuration;
using VileMod.Modules;

namespace VileMod.Survivors.Vile
{
    public static class VileConfig
    {

        public static ConfigEntry<bool> enableYipeeBool;

        public static ConfigEntry<bool> enableVoiceBool;
        public static ConfigEntry<int> enableFootstep;

        public static ConfigEntry<float> rideArmorDestroyPenalty;

        public static ConfigEntry<float> hk_flyingSpeedMultiplier;

        public static void Init()
        {
            string section = "Vile";

            enableVoiceBool = Config.BindAndOptions(
                section,
                "EnableVoice",
                true,
                "At certain moments or when using a skill, Vile may talk or scream. If you prefer to disable this feature, you can turn it off here.");

            enableFootstep = Config.BindAndOptions(
                section,
                "Enable Vile Footstep",
                1,
                0,
                2,
                "Vile footstep SFX. \n\n Turn it OFF also turns ride armor footstep OFF \n\n 0 = OFF \n\n 1 = Comand Mission SFX \n\n 2 = MegamanX8 SFX");

            hk_flyingSpeedMultiplier = Config.BindAndOptions(
                section,
                "Hawk Flying Speed Bonus",
                1.5f,
                1f,
                2f,
                "The multiplier applied when Hawk is flying.");

            enableYipeeBool = Config.BindAndOptions(
                section,
                "Yipeee SFX",
                false,
                "Change the SFX of a certain cute unit to YIPEEEE.");
        }
    }
}
