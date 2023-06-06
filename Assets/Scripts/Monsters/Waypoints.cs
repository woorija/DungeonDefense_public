using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Waypoints Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void Set_Instance() //스테이지 변경시 인스턴스 변경
    {
        Instance = this;
    }

    public Waypoint[] waypoints; //스테이지별 총 웨이포인트
}
