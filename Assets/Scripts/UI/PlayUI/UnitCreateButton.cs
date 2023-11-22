using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitCreateButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] int unitNum; //0~11
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            UnitDrag.Instance.DragStart(unitNum);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            switch (unitNum / 8)
            {
                case 0: // 1,2티어 동일유닛 3마리로 상위유닛 1마리 생성
                    if (GameManager.Instance.moneyManager.GetUnitcount(unitNum) >= 3)
                    {
                        GameManager.Instance.moneyManager.AddUnitcount(unitNum + 4);
                        GameManager.Instance.moneyManager.SubUnitcount(unitNum, 3);
                    }
                    break;
                case 1: // 3티어 유닛하나로 마나36 회복
                    GameManager.Instance.moneyManager.SubUnitcount(unitNum, 1);
                    GameManager.Instance.moneyManager.GetMana(36);
                    break;
            }
        }
    }
}
