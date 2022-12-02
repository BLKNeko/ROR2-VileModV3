using VileMod.SkillStates;
using VileMod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;

namespace VileMod.Modules
{
    public static class States
    {
        internal static void RegisterStates()
        {
            Modules.Content.AddEntityState(typeof(BaseMeleeAttack));
            Modules.Content.AddEntityState(typeof(SlashCombo));

            Modules.Content.AddEntityState(typeof(Shoot));

            Modules.Content.AddEntityState(typeof(Roll));

            Modules.Content.AddEntityState(typeof(ThrowBomb));

            Modules.Content.AddEntityState(typeof(CherryBlast));
            Modules.Content.AddEntityState(typeof(BumpityBoom));
            Modules.Content.AddEntityState(typeof(BumpityBoom2));
            Modules.Content.AddEntityState(typeof(FrontRunner));
            Modules.Content.AddEntityState(typeof(CerberusPhantom));
            Modules.Content.AddEntityState(typeof(BurningDrive));
            Modules.Content.AddEntityState(typeof(EletricSpark));
            Modules.Content.AddEntityState(typeof(ShotgunIce));
            Modules.Content.AddEntityState(typeof(Fury));
            Modules.Content.AddEntityState(typeof(DeathState));



        }
    }
}