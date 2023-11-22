using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : UnitBase
{
    protected override void Update()
    {
        base.Update();
    }

    public override void UnitInit(int _num)
    {
        GameManager.Instance.synergyManager.IncreaseUnitcount(UnitType.Skeleton);
        base.UnitInit(_num);
        AttackEvent.AddListener(AttackMonster);
    }

    protected override void ApplyArtifactOption()
    {
        int addTypeDamage = ArtifactManager.Instance.hasArtifacts[0] ? 5 : 0;
        int addUnitDamage = ArtifactManager.Instance.hasArtifacts[4] ? 10 : 0;

        damage = BaseData.baseAttackPower + addTypeDamage + addUnitDamage;
        tempCooltime = BaseData.baseAttackCooltime;
    }
}
