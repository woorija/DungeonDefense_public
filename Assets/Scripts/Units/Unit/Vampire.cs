using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : UnitBase
{
    protected override void Update()
    {
        base.Update();
    }

    public override void UnitInit(int _num)
    {
        GameManager.Instance.Synergy_manager.Increase_Unitcount(UnitType.Vampire);
        base.UnitInit(_num);
        AttackEvent.AddListener(AttackMonster);
    }

    protected override void Apply_ArtifactOption()
    {
        int add_type_damage = ArtifactManager.Instance.have_Artifact[2] ? 3 : 0;
        int add_unit_damage = ArtifactManager.Instance.have_Artifact[11] ? 10 : 0;

        damage = BaseData.base_attackpower + add_type_damage + add_unit_damage;
        temp_cooltime = BaseData.base_attackcooltime;
        Debug.Log("vampire | " + add_type_damage + " | " + add_unit_damage + "|" + temp_cooltime);
    }
}
