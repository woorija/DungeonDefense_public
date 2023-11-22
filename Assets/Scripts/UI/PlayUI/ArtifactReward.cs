using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Networking.UnityWebRequest;

public class ArtifactReward : MonoBehaviour
{
    [SerializeField] Animator[] Artifacts;
    [SerializeField] ArtifactIcon[] Icons;
    [SerializeField] GameObject[] ArtifactButtons;
    List<int> Artifactlist;

    int count;

    private void OnEnable()
    {
        Init(); //유물획득 UI 팝업시
    }
    void Init() // UI초기화
    {
        count = 0;
        for (int i = 0; i < Artifacts.Length; i++)
        {
            Artifacts[i].Rebind();
            Artifacts[i].speed = 0;
        }
        for (int i = 0; i < ArtifactButtons.Length; i++)
        {
            if (!ArtifactButtons[i].activeSelf)
                ArtifactButtons[i].SetActive(true);
        }
    }

    public void SelectArtifact(int _num) // 유물선택버튼
    {
        Artifacts[_num].speed = 1;
        count++;
        SoundManager.Instance.PlayNormalSfx(4);
        GetRandomArtifact(_num);

        if (count == 2)
        {
            UIManager.Instance.SelectArtifactReward();
        }
    }

    //랜덤유물 획득함수
    void GetRandomArtifact(int _num)
    {
        Artifactlist = new List<int>(ArtifactManager.Instance.notHaveArtifactList.ToArray());

        int result = Artifactlist[Random.Range(0, Artifactlist.Count)];
        Icons[_num].SetArtifact(result);
        ArtifactButtons[_num].SetActive(false);
        ArtifactManager.Instance.GetArtifact(result);
    }
}
