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
        GameManager.Instance.synergyManager.IncreaseUnitcount(UnitType.Ghost);
        base.UnitInit(_num);
        AttackEvent.AddListener(AttackMonster);
    }
    protected override void ApplyArtifactOption()
    {
        int addTypeDamage = ArtifactManager.Instance.hasArtifacts[1] ? 5 : 0;
        int addUnitDamage = ArtifactManager.Instance.hasArtifacts[8] ? 5 : 0;
        float addUnitRange = ArtifactManager.Instance.hasArtifacts[8] ? 0.5f : 0f;

        damage = BaseData.baseAttackPower + addTypeDamage + addUnitDamage;
        range.RangeInit(BaseData.baseRange + addUnitRange);
        tempCooltime = BaseData.baseAttackCooltime;
    }
}
