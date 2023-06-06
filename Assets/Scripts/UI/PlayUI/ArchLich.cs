using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArchLich : MonoBehaviour,IPointerClickHandler
{
    int use_unitmana = 5;
    int use_artifactmana = 10;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (GameManager.Instance.Money_manager.UseMana(use_unitmana))
            {
                GameManager.Instance.Money_manager.Add_Unitcount(Random.Range(0, 4));
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (ArtifactManager.Instance.Artifacts.Count <= 20 && GameManager.Instance.Money_manager.UseMana(use_artifactmana))
            {
                ArtifactManager.Instance.Get_Random_Artifact();
            }
        }
    }
}
