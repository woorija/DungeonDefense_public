using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum UnitType
{
    Skeleton,
    Ghost,
    Vampire,
    Zombie
}

public class SynergyManager : MonoBehaviour
{
    public int[] SynergyLevel { get; private set; } = new int[4];
    public int[] Unitcount { get; private set; } = new int[4];

    public float monster_reduce_def { get; private set; } // 시너지옵션 : 적군 방어력 감소
    public float monster_reduce_speed { get; private set; } // 시너지옵션 : 적군 이동속도 감소
    public float unit_increase_atkspd { get; private set; } // 시너지 옵션 : 아군 공격속도 증가
    public float zombie_explosion_damage { get; private set; } // 시너지 옵션 : 좀비 폭발뎀
    public bool zombie_synergy_apply { get; private set; } // 좀비 시너지 적용 여부

    //시너지 레벨당 옵션 
    float[] monster_reduce_def_level = {0f, 0.1f, 0.25f, 0.4f }; 
    float[] monster_reduce_speed_level = {0f, 0.1f, 0.2f, 0.3f };
    float[] unit_increase_atkspd_level = {1f, 1.1f, 1.3f, 1.5f };
    float[] zombie_explosion_damage_level = {0f, 0.1f, 0.15f, 0.2f };

    //아군,적군 유닛 일괄적용 이벤트
    UnityEvent<float> E_synergy_Apply_unit_atkspd;
    UnityEvent<float> E_synergy_Apply_monster_speed;
    UnityEvent<float> E_synergy_Apply_monster_def;
    UnityEvent<bool> E_synergy_Apply_zombie_synergy;
    public void Init() //매니저 초기화
    {
        Reset();
        E_synergy_Apply_unit_atkspd = new UnityEvent<float>();
        E_synergy_Apply_monster_speed = new UnityEvent<float>();
        E_synergy_Apply_monster_def = new UnityEvent<float>();
        E_synergy_Apply_zombie_synergy = new UnityEvent<bool>();
    }

    public void Reset() // 시너지 레벨,옵션  초기화
    {
        for (int i = 0; i < 4; i++)
        {
            SynergyLevel[i] = 0;
            Unitcount[i] = 0;
        }

        monster_reduce_def = monster_reduce_def_level[0];
        monster_reduce_speed = monster_reduce_speed_level[0];
        unit_increase_atkspd = unit_increase_atkspd_level[0];
        zombie_explosion_damage = zombie_explosion_damage_level[0];
    }

    public void Increase_Unitcount(UnitType _type) // 일정 유닛 소환마다 시너지 렙 증가
    {
        Unitcount[(int)_type]++;
        switch (Unitcount[(int)_type]) // 3마리 1렙 6마리 2렙 9마리 3렙
        {
            case 3:
                Synergy_apply(_type, 1);
                break;
            case 6:
                Synergy_apply(_type, 2);
                break;
            case 9:
                Synergy_apply(_type, 3);
                break;
        }
    }
    public void Decrease_Unitcount(UnitType _type)
    {
        Unitcount[(int)_type]--;
        switch (Unitcount[(int)_type])
        {
            case 2:
                Synergy_apply(_type, 0);
                break;
            case 5:
                Synergy_apply(_type, 1);
                break;
            case 8:
                Synergy_apply(_type, 2);
                break;
        }
    }

    public void Synergy_apply(UnitType _type, int _level) // 유닛에 시너지 적용
    {
        SynergyLevel[(int)_type] = _level;
        switch (_type) // 유닛타입마다
        {
            case UnitType.Ghost:
                monster_reduce_speed = monster_reduce_speed_level[_level];
                E_synergy_Apply_monster_speed.Invoke(monster_reduce_speed);
                break;
            case UnitType.Skeleton:
                unit_increase_atkspd = unit_increase_atkspd_level[_level];
                E_synergy_Apply_unit_atkspd.Invoke(unit_increase_atkspd);
                break;
            case UnitType.Vampire:
                monster_reduce_def = monster_reduce_def_level[_level];
                E_synergy_Apply_monster_def.Invoke(monster_reduce_def);
                break;
            case UnitType.Zombie:
                zombie_explosion_damage = zombie_explosion_damage_level[_level];
                zombie_synergy_apply = _level != 0;
                E_synergy_Apply_zombie_synergy.Invoke(zombie_synergy_apply);
                break;
        }
    }
    #region 이벤트 적용 함수
    public void Add_UnitSynergy_atkspd_list(UnityAction<float> _function)
    {
        E_synergy_Apply_unit_atkspd.AddListener(_function);
    }

    public void Remove_UnitSynergy_atkspd_list(UnityAction<float> _function)
    {
        E_synergy_Apply_unit_atkspd.RemoveListener(_function);
    }

    public void Add_MonsterSynergy_speed_list(UnityAction<float> _function)
    {
        E_synergy_Apply_monster_speed.AddListener(_function);
    }

    public void Remove_MonsterSynergy_speed_list(UnityAction<float> _function)
    {
        E_synergy_Apply_monster_speed.RemoveListener(_function);
    }

    public void Add_MonsterSynergy_def_list(UnityAction<float> _function)
    {
        E_synergy_Apply_monster_def.AddListener(_function);
    }

    public void Remove_MonsterSynergy_def_list(UnityAction<float> _function)
    {
        E_synergy_Apply_monster_def.RemoveListener(_function);
    }

    public void Add_ZombieSynergy_list(UnityAction<bool> _function) 
    {
        E_synergy_Apply_zombie_synergy.AddListener(_function);
    }

    public void Remove_ZombieSynergy_list(UnityAction<bool> _function)
    {
        E_synergy_Apply_zombie_synergy.RemoveListener(_function);
    }
    #endregion
}
