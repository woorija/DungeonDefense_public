using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    [SerializeField]
    GameObject[] Effects;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject EffectPlay(int _num, Transform _obj, int _ypos)
    {
        GameObject temp_effect = Instantiate(Effects[_num], _obj.position + new Vector3(0, _ypos, 0), Quaternion.identity, _obj);
        return temp_effect;
    }
}
