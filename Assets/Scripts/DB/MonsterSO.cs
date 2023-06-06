using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObjects/MonsterData", order = 2)]
public class MonsterSO : ScriptableObject
{
    [field: SerializeField] public int base_hp { get; private set; }
    [field: SerializeField] public float base_speed { get; private set; }
    [field: SerializeField] public int base_def { get; private set; }
    public void SetHp(int _hp)
    {
        base_hp = _hp;
    }
    public void SetSpeed(float _speed)
    {
        base_speed = _speed;
    }
    public void SetDef(int _def)
    {
        base_def = _def;
    }
}
