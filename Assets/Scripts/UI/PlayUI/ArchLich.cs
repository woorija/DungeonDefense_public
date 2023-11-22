using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArchLich : MonoBehaviour,IPointerClickHandler
{
    int useManaToUnit = 5;
    int useManaToArtifact = 10;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (GameManager.Instance.moneyManager.UseMana(useManaToUnit))
            {
                GameManager.Instance.moneyManager.AddUnitcount(Random.Range(0, 4));
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (ArtifactManager.Instance.Artifacts.Count <= 20 && GameManager.Instance.moneyManager.UseMana(useManaToArtifact))
            {
                ArtifactManager.Instance.GetRandomArtifact();
            }
        }
    }
}
