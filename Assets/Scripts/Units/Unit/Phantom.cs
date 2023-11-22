using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phantom : UnitBase
{
    int bonusDamage;
    protected override void Update()
    {
        base.Update();
    }

    public override void UnitInit(int _num)
    {
        GameManager.Instance.synergyManager.IncreaseUnitcount(UnitType.Ghost);
        base.UnitInit(_num);
        AttackEvent.AddListener(AttackMonster);
        GameManager.Instance.abilityManager.OnGhostTypeKilled += SpecialAbility;
    }
    public override void UnitDelete()
    {
        GameManager.Instance.abilityManager.OnGhostTypeKilled -= SpecialAbility;
        base.UnitDelete();
    }

    protected override void ApplyArtifactOption()
    {
        int addTypeDamage = ArtifactManager.Instance.hasArtifacts[1] ? 5 : 0;
        bonusDamage = ArtifactManager.Instance.hasArtifacts[9] ? 2 : 1;

        damage = BaseData.baseAttackPower + addTypeDamage;
        tempCooltime = BaseData.baseAttackCooltime;
    }
    protected override void SpecialAbility()
    {
        damage += bonusDamage;
    }
}
