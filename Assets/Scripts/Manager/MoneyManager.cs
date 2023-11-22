
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour //재화관리 매니저
{
    public float mana { get; private set; }
    public int maxMana { get; private set; }
    public int[] unitCounts { get; private set; } // 뽑을수 있는 유닛 수

    [SerializeField] ManaUI manaUI;
    [SerializeField] UnitUI unitUI;

    public void Reset()
    {
        UnitCountInit();
        ManaInit();
    }
    public void Init()
    {
        unitCounts = new int[12];
        Reset();
    }

    void UnitCountInit()
    {
        for(int i = 0; i < unitCounts.Length; i++)
        {
            unitCounts[i] = 0;
            unitUI.Lock(i);
            unitUI.SetText(i, unitCounts[i]);
        }
    }

    public void AddUnitcount(int _num)
    {
        if (unitCounts[_num] == 0)
        {
            unitUI.Unlock(_num);
        }
        unitCounts[_num]++;
        unitUI.SetText(_num, unitCounts[_num]);
    }
    public void SubUnitcount(int _num,int _value)
    {
        unitCounts[_num] = Mathf.Max(unitCounts[_num] - _value, 0);
        unitUI.SetText(_num, unitCounts[_num]);
        if (unitCounts[_num] == 0)
        {
            unitUI.Lock(_num);
        }
    }
    public int GetUnitcount(int _num)
    {
        return unitCounts[_num];
    }
    void ManaInit()
    {
        maxMana = 20;
        mana = 0;
        manaUI.TextUpdate(mana, maxMana);
    }
    public void HasManaArtifact()
    {
        maxMana = ArtifactManager.Instance.hasArtifacts[17] ? 40 : 20;
        manaUI.TextUpdate(mana, maxMana);
    }
    public void GetMana(float _get) //마나 획득
    {
        mana = Mathf.Clamp(mana + _get, 0f, maxMana);
        manaUI.TextUpdate(mana, maxMana);
    }
    public bool UseMana(int _value) //마나 사용
    {
        if(mana >= _value)
        {
            GetMana(-_value);
            return true;
        }
        return false;
    }
}
