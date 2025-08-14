using MegamanXMod.Survivors.X.SkillStates;
using VileMod.Characters.Survivors.Vile.BuffSkillStates;
using VileMod.Survivors.Vile.SkillStates;

namespace VileMod.Survivors.Vile
{
    public static class VileStates
    {
        public static void Init()
        {

            Modules.Content.AddEntityState(typeof(CherryBlast));

            Modules.Content.AddEntityState(typeof(CherryBlastEnd));

            Modules.Content.AddEntityState(typeof(CherryBlastStart));

            Modules.Content.AddEntityState(typeof(CYDash));

            Modules.Content.AddEntityState(typeof(CYPunch0));

            Modules.Content.AddEntityState(typeof(CYPunch1));

            Modules.Content.AddEntityState(typeof(CYPunch2));

            Modules.Content.AddEntityState(typeof(CYPunchN));

            Modules.Content.AddEntityState(typeof(CYShot));

            Modules.Content.AddEntityState(typeof(DestroyRideArmor));

            Modules.Content.AddEntityState(typeof(DistanceNeedler));

            Modules.Content.AddEntityState(typeof(DistanceNeedlerEnd));

            Modules.Content.AddEntityState(typeof(DistanceNeedlerStart));

            Modules.Content.AddEntityState(typeof(EnterCyclops));

            Modules.Content.AddEntityState(typeof(EnterCyclopsEnd));

            Modules.Content.AddEntityState(typeof(EnterCyclopsEnd2));

            Modules.Content.AddEntityState(typeof(EnterGoliath));

            Modules.Content.AddEntityState(typeof(EnterGoliathEnd));

            Modules.Content.AddEntityState(typeof(EnterGoliathEnd2));

            Modules.Content.AddEntityState(typeof(EnterHawk));

            Modules.Content.AddEntityState(typeof(EnterHawkEnd));

            Modules.Content.AddEntityState(typeof(EnterHawkEnd2));

            Modules.Content.AddEntityState(typeof(ExitCyclops));

            Modules.Content.AddEntityState(typeof(ExitGoliath));

            Modules.Content.AddEntityState(typeof(ExitHawk));

            Modules.Content.AddEntityState(typeof(GDashPunch));

            Modules.Content.AddEntityState(typeof(GPunch0));

            Modules.Content.AddEntityState(typeof(GPunch1));

            Modules.Content.AddEntityState(typeof(GPunch2));

            Modules.Content.AddEntityState(typeof(GShot));

            Modules.Content.AddEntityState(typeof(HawkDash));

            Modules.Content.AddEntityState(typeof(HGun1));

            Modules.Content.AddEntityState(typeof(HGunBarrage));

            Modules.Content.AddEntityState(typeof(RepairRideArmor));

            Modules.Content.AddEntityState(typeof(ResumeCyclops));

            Modules.Content.AddEntityState(typeof(ResumeGoliath));

            Modules.Content.AddEntityState(typeof(ResumeHawk));

            Modules.Content.AddEntityState(typeof(Triple7));

            Modules.Content.AddEntityState(typeof(Triple7End));

            Modules.Content.AddEntityState(typeof(Triple7Start));

            Modules.Content.AddEntityState(typeof(UnitBigBit));

            Modules.Content.AddEntityState(typeof(UnitGunVolt));

            Modules.Content.AddEntityState(typeof(UnitMameQ));

            Modules.Content.AddEntityState(typeof(UnitMettaurCommander));

            Modules.Content.AddEntityState(typeof(UnitMettaurcure));

            Modules.Content.AddEntityState(typeof(UnitNightmareV));

            Modules.Content.AddEntityState(typeof(UnitPreonE));

            Modules.Content.AddEntityState(typeof(UnitSpiky));

            Modules.Content.AddEntityState(typeof(UnitTogerics));

            Modules.Content.AddEntityState(typeof(VBumpityBoom));

            Modules.Content.AddEntityState(typeof(VBurningDrive));

            Modules.Content.AddEntityState(typeof(VCerberusPhanton));

            Modules.Content.AddEntityState(typeof(VEletricSpark));

            Modules.Content.AddEntityState(typeof(VFlameRound));

            Modules.Content.AddEntityState(typeof(VFrontRunner));

            Modules.Content.AddEntityState(typeof(VNapalmBomb));

            Modules.Content.AddEntityState(typeof(VSDR));

            Modules.Content.AddEntityState(typeof(VShotgunIce));

            Modules.Content.AddEntityState(typeof(ZipZapper));

            Modules.Content.AddEntityState(typeof(ZipZapperEnd));

            Modules.Content.AddEntityState(typeof(ZipZapperStart));




            Modules.Content.AddEntityState(typeof(VileAddTempOverHeatDebuff));

            Modules.Content.AddEntityState(typeof(VileAddFlamePrimaryBuff));
            Modules.Content.AddEntityState(typeof(VileAddFrostPrimaryBuff));
            Modules.Content.AddEntityState(typeof(VileAddShockPrimaryBuff));

            Modules.Content.AddEntityState(typeof(VileRemFlamePrimaryBuff));
            Modules.Content.AddEntityState(typeof(VileRemFrostPrimaryBuff));
            Modules.Content.AddEntityState(typeof(VileRemShockPrimaryBuff));




        }
    }
}
