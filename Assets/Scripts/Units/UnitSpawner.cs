using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public static UnitSpawner Instance;
    void Awake()
    {
        Instance = this;
    }
    public UnitBase Create_Unit(Vector3 _pos,int _unitnum) //유닛 생성함수
    {
        GameObject unit = PoolManager.Instance.GetUnit(_pos);

        Create_Unit_Classname(unit, _unitnum);

        return unit.GetComponent<UnitBase>();
    }

    void Create_Unit_Classname(GameObject _unit, int _num) // 유닛 오브젝트에 클래스 부여
    {
        UnitBase temp;
        switch (_num)
        {
            case 0:
                temp = _unit.gameObject.AddComponent<Skeleton>();
                break;
            case 1:
                temp = _unit.gameObject.AddComponent<Ghost>();
                break;
            case 2:
                temp = _unit.gameObject.AddComponent<BloodBat>();
                break;
            case 3:
                temp = _unit.gameObject.AddComponent<Zombie>();
                break;

            case 4:
                temp = _unit.gameObject.AddComponent<SkeletonKnight>();
                break;
            case 5:
                temp = _unit.gameObject.AddComponent<Specter>();
                break;
            case 6:
                temp = _unit.gameObject.AddComponent<Vampire>();
                break;
            case 7:
                temp = _unit.gameObject.AddComponent<Ghoul>();
                break;

            case 8:
                temp = _unit.gameObject.AddComponent<DeathKnight>();
                break;
            case 9:
                temp = _unit.gameObject.AddComponent<Phantom>();
                break;
            case 10:
                temp = _unit.gameObject.AddComponent<VampireLord>();
                break;
            case 11:
                temp = _unit.gameObject.AddComponent<Abomination>();
                break;

            default: // 해당없음
                temp = _unit.gameObject.AddComponent<UnitBase>();
                break;
        }
        temp.UnitInit(_num); // 유닛소환
    }
}
