using System;
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
    public int[] synergyLevels { get; private set; } = new int[4];
    public int[] unitCounts { get; private set; } = new int[4];

    public float reduceMonsterDef { get; private set; } // 시너지옵션 : 적군 방어력 감소
    public float reduceMonsterSpeed { get; private set; } // 시너지옵션 : 적군 이동속도 감소
    public float increaseUnitAtkspd { get; private set; } // 시너지 옵션 : 아군 공격속도 증가
    public float zombieExplosionDamage { get; private set; } // 시너지 옵션 : 좀비 폭발뎀
    public bool isZombieSynergyApply { get; private set; } // 좀비 시너지 적용 여부

    //시너지 레벨당 옵션 
    float[] reduceMonsterDefLevel = {0f, 0.1f, 0.25f, 0.4f }; 
    float[] reduceMonsterSpeedLevel = {0f, 0.1f, 0.2f, 0.3f };
    float[] increaseUnitAtkspdLevel = {1f, 1.1f, 1.3f, 1.5f };
    float[] zombieExplosionDamageLevel = {0f, 0.1f, 0.15f, 0.2f };

    public Action<float> OnUnitAtkspdSynergyApply;
    public Action<float> OnMonsterSpeedSynergyApply;
    public Action<float> OnMonsterDefSynergyApply;
    public Action<bool> OnZombieSynergyApply;
    public void Init() //매니저 초기화
    {
        Reset();
    }

    public void Reset() // 시너지 레벨,옵션  초기화
    {
        for (int i = 0; i < 4; i++)
        {
            synergyLevels[i] = 0;
            unitCounts[i] = 0;
        }
        reduceMonsterDef = reduceMonsterDefLevel[0];
        reduceMonsterSpeed = reduceMonsterSpeedLevel[0];
        increaseUnitAtkspd = increaseUnitAtkspdLevel[0];
        zombieExplosionDamage = zombieExplosionDamageLevel[0];
    }

    public void IncreaseUnitcount(UnitType _type) // 일정 유닛 소환마다 시너지 렙 증가
    {
        unitCounts[(int)_type]++;
        switch (unitCounts[(int)_type]) // 3마리 1렙 6마리 2렙 9마리 3렙
        {
            case 3:
                SynergyApply(_type, 1);
                break;
            case 6:
                SynergyApply(_type, 2);
                break;
            case 9:
                SynergyApply(_type, 3);
                break;
        }
    }
    public void DecreaseUnitcount(UnitType _type)
    {
        unitCounts[(int)_type]--;
        switch (unitCounts[(int)_type])
        {
            case 2:
                SynergyApply(_type, 0);
                break;
            case 5:
                SynergyApply(_type, 1);
                break;
            case 8:
                SynergyApply(_type, 2);
                break;
        }
    }

    public void SynergyApply(UnitType _type, int _level) // 유닛에 시너지 적용
    {
        synergyLevels[(int)_type] = _level;
        switch (_type) // 유닛타입마다
        {
            case UnitType.Ghost:
                reduceMonsterSpeed = reduceMonsterSpeedLevel[_level];
                OnMonsterSpeedSynergyApply?.Invoke(reduceMonsterSpeed);
                break;
            case UnitType.Skeleton:
                increaseUnitAtkspd = increaseUnitAtkspdLevel[_level];
                OnUnitAtkspdSynergyApply?.Invoke(increaseUnitAtkspd);
                break;
            case UnitType.Vampire:
                reduceMonsterDef = reduceMonsterDefLevel[_level];
                OnMonsterDefSynergyApply?.Invoke(reduceMonsterDef);
                break;
            case UnitType.Zombie:
                zombieExplosionDamage= zombieExplosionDamageLevel[_level];
                isZombieSynergyApply = _level != 0;
                OnZombieSynergyApply?.Invoke(isZombieSynergyApply);
                break;
        }
    }
}
