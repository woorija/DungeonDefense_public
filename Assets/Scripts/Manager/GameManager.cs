using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    Story,
    Infinity
}

public class GameManager : MonoBehaviour //총괄매니저
{
    public static GameManager Instance;
    public SynergyManager synergyManager { get; private set; }
    public MoneyManager moneyManager { get; private set; }
    public StageManager stageManager { get; private set; }
    public AbilityManager abilityManager { get; private set; }

    public GameMode gameMode; //스토리,무한모드에 따른 인게임 변동 체크

    private void Awake()
    {
        //각종 매니저 캐싱
        Instance = this;
        synergyManager = GetComponent<SynergyManager>();
        moneyManager = GetComponent<MoneyManager>();
        stageManager = GetComponent<StageManager>();
        abilityManager = GetComponent<AbilityManager>();
    }

    private void Start()
    {
        GameInit();
    }
    void GameInit()
    {
        synergyManager.Init();
        moneyManager.Init();
        abilityManager.Init();
        //매니저 초기화
        StartCoroutine(UnitSelectPopup()); // 4개카드 팝업
    }

    IEnumerator UnitSelectPopup()
    {
        yield return null; // UI싱글톤 체크용
        UIManager.Instance.PopupCardStart();
    }

    public void StageStart() // x스테이지 시작
    {
        SoundManager.Instance.PlayNormalSfx(0);
        stageManager.StageStart();
    }

    public void GameStart() // 1스테이지 시작
    {
        SoundManager.Instance.PlayNormalSfx(0);
        stageManager.Init();
    }

    public void StageClear() // 스테이지 마지막 웨이브 클리어시
    {
        SoundManager.Instance.PlayNormalSfx(1);
        switch (gameMode)
        {
            case GameMode.Story:
                UIManager.Instance.PopupCardReward();
                break;
            case GameMode.Infinity:
                if(stageManager.stage%3 == 0)
                {
                    UIManager.Instance.PopupArtifactReward();
                }
                else
                {
                    UIManager.Instance.PopupCardReward();
                }
                break;
        }
    }
}
