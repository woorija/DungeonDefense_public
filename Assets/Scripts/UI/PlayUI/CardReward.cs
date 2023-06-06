using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardReward : MonoBehaviour
{
    [SerializeField] Animator[] cards;
    [SerializeField] GameObject[] card_btns;

    int[] _random = new int[4] { 0, 1, 2, 3 };

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
        shuffle();
        for (int i = 0; i < card_btns.Length; i++)
        {
            if (!card_btns[i].activeSelf)
                card_btns[i].SetActive(true);
        }
    }

    void shuffle()
    {
        for (int i = 0; i < _random.Length; i++)
        {
            int rand1 = Random.Range(0, _random.Length);
            int rand2 = Random.Range(0, _random.Length);
            int temp = _random[rand1];
            _random[rand1] = _random[rand2];
            _random[rand2] = temp;
        }
    }

    public void Select_Card(int _num) // 카드버튼 클릭시
    {
        int unit = _random[_num];
        cards[_num].speed = 1;
        if (Random.Range(0, 5) == 0) // 20%확률로 2티어 유닛 획득
        {
            unit += 4;
        }
        GameManager.Instance.Money_manager.Add_Unitcount(unit);
        cards[_num].SetInteger("Type", unit);
        card_btns[_num].SetActive(false);
        UIManager.Instance.Select_UnitReward();
    }
}
