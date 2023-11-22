using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipUI : SingletonBehaviour<TipUI>
{
    [SerializeField] GameObject tipUI;
    [SerializeField] GameObject[] tips;
    int currentTipPage = 0;

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
        if (currentTipPage < 2)
        {
            tips[currentTipPage].SetActive(false);
            currentTipPage++;
            tips[currentTipPage].SetActive(true);
        }
    }
    public void PrevTip()
    {
        if (currentTipPage > 0)
        {
            tips[currentTipPage].SetActive(false);
            currentTipPage--;
            tips[currentTipPage].SetActive(true);
        }
    }
}
