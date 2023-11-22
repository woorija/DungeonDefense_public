using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScripts : MonoBehaviour // 버튼스크립트 모음
{
    #region 인게임 버튼
    public void onClickPauseButton()
    {
        UIManager.Instance.Pause();
        PlayClickSound();
    }

    public void onClickResumeButton()
    {
        UIManager.Instance.Resome();
        PlayClickSound();
    }

    public void onClickRestartButton()
    {
        UIManager.Instance.Restart();
        PlayClickSound();
    }

    public void onClickMainButton()
    {
        UIManager.Instance.MoveTitle();
        PlayClickSound();
    }

    public void onClickOptionButton()
    {
        UIManager.Instance.PopupOption();
        PlayClickSound();
    }

    public void onClickGameOver()
    {
        UIManager.Instance.GameOverClick();
        PlayClickSound();
    }
    #endregion

    #region 타이틀화면 버튼
    public void onClickPlay()
    {
        CustomSceneManager.Instance.SceneLoad("PlayScene");
        PlayClickSound();
    }

    public void onClickPlayInfi()
    {
        CustomSceneManager.Instance.SceneLoad("InfiScene");
        PlayClickSound();
    }

    public void onClickGameEXIT()
    {
        CustomSceneManager.Instance.EXIT();
        PlayClickSound();
    }

    public void onClickSetting()
    {
        SoundManager.Instance.Popup_settingUI();
        PlayClickSound();
    }
    #endregion
    void PlayClickSound()
    {
        SoundManager.Instance.PlayNormalSfx(3);
    }

    public void TipOpen()
    {
        TipUI.Instance.OpenTip();
    }
}
