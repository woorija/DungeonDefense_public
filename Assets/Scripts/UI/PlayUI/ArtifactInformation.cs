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
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition); // ����â ��ġ�� ����
    }

    public void Set_infor(int _num) // ���� ������ �޾ƿ���
    {
        Artifactname.text = DataBase.ArtifactNameDB[_num];
        Artifact_infor.text = DataBase.ArtifactTextDB[_num];
    }
}
