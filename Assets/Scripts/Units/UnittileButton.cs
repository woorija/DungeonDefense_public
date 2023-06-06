using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnittileButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Unittile _unittile;
    int unit_num;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (UnitDrag.Instance.isDrag)
            {
                UnitDrag.Instance.DragEnd();
                if (!_unittile.unit_check)
                {
                    int num = UnitDrag.Instance.unit_num;
                    _unittile.Create_Unit(num);
                    unit_num = num;
                }
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            DecreaseUnitcount(unit_num);
            GetMana(unit_num);
            _unittile.Delete_Unit();
            //유닛 제거하고 소량의 마나로 변환
        }
    }
    void DecreaseUnitcount(int _num)
    {
        switch (_num % 4)
        {
            case 0:
                GameManager.Instance.Synergy_manager.Decrease_Unitcount(UnitType.Skeleton);
                break;
            case 1:
                GameManager.Instance.Synergy_manager.Decrease_Unitcount(UnitType.Ghost);
                break;
            case 2:
                GameManager.Instance.Synergy_manager.Decrease_Unitcount(UnitType.Vampire);
                break;
            case 3:
                GameManager.Instance.Synergy_manager.Decrease_Unitcount(UnitType.Zombie);
                break;
        }
    }
    void GetMana(int _num)
    {
        switch(_num / 4)
        {
            case 0:
                GameManager.Instance.Money_manager.Get_mana(4);
                break;
            case 1:
                GameManager.Instance.Money_manager.Get_mana(12);
                break;
            case 2:
                GameManager.Instance.Money_manager.Get_mana(36);
                break;
        }
    }
}
