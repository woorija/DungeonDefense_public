using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitDrag : MonoBehaviour
{
    public static UnitDrag Instance;

    public bool isDrag { get; private set; } // 유닛 드래그 판별

    bool isFieldDrag; // 장판 설치상태인가
    bool isSetField; // 장판 설치 판별
    public int unit_num { get; private set; }

    SpriteRenderer drag_sprite; // 드래그시 나올 스프라이트

    float tile_size = 120f; // 타일크기
    private void Awake()
    {
        isDrag = false;
        isFieldDrag = false;
        isSetField = false;
        drag_sprite = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    public void DragStart(int _num)
    {
        isDrag = true;
        unit_num = _num;
        drag_sprite.sprite = ResourceManager.GetSpriteToAtlas("DragSprites", DataBase.Instance.unitDB.UnitDataBase[_num].path);
    }

    public void DK_SpecialAbility() //데나 소환시 자동적용되는 장판생성용 드래그호출함수
    {
        isDrag = false;
        isFieldDrag = true;
        drag_sprite.sprite = ResourceManager.GetSpriteToAtlas("DragSprites", "Field");
    }

    public void DragEnd()
    {
        isDrag = false;
        isFieldDrag = false;
        drag_sprite.sprite = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDrag || isFieldDrag)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 tile_pos = new Vector3(tile_size * Mathf.Ceil(pos.x / tile_size) - (tile_size*0.5f), tile_size * Mathf.Ceil((pos.y - (tile_size * 0.5f)) / tile_size), 0);
            transform.position = tile_pos;
        }

        if (Input.GetMouseButtonDown(1))//우클릭시 생성취소
        {
            DragEnd();
        }

        if(isSetField && Input.GetMouseButtonDown(0))
        {
            PoolManager.Instance.GetFireField(transform.position);
            isFieldDrag = false;
            isSetField = false;
            DragEnd();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) //장판적용용
    {
        if (isFieldDrag)
        {
            if (collision.CompareTag("Waypoint"))
            {
                isSetField = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) //장판적용용
    {
        if (collision.CompareTag("Waypoint"))
        {
            isSetField = false;
        }
    }
}
