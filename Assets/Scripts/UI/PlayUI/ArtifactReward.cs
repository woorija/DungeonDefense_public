using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Networking.UnityWebRequest;

public class ArtifactReward : MonoBehaviour
{
    [SerializeField] Animator[] Artifacts;
    [SerializeField] ArtifactIcon[] Icons;
    [SerializeField] GameObject[] Artifact_btns;
    List<int> Artifactlist;

    int _count;

    private void OnEnable()
    {
        Init(); //유물획득 UI 팝업시
    }
    void Init() // UI초기화
    {
        _count = 0;
        for (int i = 0; i < Artifacts.Length; i++)
        {
            Artifacts[i].Rebind();
            Artifacts[i].speed = 0;
        }
        for (int i = 0; i < Artifact_btns.Length; i++)
        {
            if (!Artifact_btns[i].activeSelf)
                Artifact_btns[i].SetActive(true);
        }
    }

    public void Select_Artifact(int _num) // 유물선택버튼
    {
        Artifacts[_num].speed = 1;
        _count++;
        SoundManager.Instance.PlayNormalSfx(4);
        Get_RandomArtifact(_num);

        if (_count == 2)
        {
            UIManager.Instance.Select_ArtifactReward();
        }
    }

    //랜덤유물 획득함수
    void Get_RandomArtifact(int _num)
    {
        Artifactlist = new List<int>(ArtifactManager.Instance.Not_have_Artifactlist.ToArray());

        int result = Artifactlist[Random.Range(0, Artifactlist.Count)];
        Icons[_num].Set_Artifact(result);
        Artifact_btns[_num].SetActive(false);
        ArtifactManager.Instance.Get_Artifact(result);
    }
}
