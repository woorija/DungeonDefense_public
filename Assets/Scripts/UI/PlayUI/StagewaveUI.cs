using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StagewaveUI : MonoBehaviour
{
    [SerializeField] Text text;

    public void Set_StageText(int _stage,int _wave)
    {
        text.text = string.Format("Stage {0}\nWave {1}", _stage, _wave);
    }
}
