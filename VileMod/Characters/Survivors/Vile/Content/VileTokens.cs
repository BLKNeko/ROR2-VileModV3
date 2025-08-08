using R2API;
using System;
using VileMod.Modules;
using VileMod.Survivors.Vile.Achievements;

namespace VileMod.Survivors.Vile
{
    public static class VileTokens
    {
        public static void Init()
        {
            AddVileTokens();

            ////uncomment this to spit out a lanuage file with all the above tokens that people can translate
            ////make sure you set Language.usingLanguageFolder and printingEnabled to true
            //Language.PrintOutput("Vile.txt");
            ////refer to guide on how to build and distribute your mod with the proper folders
        }

        public static void AddVileTokens()
        {
            string prefix = VileSurvivor.VILE_PREFIX;

            string desc = "<color=#A020F0>Vile, The X-Hunter.</color>\n\n";
            desc += "< ! > <color=#A020F0>Vile</color>'s <color=#f79902>Vulcans</color> unleash a relentless barrage. Choose one of four brutal types to dominate your foes!\n\n";
            desc += "< ! > By channeling energy from his <color=#2302f7>skills</color>, <color=#A020F0>Vile</color> can overload his Vulcans with elemental devastation.\n\n";
            desc += "< ! > Built like a war machine, <color=#A020F0>Vile</color> shrugs off fall damage—but his sheer power comes at the cost of speed.\n\n";
            desc += "< ! > <color=#f79902>Vile’s Fury</color> floods him with unstable power, turning him into a one-man army. Use it wisely... its cooldown is brutal.\n\n";
            desc += "< ! > Spend <color=#b300b3>Vile Bolts</color> to summon lethal support units. Earn Bolts by crushing enemies and unlocking gold capsules.\n\n";
            desc += "< ! > When the battlefield demands absolute dominance, <color=#7f8c8d>Vile</color> calls in his <color=#d35400>Ride Armor</color> — a walking death machine.\n\n";


            string outro = "<i><color=#a50000>...You see now, X?</color> <color=#f79902>I was built to win.</color></i>";
            string outroFailure = "<i><color=#a50000>No...</color> <color=#5555ff>I won't lose...</color> <color=#f79902>Not to you, X!</color></i>";


            Language.Add(prefix + "NAME", "Vile");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "X-Hunter");
            Language.Add(prefix + "LORE", "EX-Maverick Hunter");
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            LanguageAPI.Add(prefix + "VILE_KEYWORD_VBOLT",
                "<color=#A020F0>[VILE BOLTS]</color>\n" +
                "This skill <color=#FF8C00>demands</color> a certain amount of <color=#A9A9A9>Vile Bolts</color> to be unleashed!\n" +
                "Crush your foes to harvest <color=#A9A9A9>Bolts</color> and crack open <color=#FFD700>gold capsules</color> for more.");

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", "Vile passive");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_SLASH_NAME", "Sword");
            Language.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Tokens.agilePrefix + $"Swing forward for <style=cIsDamage>{100f * VileStaticValues.swordDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_GUN_NAME", "Handgun");
            Language.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Tokens.agilePrefix + $"Fire a handgun for <style=cIsDamage>{100f * VileStaticValues.gunDamageCoefficient}% damage</style>.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_ROLL_NAME", "Roll");
            Language.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Roll a short distance, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_BOMB_NAME", "Bomb");
            Language.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * VileStaticValues.bombDamageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(HenryMasteryAchievement.identifier), "Vile: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(HenryMasteryAchievement.identifier), "As Vile, beat the game or obliterate on Monsoon.");
            #endregion
        }

    }
}
