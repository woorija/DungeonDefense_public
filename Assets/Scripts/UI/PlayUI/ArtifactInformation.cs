using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactInformation : MonoBehaviour
{
    [SerializeField] Text Artifactname;
    [SerializeField] Text Artifact_infor;
    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 정보창 위치값 설정
    }

    public void Set_infor(int _num) // 유물 정보값 받아오기
    {
        Artifactname.text = DataBase.ArtifactNameDB[_num];
        Artifact_infor.text = DataBase.ArtifactTextDB[_num];
    }
}
