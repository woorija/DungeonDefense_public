using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObjects/MonsterData", order = 2)]
public class MonsterSO : ScriptableObject
{
    [field: SerializeField] public int baseHp { get; private set; }
    [field: SerializeField] public float baseSpeed { get; private set; }
    [field: SerializeField] public int baseDef { get; private set; }
    public void SetHp(int _hp)
    {
        baseHp = _hp;
    }
    public void SetSpeed(float _speed)
    {
        baseSpeed = _speed;
    }
    public void SetDef(int _def)
    {
        baseDef = _def;
    }
}
