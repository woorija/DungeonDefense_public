using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timeaccel : MonoBehaviour
{
    int timeScaleType;
    Image image;
    [SerializeField] Sprite[] accelSprites;

    private void Awake()
    {
        timeScaleType = 0;
        image = GetComponent<Image>();
    }

    public void onClick()
    {
        timeScaleType = (timeScaleType + 1) % 3;
        image.sprite = accelSprites[timeScaleType];
        switch (timeScaleType)
        {
            case 0:
                Time.timeScale = 1;
                break;
            case 1:
                Time.timeScale = 2;
                break;
            case 2:
                Time.timeScale = 4;
                break;
        }
    }
}
