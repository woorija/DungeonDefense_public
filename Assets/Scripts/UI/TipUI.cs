using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipUI : SingletonBehaviour<TipUI>
{
    [SerializeField] GameObject tipUI;
    [SerializeField] GameObject[] tips;
    int current_tip = 0;

    public void OpenTip()
    {
        tipUI.SetActive(true);
    }
    public void CloseTip()
    {
        tipUI.SetActive(false);
    }
    public void NextTip()
    {
        if (current_tip < 2)
        {
            tips[current_tip].SetActive(false);
            current_tip++;
            tips[current_tip].SetActive(true);
        }
    }
    public void PrevTip()
    {
        if (current_tip > 0)
        {
            tips[current_tip].SetActive(false);
            current_tip--;
            tips[current_tip].SetActive(true);
        }
    }
}
