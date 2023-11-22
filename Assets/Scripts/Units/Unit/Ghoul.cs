using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghoul : UnitBase
{
    protected override void Update()
    {
        base.Update();
    }

    public override void UnitInit(int _num)
    {
        GameManager.Instance.synergyManager.IncreaseUnitcount(UnitType.Zombie);
        base.UnitInit(_num);
    }
    protected override void EventInit()
    {
        GameManager.Instance.synergyManager.OnUnitAtkspdSynergyApply += ApplySynergyOption;
        GameManager.Instance.synergyManager.OnZombieSynergyApply += ZombieSynergyApply;
        ArtifactManager.Instance.onGetArtifact += ApplyOption;

        ApplyOption();
        ZombieSynergyApply(GameManager.Instance.synergyManager.isZombieSynergyApply);
    }
    void ZombieSynergyApply(bool _apply)
    {
        if (_apply)
        {
            Effect_play();
        }
        AttackTypeChange(_apply);
    }
    public override void UnitDelete()
    {
        GameManager.Instance.synergyManager.OnZombieSynergyApply -= ZombieSynergyApply;
        base.UnitDelete();
    }
    void Effect_play()
    {
        effect.ZombieEffectPlay();
    }
    void AttackTypeChange(bool _synergy)
    {
        AttackEvent.RemoveAllListeners();
        if (_synergy)
        {
            AttackEvent.AddListener(ZombieTypeAttack);
        }
        else
        {
            AttackEvent.AddListener(AttackMonster);
        }
    }
    protected override void ApplyArtifactOption()
    {
        int add_type_damage = ArtifactManager.Instance.hasArtifacts[3] ? 5 : 0;
        int add_unit_damage = ArtifactManager.Instance.hasArtifacts[14] ? 10 : 0;
        float inc_attacktime = ArtifactManager.Instance.hasArtifacts[14] ? 0.5f : 0f;

        damage = BaseData.baseAttackPower + add_type_damage + add_unit_damage;
        tempCooltime = BaseData.baseAttackCooltime + inc_attacktime;
    }
    void ZombieTypeAttack(int _index)
    {
        monsters[_index].HitToZombie(damage, BaseData.attackSoundNumber);
        monsters[_index].Stun(BaseData.stunTime);
    }
}
