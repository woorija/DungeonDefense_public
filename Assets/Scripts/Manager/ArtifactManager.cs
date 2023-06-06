using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/*
0. 스켈레톤 타입 공+5
1. 고스트 타입 공+5
2. 뱀파이어 타입 공+3
3. 좀비 타입 공+5

4. 스켈레톤 공+10
5. 스켈레톤 나이트 공+20
6. 데스나이트 공 + 30, 공격거리 + 1

7. 고스트 공속 + 0.5
8. 스펙터 공격거리 + 0.5, 공+5
9. 팬텀 특수능력 효과*2

10. 흡혈박쥐 공+5
11. 뱀파이어 공+10
12. 뱀파이어 로드 공+20, 특수능력 조건 5->3

13. 좀비 공속 + 0.5
14. 구울 공속 + 0.5, 공+10
15. 어보미네이션 공속 + 0.5, 특수능력 딜*2

16. 마나회복 1.5배
17. 최대마나 2배
18. 1회용 긴급탈출 *** 여러개 소지 가능 ***
 */

public class ArtifactManager : MonoBehaviour
{
    public static ArtifactManager Instance;
    public bool[] have_Artifact { get; private set; } // x번유물 소지 여부 - 실제 옵션체크용
    public List<int> Artifacts { get; private set; } // 소지중인 유물 리스트 - 랜덤제외, UI출력용
    public List<int> Not_have_Artifactlist = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };
    [SerializeField] ArtifactIcon[] ArtifactIcons; // 아이콘들
    [SerializeField] ArtifactInformation Artifact_inforUI; // 정보UI

    UnityEvent E_Artifactoption_Apply; // 유물효과 적용
    private void Awake()
    {
        Instance = this;
        have_Artifact = new bool[19];
        Artifacts = new List<int>(25);
        E_Artifactoption_Apply = new UnityEvent();
    }

    public void Reset() // 리스타트시 초기화
    {
        for (int i = 0; i < have_Artifact.Length; i++)
        {
            have_Artifact[i] = false;
        }
        Artifacts.Clear();
        for(int i = 0; i < ArtifactIcons.Length; i++)
        {
            if (ArtifactIcons[i].gameObject.activeSelf)
            {
                ArtifactIcons[i].gameObject.SetActive(false);
            }
        }
    }

    private void Start()
    {
        Init();
    }

    public void Init() // 시작시
    {
        for(int i = 0; i < have_Artifact.Length; i++)
        {
            have_Artifact[i] = false;
        }
    }
    void ArtifactIcon_Update()
    {
        for (int i = 0; i < Artifacts.Count; i++)
        {
            ArtifactIcons[i].Set_Artifact(Artifacts[i]);
        }
    }

    public void Get_Artifact(int _num) // 유물획득
    {
        have_Artifact[_num] = true;
        Artifacts.Add(_num);
        if (_num != 18)
        {
            Not_have_Artifactlist.Remove(_num);
        }
        ArtifactIcons[Artifacts.Count - 1].gameObject.SetActive(true);
        Apply_ArtifactOption(); //유물획득시 옵션적용
        ArtifactIcon_Update();
    }
    public void Get_Random_Artifact() // 아크리치 우클릭 함수
    {
        int result = Not_have_Artifactlist[Random.Range(0, Not_have_Artifactlist.Count)];
        Get_Artifact(result);
    }
    public void Use_MonsterExit_Artifact() // 긴급탈출장치 작동시
    {
        Debug.Log(Artifacts.Count);
        for (int i = 0; i < Artifacts.Count; i++)
        {
            if (ArtifactIcons[i].Get_Artifactnum() == 18)
            {
                ArtifactIcons[i].Remove_Artifact();
                break;
            }
        }
        ArtifactIcons[Artifacts.Count - 1].gameObject.SetActive(false);
        Artifacts.Remove(18);
        ArtifactIcon_Update();
        if (!Artifacts.Contains(18))
        {
            have_Artifact[18] = false; //남은 긴급탈출장치가 없으면 소지하지 않는 상태로 변경
        }
    }
    //정보창 관련
    public void Popup_information(int _num)
    {
        Artifact_inforUI.gameObject.SetActive(true);
        Artifact_inforUI.Set_infor(_num);
    }

    public void exit_information()
    {
        Artifact_inforUI.gameObject.SetActive(false);
    }

    #region 이벤트관련
    public void AddApply_list(UnityAction _function)
    {
        E_Artifactoption_Apply.AddListener(_function);
    }

    public void RemoveApply_list(UnityAction _function)
    {
        E_Artifactoption_Apply.RemoveListener(_function);
    }

    void Apply_ArtifactOption()
    {
        E_Artifactoption_Apply.Invoke();
    }
    #endregion
}
