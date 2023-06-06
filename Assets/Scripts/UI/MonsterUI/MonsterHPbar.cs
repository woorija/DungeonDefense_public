using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPbar : MonoBehaviour
{
    public Slider hpbar;
    [SerializeField] Canvas canvas;

    int ypos;
    public void UIInit(float _hp)
    {
        canvas.worldCamera = Camera.main;
        hpbar.maxValue = _hp;
        hpbar.value = _hp;
    }

    public void UIUpdate(float _hp)
    {
        hpbar.value = _hp;
    }
    public void UIPosUpdata(Vector3 _pos)
    {
        hpbar.transform.position = _pos + new Vector3(0, ypos);
    }

    public void SetOnOff(bool _on)
    {
        hpbar.gameObject.SetActive(_on);
    }
    public void ypos_Init(int _monstertype)
    {
        switch (_monstertype)
        {
            case 11: //클레오 잡몹
            case 12:
            case 13:
                ypos = 150;
                break;
            case 21: // 카드 잡몹
            case 22:
            case 23:
                ypos = 100;
                break;
            case 31: // 셰어 잡몹
            case 32:
            case 33:
                ypos = 135;
                break;
            case 994: // 길가
                ypos = 185;
                break;
            case 14: //클레오
                ypos = 185;
                break;
            case 24: // 카드
                ypos = 185;
                break;
            case 34: // 셰어
                ypos = 185;
                break;
        }
    }
}
