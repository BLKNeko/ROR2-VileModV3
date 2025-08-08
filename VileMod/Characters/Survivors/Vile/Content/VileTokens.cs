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

            #region Ride Armors

            Language.Add(prefix + "RIDE_ENTER_GOLIATH_NAME", "Ride Armor: GOLIATH");
            Language.Add(prefix + "RIDE_ENTER_GOLIATH_DESCRIPTION", $"An extremely tough Ride Armor. Its brute force is unmatched. \n<color=#A020F0>Vile Bolt</color> Cost: <color=#A9A9A9>{VileStaticValues.RideArmorGoliathCost}</color>.");

            Language.Add(prefix + "RIDE_EXIT_GOLIATH_NAME", "Exit GOLIATH");
            Language.Add(prefix + "RIDE_EXIT_GOLIATH_DESCRIPTION", "Leave the Ride Armor in a standby state, ready to be called again.");

            Language.Add(prefix + "RIDE_RESUME_GOLIATH_NAME", "Resume GOLIATH");
            Language.Add(prefix + "RIDE_RESUME_GOLIATH_DESCRIPTION", "Reactivate the standby Ride Armor, calling it back into action without cost.");

            Language.Add(prefix + "RIDE_GOLIATH_PUNCH_COMBO_NAME", "GOLIATH Punch");
            Language.Add(prefix + "RIDE_GOLIATH_PUNCH_COMBO_DESCRIPTION", $"Deliver a flurry of devastating punches, dealing <style=cIsDamage>{100f * VileStaticValues.GPunch0DamageCoefficient}% damage</style>.");

            Language.Add(prefix + "RIDE_GOLIATH_SHOOT_NAME", "Light Shot");
            Language.Add(prefix + "RIDE_GOLIATH_SHOOT_DESCRIPTION", $"A powerful shot that pierces enemies, dealing <style=cIsDamage>{100f * VileStaticValues.GShotDamageCoefficient}% damage</style>.");

            Language.Add(prefix + "RIDE_GOLIATH_DASH_PUNCH_NAME", "Lunging Punch");
            Language.Add(prefix + "RIDE_GOLIATH_DASH_PUNCH_DESCRIPTION", $"Dash forward with a powerful punch, dealing <style=cIsDamage>{100f * VileStaticValues.GDashPunchDamageCoefficient}% damage</style>.");



            Language.Add(prefix + "RIDE_ENTER_HAWK_NAME", "Ride Armor: HAWK");
            Language.Add(prefix + "RIDE_ENTER_HAWK_DESCRIPTION", $"This Ride Armor can fly across the map, but lacks the durability of the others.\n<color=#A020F0>Vile Bolt</color> Cost: <color=#A9A9A9>{VileStaticValues.RideArmorHawkCost}</color>.");

            Language.Add(prefix + "RIDE_EXIT_HAWK_NAME", "Exit HAWK");
            Language.Add(prefix + "RIDE_EXIT_HAWK_DESCRIPTION", "Leave the Ride Armor in a standby state, ready to be called again.");

            Language.Add(prefix + "RIDE_RESUME_HAWK_NAME", "Resume HAWK");
            Language.Add(prefix + "RIDE_RESUME_HAWK_DESCRIPTION", "Reactivate the standby Ride Armor, calling it back into action without cost.");

            Language.Add(prefix + "RIDE_HAWK_GUN_NAME", "HAWK Missiles");
            Language.Add(prefix + "RIDE_HAWK_GUN_DESCRIPTION", $"Fire three tracking missiles, each dealing <style=cIsDamage>{100f * VileStaticValues.HGun1DamageCoefficient}% damage</style>.");

            Language.Add(prefix + "RIDE_HAWK_GUN_BARRAGE_NAME", "Hawk Missile Barrage");
            Language.Add(prefix + "RIDE_HAWK_GUN_BARRAGE_DESCRIPTION", $"While tracking an enemy, unleash a barrage of homing missiles, each dealing <style=cIsDamage>{100f * VileStaticValues.HGunBarrageDamageCoefficient}% damage</style>. The number of missiles depends on Vile's power level.");

            Language.Add(prefix + "RIDE_HAWK_DASH_NAME", "HAWK Dash");
            Language.Add(prefix + "RIDE_HAWK_DASH_DESCRIPTION", "Dash through the air for improved aerial mobility.");




            Language.Add(prefix + "RIDE_ENTER_CYCLOPS_NAME", "Ride Armor: CYCLOPS");
            Language.Add(prefix + "RIDE_ENTER_CYCLOPS_DESCRIPTION", $"This Ride Armor offers solid resistance and comes equipped with a powerful shoulder-mounted plasma gun, capable of devastating plasma shots.\n<color=#A020F0>Vile Bolt</color> Cost: <color=#A9A9A9>{VileStaticValues.RideArmorCyclopsCost}</color>.");

            Language.Add(prefix + "RIDE_EXIT_CYCLOPS_NAME", "Exit CYCLOPS");
            Language.Add(prefix + "RIDE_EXIT_CYCLOPS_DESCRIPTION", "Leave the Ride Armor in a standby state, ready to be called again.");

            Language.Add(prefix + "RIDE_RESUME_CYCLOPS_NAME", "Resume CYCLOPS");
            Language.Add(prefix + "RIDE_RESUME_CYCLOPS_DESCRIPTION", "Reactivate the standby Ride Armor, calling it back into action without cost.");

            Language.Add(prefix + "RIDE_CYCLOPS_PUNCH_NAME", "CYCLOPS Punch");
            Language.Add(prefix + "RIDE_CYCLOPS_PUNCH_DESCRIPTION", $"Deliver a flurry of strong punches, each dealing <style=cIsDamage>{100f * VileStaticValues.CyPunchDamageCoefficient}% damage</style>.");

            Language.Add(prefix + "RIDE_CYCLOPS_SHOT_NAME", "CYCLOPS Plasma Shot");
            Language.Add(prefix + "RIDE_CYCLOPS_SHOT_DESCRIPTION", $"Fire a powerful plasma blast that pierces enemies, dealing <style=cIsDamage>{100f * VileStaticValues.CyShotDamageCoefficient}% damage</style> and leaving behind a shock sphere that stuns nearby foes.");

            Language.Add(prefix + "RIDE_CYCLOPS_DASH_NAME", "CYCLOPS Dash");
            Language.Add(prefix + "RIDE_CYCLOPS_DASH_DESCRIPTION", "Dash forward with slightly increased speed.");



            Language.Add(prefix + "RIDE_REPAIR_NAME", "Repair Ride Armor");
            Language.Add(prefix + "RIDE_REPAIR_DESCRIPTION", "Use your Vile Bolts to repair your active Ride Armor.");


            #endregion

            #region Units

            Language.Add(prefix + "UNIT_METTAURCURE_NAME", "Unit: Mettaur Cure");
            Language.Add(prefix + "UNIT_METTAURCURE_DESCRIPTION", $"A healing Mettaur that periodically restores health to Vile and his Ride Armor. \n<color=#A020F0>Vile Bolt</color> Cost: <color=#A9A9A9>{VileStaticValues.UnitMettaurcureBoltCost}</color>.");

            Language.Add(prefix + "UNIT_METTAURCOMMANDER_NAME", "Unit: Mettaur Commander");
            Language.Add(prefix + "UNIT_METTAURCOMMANDER_DESCRIPTION", $"A support Mettaur that increases Vile's shield and also regenerates it, granting a bonus to the Ride Armor as well. \n<color=#A020F0>Vile Bolt</color> Cost: <color=#A9A9A9>{VileStaticValues.UnitMettaurCommanderBoltCost}</color>.");

            Language.Add(prefix + "UNIT_GUNVOLT_NAME", "Unit: GunVolt");
            Language.Add(prefix + "UNIT_GUNVOLT_DESCRIPTION", $"This unit is slow but fires two homing missiles, each dealing <style=cIsDamage>{100f * VileStaticValues.UnitGunVoltDamageCoefficient}% damage</style>. \n<color=#A020F0>Vile Bolt</color> Cost: <color=#A9A9A9>{VileStaticValues.UnitGunVoltBoltCost}</color>.");

            Language.Add(prefix + "UNIT_BIGBIT_NAME", "Unit: BigBit");
            Language.Add(prefix + "UNIT_BIGBIT_DESCRIPTION", $"This small floating unit shoots at nearby enemies, dealing <style=cIsDamage>{100f * VileStaticValues.UnitBigBitDamageCoefficient}% damage</style>. \n<color=#A020F0>Vile Bolt</color> Cost: <color=#A9A9A9>{VileStaticValues.UnitBigBitBoltCost}</color>.");

            Language.Add(prefix + "UNIT_PREONE_NAME", "Unit: Preon Elite");
            Language.Add(prefix + "UNIT_PREONE_DESCRIPTION", $"This unit shoots at nearby enemies, dealing <style=cIsDamage>{100f * VileStaticValues.UnitPreonEDamageCoefficient}% damage</style>. \n<color=#A020F0>Vile Bolt</color> Cost: <color=#A9A9A9>{VileStaticValues.UnitPreonEBoltCost}</color>.");

            Language.Add(prefix + "UNIT_NIGHTMAREVIRUS_NAME", "Unit: Nightmare Virus");
            Language.Add(prefix + "UNIT_NIGHTMAREVIRUS_DESCRIPTION", $"This dangerous unit dashes towards enemies, infecting them with its virus and dealing <style=cIsDamage>{100f * VileStaticValues.UnitNightmareVDamageCoefficient}% damage</style>. \n<color=#A020F0>Vile Bolt</color> Cost: <color=#A9A9A9>{VileStaticValues.UnitNightmareBoltCostt}</color>.");

            Language.Add(prefix + "UNIT_MAMEQ_NAME", "Unit: MAME-Q");
            Language.Add(prefix + "UNIT_MAMEQ_DESCRIPTION", $"This supportive unit boosts Vile's stats while active. \n<color=#A020F0>Vile Bolt</color> Cost: <color=#A9A9A9>{VileStaticValues.UnitMameQBoltCost}</color>.");

            Language.Add(prefix + "UNIT_SPIKY_NAME", "Unit: Spiky");
            Language.Add(prefix + "UNIT_SPIKY_DESCRIPTION", $"This unit circles around Vile, damaging enemies on contact for <style=cIsDamage>{100f * VileStaticValues.UnitSpikyDamageCoefficient}% damage</style>. \n<color=#A020F0>Vile Bolt</color> Cost: <color=#A9A9A9>{VileStaticValues.UnitSpikyBoltCost}</color>.");

            Language.Add(prefix + "UNIT_TOGERICS_NAME", "Unit: Togerics");
            Language.Add(prefix + "UNIT_TOGERICS_DESCRIPTION", $"This flower-type unit cannot move, but any enemy within its area will suffer constant damage and be afflicted with bleed. This unit deals <style=cIsDamage>{100f * VileStaticValues.UnitTogericsDamageCoefficient}% damage</style>. \n<color=#A020F0>Vile Bolt</color> Cost: <color=#A9A9A9>{VileStaticValues.UnitTogericsBoltCost}</color>.");


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
