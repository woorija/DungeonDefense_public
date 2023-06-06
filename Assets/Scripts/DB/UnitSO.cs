using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObjects/UnitData", order = 1)]
public class UnitSO : ScriptableObject
{
    [field:SerializeField] public string path { get; private set; }
    [field: SerializeField] public int type { get; private set; }
    [field: SerializeField] public int base_attackpower { get; private set; }
    [field: SerializeField] public float base_attackcooltime { get; private set; }
    [field: SerializeField] public float base_range { get; private set; }
    [field: SerializeField] public int attacksound_number { get; private set; }
    [field: SerializeField] public float stuntime { get; private set; }
    public void SetPath(string _path)
    {
        path = _path;
    }
    public void SetType(int _type)
    {
        type = _type;
    }
    public void SetPower(int _power)
    {
        base_attackpower = _power;
    }
    public void SetCooltime(float _cooltime)
    {
        base_attackcooltime= _cooltime;
    }
    public void SetRange(float _range)
    {
        base_range= _range;
    }
    public void SetSound(int _sound)
    {
        attacksound_number = _sound;
    }
    public void SetStuntime(float _stun)
    {
        stuntime = _stun;
    }
}
