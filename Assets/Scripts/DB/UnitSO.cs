using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObjects/UnitData", order = 1)]
public class UnitSO : ScriptableObject
{
    [field:SerializeField] public string path { get; private set; }
    [field: SerializeField] public int type { get; private set; }
    [field: SerializeField] public int baseAttackPower { get; private set; }
    [field: SerializeField] public float baseAttackCooltime { get; private set; }
    [field: SerializeField] public float baseRange { get; private set; }
    [field: SerializeField] public int attackSoundNumber { get; private set; }
    [field: SerializeField] public float stunTime { get; private set; }
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
        baseAttackPower = _power;
    }
    public void SetCooltime(float _cooltime)
    {
        baseAttackCooltime= _cooltime;
    }
    public void SetRange(float _range)
    {
        baseRange= _range;
    }
    public void SetSound(int _sound)
    {
        attackSoundNumber = _sound;
    }
    public void SetStuntime(float _stun)
    {
        stunTime = _stun;
    }
}
