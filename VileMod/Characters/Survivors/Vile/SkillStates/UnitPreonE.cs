using EntityStates;
using RoR2;
using UnityEngine;
using VileMod.Survivors.Vile;

public class UnitPreonE : BaseSkillState
{
    // Defina isso na inicialização do mod
    public static GameObject turretMasterPrefab;

    public override void OnEnter()
    {
        base.OnEnter();

        turretMasterPrefab = VileAssets.unitPreonEPrefabTest;

        if (isAuthority)
        {
            SummonTurret();
        }

        outer.SetNextStateToMain();
    }

    void SummonTurret()
    {
        //MasterSummon summon = new MasterSummon
        //{
        //    masterPrefab = turretMasterPrefab,
        //    position = base.transform.position + base.GetAimRay().direction * 3f,
        //    rotation = Quaternion.identity,
        //    summonerBodyObject = base.gameObject,
        //    ignoreTeamMemberLimit = true,
        //    teamIndexOverride = TeamComponent.GetObjectTeam(base.gameObject)
        //};
        //summon.Perform();

        //GameObject.Instantiate(VileAssets.unitPreonEPrefabTest, characterBody.transform.position, Quaternion.identity);

    }
}
