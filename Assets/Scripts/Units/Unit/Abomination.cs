using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abomination : UnitBase
{
    protected override void Update()
    {
        base.Update();
    }
    public override void UnitInit(int _num)
    {
        GameManager.Instance.Synergy_manager.Increase_Unitcount(UnitType.Zombie);
        base.UnitInit(_num);
        InvokeRepeating("SpecialAbility", 0.5f, 0.5f);
    }
    protected override void EventInit()
    {
        GameManager.Instance.Synergy_manager.Add_UnitSynergy_atkspd_list(Apple_SynergyOption); //시너지 이벤트함수적용
        GameManager.Instance.Synergy_manager.Add_ZombieSynergy_list(ZombieSynergyApply); // 좀비시너지 이펙트 이벤트적용
        ArtifactManager.Instance.AddApply_list(ApplyOption); //유물 이벤트함수 적용

        ApplyOption();
        ZombieSynergyApply(GameManager.Instance.Synergy_manager.zombie_synergy_apply);
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
        GameManager.Instance.Synergy_manager.Remove_ZombieSynergy_list(ZombieSynergyApply);
        base.UnitDelete();
    }
    void Effect_play()
    {
        _effect.ZombieEffectPlay();
    }
    void AttackTypeChange(bool _synergy)
    {
        AttackEvent.RemoveAllListeners();
        if (_synergy)
        {
            AttackEvent.AddListener(Zombie_Attack_monster);
        }
        else
        {
            AttackEvent.AddListener(AttackMonster);
        }
    }
    protected override void Apply_ArtifactOption()
    {
        int add_type_damage = ArtifactManager.Instance.have_Artifact[3] ? 5 : 0;
        float inc_attacktime = ArtifactManager.Instance.have_Artifact[15] ? 0.5f : 0f;

        damage = BaseData.base_attackpower + add_type_damage;
        temp_cooltime = BaseData.base_attackcooltime + inc_attacktime;

        Debug.Log("abomination | " + add_type_damage + " | " + inc_attacktime + "|" + temp_cooltime);
    }
    void Zombie_Attack_monster(int _index)
    {
        monsters[_index].Hit_to_Zombie(damage, BaseData.attacksound_number);
        monsters[_index].stun(BaseData.stuntime);
    }
    protected override void SpecialAbility()
    {
        int SA_damage = ArtifactManager.Instance.have_Artifact[15] ? damage * 2 : damage;
        if (monsters.Count != 0) // 몬스터가 1마리 이상일때
        {
            for (int i = 0; i < monsters.Count; i++)
            {
                monsters[i].Hit_to_normal(SA_damage, BaseData.type, 3);
            }
        }
    }
}
