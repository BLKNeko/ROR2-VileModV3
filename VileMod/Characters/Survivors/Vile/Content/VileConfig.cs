using BepInEx.Configuration;
using VileMod.Modules;

namespace VileMod.Survivors.Vile
{
    public static class VileConfig
    {
        public static ConfigEntry<bool> someConfigBool;
        public static ConfigEntry<float> someConfigFloat;
        public static ConfigEntry<float> someConfigFloatWithCustomRange;

        public static ConfigEntry<float> hk_flyingSpeedMultiplier;

        public static void Init()
        {
            string section = "Vile";

            someConfigBool = Config.BindAndOptions(
                section,
                "someConfigBool",
                true,
                "this creates a bool config, and a checkbox option in risk of options");

            someConfigFloat = Config.BindAndOptions(
                section,
                "someConfigfloat",
                5f);//blank description will default to just the name

            someConfigFloatWithCustomRange = Config.BindAndOptions(
                section,
                "someConfigfloat2",
                5f,
                0,
                50,
                "if a custom range is not passed in, a float will default to a slider with range 0-20. risk of options only has sliders");

            hk_flyingSpeedMultiplier = Config.BindAndOptions(
                section,
                "Hawk Flying Speed Bonus",
                1.5f,
                1f,
                2f,
                "The multiplier applied when Hawk is flying.");
        }
    }
}
