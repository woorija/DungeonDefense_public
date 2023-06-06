using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScripts : MonoBehaviour // 버튼스크립트 모음
{
    #region 인게임 버튼
    public void PauseBtn_onClick()
    {
        UIManager.Instance.Pause();
        click_sound();
    }

    public void ResumeBtn_onClick()
    {
        UIManager.Instance.Resome();
        click_sound();
    }

    public void RestartBtn_onClick()
    {
        UIManager.Instance.Restart();
        click_sound();
    }

    public void MainBtn_onClick()
    {
        UIManager.Instance.Move_Title();
        click_sound();
    }

    public void OptionBtn_onClick()
    {
        UIManager.Instance.Popup_Option();
        click_sound();
    }

    public void Gameover_onClick()
    {
        UIManager.Instance.Gameover_Click();
        click_sound();
    }
    #endregion

    #region 타이틀화면 버튼
    public void Play_onClick()
    {
        CustomSceneManager.Instance.SceneLoad("PlayScene");
        click_sound();
    }

    public void PlayInfi_onClick()
    {
        CustomSceneManager.Instance.SceneLoad("InfiScene");
        click_sound();
    }

    public void Game_EXIT_onClick()
    {
        CustomSceneManager.Instance.EXIT();
        click_sound();
    }

    public void Setting_onClick()
    {
        SoundManager.Instance.Popup_settingUI();
        click_sound();
    }
    #endregion
    void click_sound()
    {
        SoundManager.Instance.PlayNormalSfx(3);
    }

    public void TipOpen()
    {
        TipUI.Instance.OpenTip();
    }
}
