using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class Dialogue : MonoBehaviour
{
    [SerializeField]
    Text dialogueText;
    [SerializeField]
    Text nameText;

    [SerializeField]
    SpriteRenderer ArchLichPortrait;
    [SerializeField]
    Animator ArchLichAnimator;
    [SerializeField]
    SpriteRenderer BossPortrait;
    [SerializeField]
    Animator BossAnimator;

    Coroutine typingCoroutine;
    bool isTypingComplete;
    StringBuilder dialogueBuilder = new StringBuilder();
    string[] dialogueName = new string[] { "아크리치", "클레오파트라", "카드모스", "셰어단더", "길가메시" };
    [SerializeField]
    Sprite[] bossSprites;
    int index;

    int namePath;
    string dialogueContent;
    void Update()
    {
        PlayingDialogue();
    }

    void SetPortrait()
    {
        if(namePath == 0)
        {
            ArchLichPortrait.sortingOrder = 16;
            BossPortrait.sortingOrder = 14;
            ArchLichAnimator.Rebind();
            ArchLichAnimator.speed = 1;
        }
        else
        {
            ArchLichPortrait.sortingOrder = 14;
            BossPortrait.sortingOrder = 16;
            BossPortrait.sprite = bossSprites[namePath - 1];
            BossAnimator.Rebind();
            BossAnimator.speed = 1;
        }
    }

    public void Init(int _index)
    {
        index = _index * 100 + 1;
        dialogueBuilder.Clear();
        isTypingComplete = false;
        GetDialogue(index);
        ArchLichAnimator.speed = 0;
        BossAnimator.speed = 0;
        SetDialogue();
    }

    void SetDialogue()
    {
        isTypingComplete = false;
        nameText.text = dialogueName[namePath];
        SetPortrait();
        typingCoroutine = StartCoroutine(Typing());
    }

    IEnumerator Typing()
    {
        int _index = 0;
        while(dialogueContent.Length != _index)
        {
            dialogueBuilder.Append(dialogueContent[_index++]);
            SetText();
            yield return YieldCache.WaitForSeconds(0.05f);
        }
        dialogueBuilder.Clear();
        isTypingComplete = true;
    }

    void GetDialogue(int _index)
    {
        namePath = DataBase.Dialogue_textDB[_index].namepath;
        dialogueContent = DataBase.Dialogue_textDB[_index].text;
    }

    void SetText()
    {
        dialogueText.text = dialogueBuilder.ToString();
    }

    void PlayingDialogue()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTypingComplete)
            {
                index++;
                if (DataBase.Dialogue_textDB.ContainsKey(index))
                {
                    GetDialogue(index);
                    SetDialogue();
                }
                else
                {
                    DialogueManager.Instance.DialogueEnd();
                }
            }
            else
            {
                DialogueSkip();
            }
        }
    }

    void DialogueSkip()
    {
        StopCoroutine(typingCoroutine);
        dialogueBuilder.Clear();
        dialogueBuilder.Append(dialogueContent);
        SetText();
        dialogueBuilder.Clear();
        isTypingComplete = true;
    }
}
