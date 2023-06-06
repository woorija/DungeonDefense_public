using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdvertiseMTEC : MonoBehaviour
{
    private void Start()
    {
        //PlayerPrefs.SetInt("FirstPlay", 0);
        StartCoroutine(Scenemove());
    }


    public void Advertise_MTEC()
    {
        SoundManager.Instance.PlayBgm("Title");
        SceneManager.LoadScene("TitleScene");
    }

    IEnumerator Scenemove()
    {
        yield return new WaitForSeconds(5.0f);
        Advertise_MTEC();
    }
}
