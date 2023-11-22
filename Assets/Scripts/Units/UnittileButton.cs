using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitTileButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] UnitTile unitTile;
    int unitNum;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (UnitDrag.Instance.isDrag)
            {
                UnitDrag.Instance.DragEnd();
                if (!unitTile.unitCheck)
                {
                    int num = UnitDrag.Instance.unit_num;
                    unitTile.CreateUnit(num);
                    unitNum = num;
                }
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (unitTile.unitCheck)
            {
                DecreaseUnitcount(unitNum);
                GetMana(unitNum);
                unitTile.DeleteUnit();
                //유닛 제거하고 소량의 마나로 변환
            }
        }
    }
    void DecreaseUnitcount(int _num)
    {
        switch (_num % 4)
        {
            case 0:
                GameManager.Instance.synergyManager.DecreaseUnitcount(UnitType.Skeleton);
                break;
            case 1:
                GameManager.Instance.synergyManager.DecreaseUnitcount(UnitType.Ghost);
                break;
            case 2:
                GameManager.Instance.synergyManager.DecreaseUnitcount(UnitType.Vampire);
                break;
            case 3:
                GameManager.Instance.synergyManager.DecreaseUnitcount(UnitType.Zombie);
                break;
        }
    }
    void GetMana(int _num)
    {
        switch(_num / 4)
        {
            case 0:
                GameManager.Instance.moneyManager.GetMana(4);
                break;
            case 1:
                GameManager.Instance.moneyManager.GetMana(12);
                break;
            case 2:
                GameManager.Instance.moneyManager.GetMana(36);
                break;
        }
    }
}
