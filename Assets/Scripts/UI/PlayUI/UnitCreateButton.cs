using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitCreateButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] int unit_num; //0~11
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            UnitDrag.Instance.DragStart(unit_num);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            switch (unit_num / 8)
            {
                case 0: // 1,2티어 동일유닛 3마리로 상위유닛 1마리 생성
                    if (GameManager.Instance.Money_manager.Get_Unitcount(unit_num) >= 3)
                    {
                        GameManager.Instance.Money_manager.Add_Unitcount(unit_num + 4);
                        GameManager.Instance.Money_manager.Sub_Unitcount(unit_num, 3);
                    }
                    break;
                case 1: // 3티어 유닛하나로 마나36 회복
                    GameManager.Instance.Money_manager.Sub_Unitcount(unit_num, 1);
                    GameManager.Instance.Money_manager.Get_mana(36);
                    break;
            }
        }
    }
}
