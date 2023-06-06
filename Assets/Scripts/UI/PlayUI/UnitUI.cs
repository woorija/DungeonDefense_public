using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
    [SerializeField] GameObject[] unitcovers;
    [SerializeField] Text[] unitcount;
    public void Lock(int _num)
    {
        unitcovers[_num].SetActive(true);
        unitcount[_num].text = null;
    }
    public void Unlock(int _num)
    {
        unitcovers[_num].SetActive(false);
    }
    public void SetText(int _num, int _value)
    {
        unitcount[_num].text = _value.ToString();
    }
}
