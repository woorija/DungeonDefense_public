using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    //메뉴 할당
    [SerializeField] GameObject PauseUI; // 일시정지메뉴
    [SerializeField] GameObject GameoverUI; //게임오버UI
    [SerializeField] GameObject GameclearUI;
    [SerializeField] GameObject CardObj; // 카드선택
    [SerializeField] GameObject BoxObj; // 유물선택
    [SerializeField] GameObject RewardBG; // 선택금지용
    [SerializeField] GameObject RewardFG; // 선택금지용2
    [SerializeField] GameObject Tutorial_CardObj; //튜토 카드선택

    [SerializeField] Button FieldButton; //바탕클릭용 버튼

    private void Awake()
    {
        Instance = this;
    }

    public void Pause() // 일시정지 버튼 클릭
    {
        PauseUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resome() // 정지해제 버튼 클릭
    {
        PauseUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void Popup_Option() // 옵션창 버튼 클릭
    {
        PauseUI.SetActive(false);
        SoundManager.Instance.Popup_settingUI();
    }

    public void EXIT_Option()
    {
        PauseUI.SetActive(true);
    }

    public void Move_Title() // 타이틀 버튼 클릭
    {
        CustomSceneManager.Instance.SceneLoad("TitleScene");
    }

    public void Restart() // 재시작 버튼 클릭
    {
        GameManager.Instance.Stage_manager.ReStart();
        PauseUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void Gameover() // 적군이 아크리치에 닿을시
    {
        SoundManager.Instance.PlayNormalSfx(2);
        GameoverUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void GameClear()
    {
        GameclearUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void Gameover_Click()
    {
        CustomSceneManager.Instance.SceneLoad("TitleScene");
    }

    public void Popup_Card_Start() //스타트1번
    {
        UnitDrag.Instance.DragEnd();
        Tutorial_CardObj.SetActive(true);
        RewardBG.SetActive(true);
    }

    public void Popup_Cardreward() //스테이지1번
    {
        UnitDrag.Instance.DragEnd();
        CardObj.SetActive(true);
        RewardBG.SetActive(true);
    }

    public void Popup_Artifactreward() // 스테이지 3번
    {
        RewardBG.SetActive(true);
        FieldButton.onClick.RemoveAllListeners();
        FieldButton.gameObject.SetActive(false);
        CardObj.SetActive(false);
        BoxObj.SetActive(true);
    }

    public void EXIT_Start_reward() // 스타트 3번
    {
        RewardBG.SetActive(false);
        Tutorial_CardObj.SetActive(false);
        FieldButton.onClick.RemoveAllListeners();
        FieldButton.gameObject.SetActive(false);
        GameManager.Instance.GameStart();
    }

    public void Select_UnitReward() //스테이지 2번
    {
        FieldButton.gameObject.SetActive(true);

        switch (GameManager.Instance.gamemode)
        {
            case GameMode.Story:
                FieldButton.onClick.AddListener(Popup_Artifactreward);
                break;
            case GameMode.Infinity:
                FieldButton.onClick.AddListener(EXIT_Reward_infinitymode);
                break;
        }

        StartCoroutine(Cor_Select_Reward());
    }

    public void Select_StartReward() // 스타트 2번
    {
        FieldButton.gameObject.SetActive(true);
        FieldButton.onClick.AddListener(EXIT_Start_reward);
        StartCoroutine(Cor_Select_Reward());
    }

    public void Select_ArtifactReward() // 4번
    {
        FieldButton.gameObject.SetActive(true);
        switch (GameManager.Instance.gamemode)
        {
            case GameMode.Story:
                FieldButton.onClick.AddListener(EXIT_Reward_storymode);
                break;
            case GameMode.Infinity:
                FieldButton.onClick.AddListener(EXIT_Reward_infinitymode);
                break;
        }
        StartCoroutine(Cor_Select_Reward());
    }

    IEnumerator Cor_Select_Reward() //클릭방지용
    {
        RewardFG.SetActive(true);
        yield return YieldCache.WaitForSeconds(2.0f);
        RewardFG.SetActive(false);
    }

    public void EXIT_Reward_storymode() // 5번
    {
        RewardBG.SetActive(false);
        BoxObj.SetActive(false);
        FieldButton.onClick.RemoveAllListeners();
        FieldButton.gameObject.SetActive(false);
        GameManager.Instance.Stage_manager.Nextstage_Start();
    }

    public void EXIT_Reward_infinitymode()
    {
        RewardBG.SetActive(false);
        BoxObj.SetActive(false);
        CardObj.SetActive(false);
        FieldButton.onClick.RemoveAllListeners();
        FieldButton.gameObject.SetActive(false);
        GameManager.Instance.Stage_manager.Nextstage_Start();
    }
}
