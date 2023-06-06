using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class Dialogue : MonoBehaviour
{
    [SerializeField]
    Text dialogue_text;
    [SerializeField]
    Text name_text;

    [SerializeField]
    SpriteRenderer ArchLich_portrait;
    [SerializeField]
    Animator ArchLich_anim;
    [SerializeField]
    SpriteRenderer Boss_portrait;
    [SerializeField]
    Animator Boss_anim;

    Coroutine typing;
    bool typing_complete;
    StringBuilder Dialoguebuilder = new StringBuilder();
    string[] dialogue_name = new string[] { "아크리치", "클레오파트라", "카드모스", "셰어단더", "길가메시" };
    [SerializeField]
    Sprite[] boss_spr;

    int index;

    int name_path;
    string dialogue_content;
    void Update()
    {
        Playing_Dialogue();
    }

    void Set_portrait()
    {
        if(name_path == 0)
        {
            ArchLich_portrait.sortingOrder = 16;
            Boss_portrait.sortingOrder = 14;
            ArchLich_anim.Rebind();
            ArchLich_anim.speed = 1;
        }
        else
        {
            ArchLich_portrait.sortingOrder = 14;
            Boss_portrait.sortingOrder = 16;
            Boss_portrait.sprite = boss_spr[name_path - 1];
            Boss_anim.Rebind();
            Boss_anim.speed = 1;
        }
    }

    public void Init(int _index)
    {
        index = _index * 100 + 1;
        Dialoguebuilder.Clear();
        typing_complete = false;
        Get_Dialogue(index);
        ArchLich_anim.speed = 0;
        Boss_anim.speed = 0;
        Set_Dialogue();
    }

    void Set_Dialogue()
    {
        typing_complete = false;
        name_text.text = dialogue_name[name_path];
        Set_portrait();
        typing = StartCoroutine(Typing());
    }

    IEnumerator Typing()
    {
        int _index = 0;
        while(dialogue_content.Length != _index)
        {
            Dialoguebuilder.Append(dialogue_content[_index++]);
            Set_Text();
            yield return YieldCache.WaitForSeconds(0.05f);
        }
        Dialoguebuilder.Clear();
        typing_complete = true;
    }

    void Get_Dialogue(int _index)
    {
        name_path = DataBase.Dialogue_textDB[_index].namepath;
        dialogue_content = DataBase.Dialogue_textDB[_index].text;
    }

    void Set_Text()
    {
        dialogue_text.text = Dialoguebuilder.ToString();
    }

    void Playing_Dialogue()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (typing_complete)
            {
                index++;
                if (DataBase.Dialogue_textDB.ContainsKey(index))
                {
                    Get_Dialogue(index);
                    Set_Dialogue();
                }
                else
                {
                    DialogueManager.Instance.Dialogue_End();
                }
            }
            else
            {
                Dialogue_skip();
            }
        }
    }

    void Dialogue_skip()
    {
        StopCoroutine(typing);
        Dialoguebuilder.Clear();
        Dialoguebuilder.Append(dialogue_content);
        Set_Text();
        Dialoguebuilder.Clear();
        typing_complete = true;
    }
}
