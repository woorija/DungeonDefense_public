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
    public SynergyManager Synergy_manager { get; private set; }
    public MoneyManager Money_manager { get; private set; }
    public StageManager Stage_manager { get; private set; }
    public AbilityManager Ability_manager { get; private set; }

    public GameMode gamemode; //스토리,무한모드에 따른 인게임 변동 체크

    private void Awake()
    {
        //각종 매니저 캐싱
        Instance = this;
        Synergy_manager = GetComponent<SynergyManager>();
        Money_manager = GetComponent<MoneyManager>();
        Stage_manager = GetComponent<StageManager>();
        Ability_manager = GetComponent<AbilityManager>();
    }

    private void Start()
    {
        GameInit();
    }
    void GameInit()
    {
        Synergy_manager.Init();
        Money_manager.Init();
        Ability_manager.Init();
        //매니저 초기화
        StartCoroutine(UnitSelectPopup()); // 4개카드 팝업
    }

    IEnumerator UnitSelectPopup()
    {
        yield return null; // UI싱글톤 체크용
        UIManager.Instance.Popup_Card_Start();
    }

    public void StageStart() // x스테이지 시작
    {
        SoundManager.Instance.PlayNormalSfx(0);
        Stage_manager.StageStart();
    }

    public void GameStart() // 1스테이지 시작
    {
        SoundManager.Instance.PlayNormalSfx(0);
        Stage_manager.Init();
    }

    public void StageClear() // 스테이지 마지막 웨이브 클리어시
    {
        SoundManager.Instance.PlayNormalSfx(1);
        switch (gamemode)
        {
            case GameMode.Story:
                UIManager.Instance.Popup_Cardreward();
                break;
            case GameMode.Infinity:
                if(Instance.Stage_manager.stage%3 == 0)
                {
                    UIManager.Instance.Popup_Artifactreward();
                }
                else
                {
                    UIManager.Instance.Popup_Cardreward();
                }
                break;
        }
    }
}
