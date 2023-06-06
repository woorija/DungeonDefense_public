using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitattackRange : MonoBehaviour
{
    [SerializeField] BoxCollider2D _range;

    public void RangeInit(float _size)
    {
        float size = (_size * 240 + 120) * 1;
        _range.size = new Vector2(size, size);
    }
}
