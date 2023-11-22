using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardReward : MonoBehaviour
{
    [SerializeField] Animator[] cards;
    [SerializeField] GameObject[] cardButtons;

    int[] random = new int[4] { 0, 1, 2, 3 };

    private void OnEnable()
    {
        Init(); // 유닛획득 UI 팝업시
    }
    void Init()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].Rebind();
            cards[i].speed = 0;
        }
        Shuffle();
        for (int i = 0; i < cardButtons.Length; i++)
        {
            if (!cardButtons[i].activeSelf)
                cardButtons[i].SetActive(true);
        }
    }

    void Shuffle()
    {
        for (int i = 0; i < random.Length; i++)
        {
            int rand1 = Random.Range(0, random.Length);
            int rand2 = Random.Range(0, random.Length);
            int temp = random[rand1];
            random[rand1] = random[rand2];
            random[rand2] = temp;
        }
    }

    public void SelectCard(int _num) // 카드버튼 클릭시
    {
        int unit = random[_num];
        cards[_num].speed = 1;
        if (Random.Range(0, 5) == 0) // 20%확률로 2티어 유닛 획득
        {
            unit += 4;
        }
        GameManager.Instance.moneyManager.AddUnitcount(unit);
        cards[_num].SetInteger("Type", unit);
        cardButtons[_num].SetActive(false);
        UIManager.Instance.SelectUnitReward();
    }
}
