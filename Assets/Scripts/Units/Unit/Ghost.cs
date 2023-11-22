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
        GameManager.Instance.synergyManager.IncreaseUnitcount(UnitType.Ghost);
        base.UnitInit(_num);
        AttackEvent.AddListener(AttackMonster);
    }

    protected override void ApplyArtifactOption()
    {
        int addTypeDamage = ArtifactManager.Instance.hasArtifacts[1] ? 5 : 0;
        float increaseAttackTime = ArtifactManager.Instance.hasArtifacts[7] ? 0.5f : 0f;

        damage = BaseData.baseAttackPower + addTypeDamage;
        tempCooltime = BaseData.baseAttackCooltime + increaseAttackTime;
    }
}
