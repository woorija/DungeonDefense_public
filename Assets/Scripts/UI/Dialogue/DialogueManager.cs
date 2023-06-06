using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    
    void Awake()
    {
        Instance = this;
    }

    [SerializeField] Dialogue dialogue;
    [SerializeField] GameObject dialogueBG;
    [SerializeField] GameObject portrait;

    public void Dialogue_Init(int _stage)
    {
        dialogue.gameObject.SetActive(true);
        dialogueBG.SetActive(true);
        portrait.SetActive(true);
        dialogue.Init(_stage);
    }
    public void Dialogue_End()
    {
        dialogue.gameObject.SetActive(false);
        dialogueBG.SetActive(false);
        portrait.SetActive(false);
        GameManager.Instance.StageStart();
    }
}
