using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : SingletonBehaviour<CustomSceneManager>
{
    [SerializeField]
    CanvasGroup FadeImage;
    void Start()
    {
        Application.targetFrameRate = 60;
        FadeImage.alpha = 0;
        FadeImage.blocksRaycasts = false;
    }
    public void LoadScene(string scenename)
    {
        StartCoroutine(Fade_scene(scenename));
    }

    IEnumerator Fade_In()
    {
        FadeImage.blocksRaycasts = true;
        Time.timeScale = 0;
        while (FadeImage.alpha < 1)
        {
            FadeImage.alpha += 0.02f;
            yield return null;
        }
    }

    IEnumerator Fade_Out()
    {
        while (FadeImage.alpha > 0)
        {
            FadeImage.alpha -= 0.02f;
            yield return null;
        }
        Time.timeScale = 1;
        FadeImage.blocksRaycasts = false;
    }

    IEnumerator Fade_scene(string scenename)
    {
        yield return StartCoroutine(Fade_In());
        switch (scenename)//씬따라 bgm변경
        {
            case "TitleScene":
                SoundManager.Instance.PlayBgm("Title");
                break;
            case "PlayScene":
                SoundManager.Instance.PlayBgm("Stage");
                break;
            case "InfiScene":
                SoundManager.Instance.PlayBgm("Infinity");
                break;
        }
        SceneManager.LoadScene(scenename);
        ResourceManager.UnloadAsset();
        yield return YieldCache.WaitForSecondsRealtime(0.5f);
        yield return StartCoroutine(Fade_Out());
    }

    public void SceneLoad(string scenename)
    {
        StartCoroutine(Fade_scene(scenename));
    }
    //게임 끄기
    public void EXIT()
    {
        StartCoroutine(EXIT_APP());
    }

    IEnumerator EXIT_APP()
    {
        yield return StartCoroutine(Fade_In());
        Application.Quit();
    }

    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
