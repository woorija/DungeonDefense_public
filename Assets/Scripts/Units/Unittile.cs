using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTile : MonoBehaviour
{
    UnitBase unit;
    public bool unitCheck { get; private set; } = false;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        DeleteUnit();
        gameObject.SetActive(true); // 제거된 타일 재생성
    }

    public void CreateUnit(int _num) // 마나가 있으면 유닛을 해당 위치에 생성
    {
        if (unitCheck) return;

        unitCheck = true;
        unit = UnitSpawner.Instance.Create_Unit(transform.position, _num);
        GameManager.Instance.moneyManager.SubUnitcount(_num, 1);
    }
    public void DeleteUnit()
    {
        if (unit != null) // 소환된 유닛 제거
            unit.UnitDelete();
        unit = null;
        unitCheck = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) // 맵 초기화 이후 유닛을 생성할 수 없는 위치의 타일 제거
    {
        if (collision.CompareTag("Waypoint") || collision.CompareTag("NotUseTile"))
        {
            gameObject.SetActive(false); 
        }
    }
}
