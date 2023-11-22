using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireLord : UnitBase
{
    protected override void Update()
    {
        base.Update();
    }
    public override void UnitInit(int _num)
    {
        GameManager.Instance.synergyManager.IncreaseUnitcount(UnitType.Vampire);
        base.UnitInit(_num);
        AttackEvent.AddListener(AttackMonster);
        GameManager.Instance.abilityManager.OnVampireTypeKilled += SpecialAbility;
    }
    public override void UnitDelete()
    {
        GameManager.Instance.abilityManager.OnVampireTypeKilled -= SpecialAbility;
        base.UnitDelete();
    }
    protected override void ApplyArtifactOption()
    {
        int addTypeDamage = ArtifactManager.Instance.hasArtifacts[2] ? 3 : 0;
        int addUnitDamage = ArtifactManager.Instance.hasArtifacts[12] ? 20 : 0;

        damage = BaseData.baseAttackPower + addTypeDamage + addUnitDamage;
        tempCooltime = BaseData.baseAttackCooltime;
    }
    protected override void SpecialAbility()
    {
        PoolManager.Instance.GetBust(transform.position);
    }
}
