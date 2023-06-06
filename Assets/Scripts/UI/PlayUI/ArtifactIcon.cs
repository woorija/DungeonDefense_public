using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ArtifactIcon : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] int Artifact_num;
    [SerializeField] Image Icon_sprite;

    public void Set_Artifact(int _num) //유물 획득시 유물설정
    {
        Artifact_num = _num;
        Icon_sprite.sprite = ResourceManager.Get_Sprite("ArtifactIcons/Artifact" + _num);
    }
    public void Remove_Artifact()
    {
        Artifact_num = 0;
        Icon_sprite.sprite = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ArtifactManager.Instance.Popup_information(Artifact_num);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ArtifactManager.Instance.exit_information();
    }

    public int Get_Artifactnum() // 긴급탈출장치 파악용
    {
        return Artifact_num;
    }
}
