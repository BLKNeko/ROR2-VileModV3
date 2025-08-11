using MegamanXMod.Survivors.X.SkillStates;
using System;
using VileMod.Survivors.Vile.SkillStates;

namespace VileMod.Survivors.Vile
{
    public static class VileStaticValues
    {
        public const float swordDamageCoefficient = 2.8f;

        public const float gunDamageCoefficient = 4.2f;

        public const float bombDamageCoefficient = 16f;

        //VILE

        // PRIMARY

        public const float CherryBlastDamageCoefficient = 0.35f;

        public const float DistanceNeedlerDamageCoefficient = 0.7f;

        public const float Triple7DamageCoefficient = 0.3f;

        public const float ZipZapperDamageCoefficient = 0.25f;

        // SECONDARY

        public const float VBumpityBoomDamageCoefficient = 2.5f;

        public const float VNapalmBombDamageCoefficient = 3f;

        public const float VFrontRunnerDamageCoefficient = 3f;

        


        // UTILITY

        public const float VShotgunIceDamageCoefficient = 6f;

        public const float VEletricSparkDamageCoefficient = 7f;

        public const float VFlameRoundDamageCoefficient = 6.5f;

        public const float VFlameRoundPillarDamageCoefficient = 0.7f;


        // SPECIAL

        public const float VBurningDriveDamageCoefficient = 8f;

        public const float VCerberusPhantonDamageCoefficient = 3f;

        public const float VSeaDragonRageDamageCoefficient = 0.2f;


        // RIDE ARMOR

        public const float GPunch0DamageCoefficient = 3f;

        public const float GPunch1DamageCoefficient = 3f;

        public const float GPunch2DamageCoefficient = 3f;

        public const float GDashPunchDamageCoefficient = 5f;

        public const float GShotDamageCoefficient = 2f;

        public const float HGun1DamageCoefficient = 0.85f;

        public const float HGunBarrageDamageCoefficient = 1f;

        public const float CyPunchDamageCoefficient = 1.7f;

        public const float CyShotDamageCoefficient = 5f;

        public const float CyPlasmaShockDamageCoefficient = 0.2f;

        public const float RepairRideArmorDamageCoefficient = 1f;

        public const float DestroyRideArmorDamageCoefficient = 15f;

        // UNIT

        public const float UnitBigBitDamageCoefficient = 0.75f;

        public const float UnitMettaurCommanderDamageCoefficient = 1f;

        public const float UnitMettaurcureDamageCoefficient = 1f;

        public const float UnitNightmareVDamageCoefficient = 2f;

        public const float UnitPreonEDamageCoefficient = 1.5f;

        public const float UnitTogericsDamageCoefficient = 0.5f;

        public const float UnitGunVoltDamageCoefficient = 1f;

        public const float UnitSpikyDamageCoefficient = 0.8f;

        // VILE BOLT

        public const int UnitMettaurcureBoltCost = 150;
        public const int UnitBigBitBoltCost = 125;
        public const int UnitSpikyBoltCost = 130;

        public const int UnitPreonEBoltCost = 250;
        public const int UnitMettaurCommanderBoltCost = 280;
        public const int UnitGunVoltBoltCost = 270;

        public const int UnitNightmareBoltCostt = 380;
        public const int UnitTogericsBoltCost = 350;
        public const int UnitMameQBoltCost = 420;

        public const int RideArmorGoliathCost = 0; // 750
        public const int RideArmorHawkCost = 0; // 700
        public const int RideArmorCyclopsCost = 0; // 725





        //SOUND STRINGS

        public static readonly string Play_Vile_Armed = "Play_Vile_Armed";
        public static readonly string Play_Vile_Attack = "Play_Vile_Attack";
        public static readonly string Play_Vile_Come_Here = "Play_Vile_Come_Here"; // Corrigido: Come_Here
        public static readonly string Play_Vile_Charge_Shoot = "Play_Vile_Charge_Shoot";
        public static readonly string Play_Vile_Cherry_Blast = "Play_Vile_Cherry_Blast";
        public static readonly string Play_Vile_CYPunch_SFX = "Play_Vile_CYPunch_SFX";
        public static readonly string Play_Vile_Die = "Play_Vile_Die";
        public static readonly string Play_Vile_Death_Explosion = "Play_Vile_Death_Explosion";
        public static readonly string Play_Vile_Eat_This = "Play_Vile_Eat_This";
        public static readonly string Play_Vile_Error = "Play_Vile_Error";
        public static readonly string Play_Fire_SFX = "Play_Fire_SFX";
        public static readonly string Play_Vile_Footstep_SFX = "Play_Vile_Footstep_SFX";
        public static readonly string Play_Vile_Footstep_X8_SFX = "Play_Vile_Footstep_X8_SFX";
        public static readonly string Play_Vile_Frag_Drop = "Play_Vile_Frag_Drop";
        public static readonly string Play_Vile_Fury = "Play_Vile_Fury";
        public static readonly string Play_Vile_GPunch_SFX = "Play_Vile_GPunch_SFX";
        public static readonly string Play_Vile_Ha = "Play_Vile_Ha";
        public static readonly string Play_Vile_Haaa = "Play_Vile_Haaa";
        public static readonly string Play_Vile_Hahahaha = "Play_Vile_Hahahaha";
        public static readonly string Play_Vile_Heree = "Play_Vile_Heree";
        public static readonly string Play_Vile_I_Cant_Lose_To_X = "Play_Vile_I_Cant_Lose_To_X";
        public static readonly string Play_Vile_Laser_Shot = "Play_Vile_Laser_Shot";
        public static readonly string Play_Vile_Missile_SFX = "Play_Vile_Missile_SFX";
        public static readonly string Play_MMQVSFX = "Play_MMQVSFX";
        public static readonly string Play_Vile_Overheat_SFX = "Play_Vile_Overheat_SFX";
        public static readonly string Play_Vile_Ready = "Play_Vile_Ready";
        public static readonly string Play_Vile_Return = "Play_Vile_Return";
        public static readonly string Play_Vile_Ride_Armor_Boost = "Play_Vile_Ride_Armor_Boost";
        public static readonly string Play_Vile_Ride_Armor_Explosion = "Play_Vile_Ride_Armor_Explosion";
        public static readonly string Play_Vile_Ride_Armor_FootStep = "Play_Vile_Ride_Armor_FootStep_2";
        public static readonly string Play_Vile_Ride_Armor_In_SFX = "Play_Vile_Ride_Armor_In";
        public static readonly string Play_Vile_RideArmor_In_VSFX = "Play_Vile_RideArmor_In";
        public static readonly string Play_Vile_Ride_Armor_Lose = "Play_Vile_Ride_Armor_Lose";
        public static readonly string Play_Vile_Ride_Armor_Out = "Play_Vile_Ride_Armor_Out";
        public static readonly string Play_Vile_Simple_Bullet = "Play_Vile_Simple_Bullet";
        public static readonly string Play_Vile_SFX_Recov_HP = "Play_Vile_SFX_Recov_HP";
        public static readonly string Play_Vile_SFX_Shock = "Play_Vile_SFX_Shock";
        public static readonly string Play_Vile_Spawn = "Play_Vile_Spawn";
        public static readonly string Play_Vile_TP_In = "Play_Vile_TP_In";
        public static readonly string Play_Vile_TP_Out = "Play_Vile_TP_Out";
        public static readonly string Play_Vile_Take_This = "Play_Vile_Take_This";


    }
}