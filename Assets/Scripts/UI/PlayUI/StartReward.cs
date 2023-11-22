using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartReward : MonoBehaviour
{
    [SerializeField] Animator[] cards;
    [SerializeField] GameObject[] cardButtons;

    int[] random = new int[4] { 0, 1, 2, 3 };

    int count;

    private void OnEnable()
    {
        Init();
    }
    void Init()
    {
        count = 0;
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].Rebind();
            cards[i].speed = 0;
        }
        Shuffle();
        for(int i = 0; i < cardButtons.Length; i++)
        {
            if (!cardButtons[i].activeSelf)
                cardButtons[i].SetActive(true);
        }
    }

    void Shuffle()
    {
        for(int i = 0; i < random.Length; i++)
        {
            int rand1 = Random.Range(0, random.Length);
            int rand2 = Random.Range(0, random.Length);
            int temp = random[rand1];
            random[rand1] = random[rand2];
            random[rand2] = temp;
        }
    }

    public void SelectCard(int _num)
    {
        int unit = random[_num];
        GameManager.Instance.moneyManager.AddUnitcount(unit);
        cards[_num].speed = 1;
        cards[_num].SetInteger("Type", unit);
        cardButtons[_num].SetActive(false);
        count++;
        if (count == 2)
        {
            UIManager.Instance.SelectStartReward();
        }
    }
}
