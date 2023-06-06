using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : UnitBase
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
        float inc_attacktime = ArtifactManager.Instance.have_Artifact[7] ? 0.5f : 0f;

        damage = BaseData.base_attackpower + add_type_damage;
        temp_cooltime = BaseData.base_attackcooltime + inc_attacktime;
    }
}
