using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathKnight : UnitBase
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
        UnitDrag.Instance.DK_SpecialAbility();
    }
    protected override void ApplyArtifactOption()
    {
        int addTypeDamage = ArtifactManager.Instance.hasArtifacts[0] ? 5 : 0;
        int addUnitDamage = ArtifactManager.Instance.hasArtifacts[6] ? 30 : 0;
        float addUnitRange = ArtifactManager.Instance.hasArtifacts[6] ? 1f : 0f;

        damage = BaseData.baseAttackPower + addTypeDamage + addUnitDamage;
        range.RangeInit(BaseData.baseRange + addUnitRange);
        tempCooltime = BaseData.baseAttackCooltime;
    }
}
