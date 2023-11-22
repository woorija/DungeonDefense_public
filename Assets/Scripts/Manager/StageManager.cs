using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageManager : MonoBehaviour
{

    [SerializeField] Waypoints[] waypoints; //전체 웨이포인트
    [SerializeField] UnitTile[] unitTiles; //전체타일
    [SerializeField] StagewaveUI waveUI;
    [SerializeField] MonsterSpawner monsterSpawner;

    public int stage { get; private set; }
    public int wave { get; private set; }

    int monsterCount; //웨이브 몬스터카운트
    int[] monsterCategories; // 스테이지 몬스터 결정

    [SerializeField] SpriteRenderer BG;
    [SerializeField] SpriteRenderer FG;

    public Action onGameReset; //필드,몬스터 리셋용 델리게이트

    private void Awake()
    {
        monsterCategories = new int[3] { 1, 2, 3 };
    }

    public void StageStart()
    {//웨이브1 스타트
        switch (GameManager.Instance.gameMode) // 길가메시스테이지에서 BGM변경
        {
            case GameMode.Story:
                if(stage == 4)
                {
                    SoundManager.Instance.PlayBgm("Gilgamesh");
                }
                break;
            case GameMode.Infinity:
                InfinityShuffle();
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
        GameManager.Instance.moneyManager.HasManaArtifact(); // 유물소지시 최대마나 변경
        WaveStart();
    }
    public void Init()
    {
        SoundManager.Instance.PlayBgm("Stage");

        stage = 1;
        wave = 1;
        waveUI.Set_StageText(stage, wave); // 현재 웨이브 표현

        Shuffle();

        switch (GameManager.Instance.gameMode)
        {
            case GameMode.Story:
                BG.sprite = ResourceManager.GetSprite("BGs/Stage2BG"); //배경리셋
                FG.sprite = ResourceManager.GetSprite("BGs/Stage2"); //배경리셋
                break;
            case GameMode.Infinity:
                BG.sprite = ResourceManager.GetSprite("BGs/Stage1BG"); //배경리셋
                FG.sprite = ResourceManager.GetSprite("BGs/Stage1"); //배경리셋
                break;
        }

        if (Waypoints.Instance != null) // 웨이포인트 옮기기용
        {
            Waypoints.Instance.gameObject.SetActive(false);
        }

        switch (GameManager.Instance.gameMode)
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

        switch (GameManager.Instance.gameMode)
        {
            case GameMode.Story:
                //스테이지-웨이브1 스타트 전 텍스트표기
                DialogueManager.Instance.DialogueInit(GetCurrentMonstercategory());
                break;
            case GameMode.Infinity:
                WaveStart();
                break;
        }
        TileClear();
    }

    public void Reset()
    {
        Shuffle(); // 1,2,3스테이지 몬스터 종류 랜덤으로 돌리기

        UnitDrag.Instance.DragEnd(); //혹시 모를 선택된 유닛 해제
        monsterSpawner.SpawnStop(); // 스폰중지

        ArtifactManager.Instance.Reset(); //유물제거
        GameManager.Instance.moneyManager.Reset();
        GameManager.Instance.synergyManager.Reset(); //시너지 제거

        //필드정리
        onGameReset?.Invoke();
        UnitClear();
        TileClear();

    }

    void Shuffle() //스토리모드 1~3스테이지 몬스터 종족 순서 랜덤
    {
        for (int i = 0; i < monsterCategories.Length; i++)
        {
            int rand1 = UnityEngine.Random.Range(0, monsterCategories.Length);
            int rand2 = UnityEngine.Random.Range(0, monsterCategories.Length);
            int temp = monsterCategories[rand1];
            monsterCategories[rand1] = monsterCategories[rand2];
            monsterCategories[rand2] = temp;
        }
    }

    void InfinityShuffle() // 이전 스테이지와 같은 종류의 몬스터가 뜨지 않도록 반복 셔플
    {
        int temp = UnityEngine.Random.Range(1, 4);
        while (monsterCategories[0] == temp)
        {
            temp = UnityEngine.Random.Range(1, 4);
        }
        monsterCategories[0] = temp;
    }
    #region 필드클리어

    void UnitClear() // 유닛타일 리셋
    {
        for (int i = 0; i < unitTiles.Length; i++)
        {
            if (unitTiles[i].gameObject.activeSelf)
            {
                unitTiles[i].Init();
            }
        }
    }
    void TileClear()
    {
        for (int i = 0; i < unitTiles.Length; i++)
        {
            if (!unitTiles[i].gameObject.activeSelf)
            {
                unitTiles[i].gameObject.SetActive(true); // 모든 타일 활성화
            }
            else
            {
                unitTiles[i].Init(); // 타일초기화
            }
        }
    }
    #endregion

    void WaveStart()
    {
        int num = stage * 100 + wave;
        DataBase.Instance.monsterDB.SetData(num);
        waveUI.Set_StageText(stage, wave);
        monsterCount = monsterSpawner.Get_Monstercount(); // 웨이브의 몬스터 수 파악
        monsterSpawner.SpawnStart(num);
    }

    public void ReduceMonsterCount()
    {
        monsterCount--; // 몹 잡을때마다 카운트 감소
        IsWaveClear();
    }

    void IsWaveClear() // 웨이브 몬스터 다 잡으면
    {
        if (monsterCount == 0)
        {
            wave++;
            if(monsterSpawner.FindWave(stage * 100 + wave)) // 다음웨이브 있으면 스타트, 없으면 클리어
            {
                GameManager.Instance.moneyManager.GetMana(5); //보스전 이외 웨이브 클리어시 마나 5 획득
                WaveStart();
            }
            else
            {
                if(stage == 4 && wave == 4 && GameManager.Instance.gameMode.Equals(GameMode.Story))
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

    public void NextStageStart() // 스토리모드 다이얼로그 출력용
    {
        stage++;
        switch (GameManager.Instance.gameMode)
        {
            case GameMode.Story:
                DialogueManager.Instance.DialogueInit(GetCurrentMonstercategory());
                break;
            case GameMode.Infinity:
                GameManager.Instance.StageStart();
                break;
        }
    }

    public int GetCurrentMonstercategory() // 스토리모드 몬스터종류체크용
    {
        switch (GameManager.Instance.gameMode)
        {
            case GameMode.Story:
                if (stage != 4) // 마지막스테이지 체크
                {
                    return monsterCategories[stage - 1];
                }
                else
                {
                    return 99;
                }
            case GameMode.Infinity: // 5스테이지마다 길가메시 출현
                if (stage % 5 != 0)
                {
                    return monsterCategories[0];
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
        UIManager.Instance.PopupCardStart();
    }
}
