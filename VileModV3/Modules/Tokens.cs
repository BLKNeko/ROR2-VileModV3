using R2API;
using System;

namespace VileMod.Modules
{
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            #region Vile
            string prefix = VilePlugin.DEVELOPER_PREFIX + "_VILEV3_BODY_";

            string desc = "Vile, the EX-Maverick Hunter.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Vile's Cherry Blast has a low start so use it after any skill for a momentary buff and faster start" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Vile is very powerfull, but is pretty slow" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > When activated, Vile's Passive give him a life steal buff for 6 seconds, so try to cause the maximum damage possible" + Environment.NewLine + Environment.NewLine;


            string outro = "Back to the Hunter Base.";
            string outroFailure = "I...Failed....";

            LanguageAPI.Add(prefix + "NAME", "Vile");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Vile, EX-Maverick Hunter");
            LanguageAPI.Add(prefix + "LORE", "EX-Maverick Hunter");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Rage");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "<style=cIsUtility>Vile won't give up that easily from a fight, moved by his anger he get stronger in a critical state.</style> <style=cIsHealing>When in low health vile gain 10 seconds of buffs</style>.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "CHERRYBLAST_NAME", "CherryBlast");
            LanguageAPI.Add(prefix + "CHERRYBLAST_DESCRIPTION", "Vile's gatling can fire super fast bullets after completely heated, dealing <style=cIsDamage>25% damage</style>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "BUMPITYBOOM_NAME", "BumpityBoom");
            LanguageAPI.Add(prefix + "BUMPITYBOOM_DESCRIPTION", "Vile throws two granades, dealing <style=cIsDamage>250% damage</style>.");


            LanguageAPI.Add(prefix + "FRONTRUNNER_NAME", "Front Runner");
            LanguageAPI.Add(prefix + "FRONTRUNNER_DESCRIPTION", "A cannon shot that explodes on impact, dealing <style=cIsDamage>300% damage</style>.");

            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "ELETRICSPARK_NAME", "Electric Shock Round");
            LanguageAPI.Add(prefix + "ELETRICSPARK_DESCRIPTION", "Fire an eletric bomb, dealing <style=cIsDamage>1000% damage</style> and paralize enemies for 5s.");

            LanguageAPI.Add(prefix + "SHOTGUNICE_NAME", "Shotgun Ice");
            LanguageAPI.Add(prefix + "SHOTGUNICE_DESCRIPTION", "A powerful ice shot that cause <style=cIsDamage>400% damage</style> and freezing the enemies.");

            #endregion

            #region Special
            LanguageAPI.Add(prefix + "BURNINGDRIVE_NAME", "Burning Drive");
            LanguageAPI.Add(prefix + "BURNINGDRIVE_DESCRIPTION", "Create a powerful ball of flame using nearby oxygen as fuel, dealing <style=cIsDamage>1000% damage</style>.");

            LanguageAPI.Add(prefix + "CERBERUSPHANTOM_NAME", "Cerberus Phantom");
            LanguageAPI.Add(prefix + "CERBERUSPHANTOM_DESCRIPTION", "Shoot a spread of 3 lasers, dealing <style=cIsDamage>250% damage</style>.");

            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "VileV2: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As VileV2, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "VileV2: Mastery");
            #endregion
            #endregion
        }
    }
}