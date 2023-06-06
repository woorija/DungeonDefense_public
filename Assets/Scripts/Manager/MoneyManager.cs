
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour //재화관리 매니저
{
    public float mana { get; private set; }
    public int max_mana { get; private set; }
    public int[] unit_count { get; private set; } // 뽑을수 있는 유닛 수

    [SerializeField] ManaUI manaUI;
    [SerializeField] UnitUI unitUI;

    public void Reset()
    {
        Unitcount_Init();
        ManaInit();
    }
    public void Init()
    {
        unit_count = new int[12];
        Reset();
    }

    void Unitcount_Init()
    {
        for(int i = 0; i < unit_count.Length; i++)
        {
            unit_count[i] = 0;
            unitUI.Lock(i);
            //unit_count[i] = 15;
            //unitUI.Unlock(i);
            unitUI.SetText(i, unit_count[i]);
        }
    }

    public void Add_Unitcount(int _num)
    {
        if (unit_count[_num] == 0)
        {
            unitUI.Unlock(_num);
        }
        unit_count[_num]++;
        unitUI.SetText(_num, unit_count[_num]);
    }
    public void Sub_Unitcount(int _num,int _value)
    {
        unit_count[_num] = Mathf.Max(unit_count[_num] - _value, 0);
        unitUI.SetText(_num, unit_count[_num]);
        if (unit_count[_num] == 0)
        {
            unitUI.Lock(_num);
        }
    }
    public int Get_Unitcount(int _num)
    {
        return unit_count[_num];
    }
    void ManaInit()
    {
        max_mana = 20;
        mana = 0;
        manaUI.TextUpdate(mana, max_mana);
    }
    public void HaveManaArtifact()
    {
        max_mana = ArtifactManager.Instance.have_Artifact[17] ? 40 : 20;
        manaUI.TextUpdate(mana, max_mana);
    }
    public void Get_mana(float _get) //마나 획득
    {
        mana = Mathf.Clamp(mana + _get, 0f, max_mana);
        manaUI.TextUpdate(mana, max_mana);
    }
    public bool UseMana(int _value) //마나 사용
    {
        if(mana >= _value)
        {
            Get_mana(-_value);
            return true;
        }
        return false;
    }
}
