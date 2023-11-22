using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ArtifactIcon : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] int ArtifactNum;
    [SerializeField] Image IconSprite;

    public void SetArtifact(int _num) //유물 획득시 유물설정
    {
        ArtifactNum = _num;
        IconSprite.sprite = ResourceManager.GetSprite("ArtifactIcons/Artifact" + _num);
    }
    public void RemoveArtifact()
    {
        ArtifactNum = 0;
        IconSprite.sprite = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ArtifactManager.Instance.PopupInformation(ArtifactNum);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ArtifactManager.Instance.ExitInformation();
    }

    public int GetArtifactNumber() // 긴급탈출장치 파악용
    {
        return ArtifactNum;
    }
}
