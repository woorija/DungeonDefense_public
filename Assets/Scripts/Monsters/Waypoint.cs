using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    enum Direction // 웨이포인트 방향 설정
    {
        Up,
        Down,
        Left,
        Right
    };
    [SerializeField] Direction direction;
    [SerializeField] Direction direction_2way;
    
    public int Get_Dir()
    {
        return (int)direction;
    }
    
    public int Get_Dir2way()
    {
        return (int)direction_2way;
    }
}
