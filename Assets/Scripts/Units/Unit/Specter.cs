using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Specter : UnitBase
{
    protected override void Update()
    {
        base.Update();
    }
    public override void UnitInit(int _num)
    {
        GameManager.Instance.Synergy_manager.Increase_Unitcount(UnitType.Ghost);
        base.UnitInit(_num);
        AttackEvent.AddListener(AttackMonster);
    }
    protected override void Apply_ArtifactOption()
    {
        int add_type_damage = ArtifactManager.Instance.have_Artifact[1] ? 5 : 0;
        int add_unit_damage = ArtifactManager.Instance.have_Artifact[8] ? 5 : 0;
        float add_unit_range = ArtifactManager.Instance.have_Artifact[8] ? 0.5f : 0f;

        damage = BaseData.base_attackpower + add_type_damage + add_unit_damage;
        _range.RangeInit(BaseData.base_range + add_unit_range);
        temp_cooltime = BaseData.base_attackcooltime;

        Debug.Log("spector | " + add_type_damage + " | " + add_unit_damage + "|" + add_unit_range + "|" + temp_cooltime);
    }
}
