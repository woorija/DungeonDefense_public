using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeaccel : MonoBehaviour
{
    int timescaletype;
    private void Awake()
    {
        timescaletype = 0;
    }

    public void onClick()
    {
        switch(timescaletype)
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
        timescaletype = (timescaletype + 1) % 3;
    }
}
