using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageManager : MonoBehaviour
{

    [SerializeField] Waypoints[] waypoints; //전체 웨이포인트
    [SerializeField] Unittile[] unittiles; //전체타일
    [SerializeField] StagewaveUI waveUI;
    [SerializeField] MonsterSpawner monsterspawner;

    public int stage { get; private set; }
    public int wave { get; private set; }

    int monster_count; //웨이브 몬스터카운트
    int[] monster_category; // 스테이지 몬스터 결정

    [SerializeField] SpriteRenderer BG_BG;
    [SerializeField] SpriteRenderer BG_FG;

    UnityEvent E_FieldClear; // 필드초기화 이벤트
    UnityEvent E_MonsterClear; // 몬스터리셋 이벤트

    private void Awake()
    {
        E_FieldClear = new UnityEvent();
        E_MonsterClear = new UnityEvent();
        monster_category = new int[3] { 1, 2, 3 };
    }

    public void StageStart()
    {//웨이브1 스타트
        switch (GameManager.Instance.gamemode) // 길가메시스테이지에서 BGM변경
        {
            case GameMode.Story:
                if(stage == 4)
                {
                    SoundManager.Instance.PlayBgm("Gilgamesh");
                }
                break;
            case GameMode.Infinity:
                infinity_shuffle();
                if (stage % 5 == 0)
                {
                    SoundManager.Instance.PlayBgm("Gilgamesh");
                }
                else if(stage % 5 == 1){
                    SoundManager.Instance.PlayBgm("Infinity");
                }
                break;
        }
        wave = 1;
        GameManager.Instance.Money_manager.HaveManaArtifact(); // 유물소지시 최대마나 변경
        WaveStart();
    }
    public void Init()
    {
        SoundManager.Instance.PlayBgm("Stage");

        stage = 1;
        wave = 1;
        waveUI.Set_StageText(stage, wave); // 현재 웨이브 표현

        shuffle();

        switch (GameManager.Instance.gamemode)
        {
            case GameMode.Story:
                BG_BG.sprite = ResourceManager.Get_Sprite("BGs/Stage2BG"); //배경리셋
                BG_FG.sprite = ResourceManager.Get_Sprite("BGs/Stage2"); //배경리셋
                break;
            case GameMode.Infinity:
                BG_BG.sprite = ResourceManager.Get_Sprite("BGs/Stage1BG"); //배경리셋
                BG_FG.sprite = ResourceManager.Get_Sprite("BGs/Stage1"); //배경리셋
                break;
        }

        if (Waypoints.Instance != null) // 웨이포인트 옮기기용
        {
            Waypoints.Instance.gameObject.SetActive(false);
        }

        switch (GameManager.Instance.gamemode)
        {
            case GameMode.Story:
                waypoints[0].gameObject.SetActive(true);
                waypoints[0].Set_Instance(); // 인스턴스 바꾸기
                break;
            case GameMode.Infinity:
                waypoints[1].gameObject.SetActive(true);
                waypoints[1].Set_Instance(); // 인스턴스 바꾸기
                break;
        }

        switch (GameManager.Instance.gamemode)
        {
            case GameMode.Story:
                //스테이지-웨이브1 스타트 전 텍스트표기
                DialogueManager.Instance.Dialogue_Init(Get_current_monstercategory());
                break;
            case GameMode.Infinity:
                WaveStart();
                break;
        }
        TileClear();
    }

    public void Reset()
    {
        shuffle(); // 1,2,3스테이지 몬스터 종류 랜덤으로 돌리기

        UnitDrag.Instance.DragEnd(); //혹시 모를 선택된 유닛 해제
        monsterspawner.SpawnStop(); // 스폰중지

        ArtifactManager.Instance.Reset(); //유물제거
        GameManager.Instance.Money_manager.Reset();
        GameManager.Instance.Synergy_manager.Reset(); //시너지 제거

        //필드정리
        FieldClear();
        MonsterClear();
        UnitClear();
        TileClear();

    }

    void shuffle() //스토리모드 1~3스테이지 몬스터 종족 순서 랜덤
    {
        for (int i = 0; i < monster_category.Length; i++)
        {
            int rand1 = Random.Range(0, monster_category.Length);
            int rand2 = Random.Range(0, monster_category.Length);
            int temp = monster_category[rand1];
            monster_category[rand1] = monster_category[rand2];
            monster_category[rand2] = temp;
        }
    }

    void infinity_shuffle() // 이전 스테이지와 같은 종류의 몬스터가 뜨지 않도록 반복 셔플
    {
        int temp = Random.Range(1, 4);
        while (monster_category[0] == temp)
        {
            temp = Random.Range(1, 4);
        }
        monster_category[0] = temp;
    }
    #region 필드클리어
    void FieldClear()
    {
        E_FieldClear.Invoke(); 
    }

    void MonsterClear()
    {
        E_MonsterClear.Invoke();
    }

    void UnitClear() // 유닛타일 리셋
    {
        for (int i = 0; i < unittiles.Length; i++)
        {
            if (unittiles[i].gameObject.activeSelf)
            {
                unittiles[i].Init();
            }
        }
    }
    void TileClear()
    {
        for (int i = 0; i < unittiles.Length; i++)
        {
            if (!unittiles[i].gameObject.activeSelf)
            {
                unittiles[i].gameObject.SetActive(true); // 모든 타일 활성화
            }
            else
            {
                unittiles[i].Init(); // 타일초기화
            }
        }
    }
    #endregion

    void WaveStart()
    {
        int num = stage * 100 + wave;
        DataBase.Instance.monsterDB.SetData(num);
        waveUI.Set_StageText(stage, wave);
        monster_count = monsterspawner.Get_Monstercount(); // 웨이브의 몬스터 수 파악
        monsterspawner.SpawnStart(num);
    }

    public void dec_monstercount()
    {
        monster_count--; // 몹 잡을때마다 카운트 감소
        IsWaveClear();
    }

    void IsWaveClear() // 웨이브 몬스터 다 잡으면
    {
        if (monster_count == 0)
        {
            wave++;
            if(monsterspawner.FindWave(stage * 100 + wave)) // 다음웨이브 있으면 스타트, 없으면 클리어
            {
                GameManager.Instance.Money_manager.Get_mana(5); //보스전 이외 웨이브 클리어시 마나 5 획득
                WaveStart();
            }
            else
            {
                if(stage == 4 && wave == 4 && GameManager.Instance.gamemode == GameMode.Story)
                {
                    UIManager.Instance.GameClear();
                }
                else
                {
                    GameManager.Instance.StageClear();
                }
            }
        }
    }

    public void Nextstage_Start() // 스토리모드 다이얼로그 출력용
    {
        stage++;
        switch (GameManager.Instance.gamemode)
        {
            case GameMode.Story:
                DialogueManager.Instance.Dialogue_Init(Get_current_monstercategory());
                break;
            case GameMode.Infinity:
                GameManager.Instance.StageStart();
                break;
        }
    }

    public int Get_current_monstercategory() // 스토리모드 몬스터종류체크용
    {
        switch (GameManager.Instance.gamemode)
        {
            case GameMode.Story:
                if (stage != 4) // 마지막스테이지 체크
                {
                    return monster_category[stage - 1];
                }
                else
                {
                    return 99;
                }
            case GameMode.Infinity: // 5스테이지마다 길가메시 출현
                if (stage % 5 != 0)
                {
                    return monster_category[0];
                }
                else
                {
                    return 99;
                }
        }
        return 1;
    }

    public void ReStart() //리스타트버튼 - 스테이지 초기화용
    {
        Reset();
        UIManager.Instance.Popup_Card_Start();
    }

    #region 이벤트관리
    public void Add_FieldList(UnityAction _function)
    {
        E_FieldClear.AddListener(_function);
    }

    public void Remove_FiledList(UnityAction _function)
    {
        E_FieldClear.RemoveListener(_function);
    }

    public void Add_MonsterList(UnityAction _function)
    {
        E_MonsterClear.AddListener(_function);
    }

    public void Remove_MonsterList(UnityAction _function)
    {
        E_MonsterClear.RemoveListener(_function);
    }
    #endregion
}
