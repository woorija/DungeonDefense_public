using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartReward : MonoBehaviour
{
    [SerializeField] Animator[] cards;
    [SerializeField] GameObject[] card_btns;

    int[] _random = new int[4] { 0, 1, 2, 3 };

    int _count;

    private void OnEnable()
    {
        Init();
    }
    void Init()
    {
        _count = 0;
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].Rebind();
            cards[i].speed = 0;
        }
        shuffle();
        for(int i = 0; i < card_btns.Length; i++)
        {
            if (!card_btns[i].activeSelf)
                card_btns[i].SetActive(true);
        }
    }

    void shuffle()
    {
        for(int i = 0; i < _random.Length; i++)
        {
            int rand1 = Random.Range(0, _random.Length);
            int rand2 = Random.Range(0, _random.Length);
            int temp = _random[rand1];
            _random[rand1] = _random[rand2];
            _random[rand2] = temp;
        }
    }

    public void Select_Card(int _num)
    {
        int unit = _random[_num];
        GameManager.Instance.Money_manager.Add_Unitcount(unit);
        cards[_num].speed = 1;
        cards[_num].SetInteger("Type", unit);
        card_btns[_num].SetActive(false);
        _count++;
        if (_count == 2)
        {
            UIManager.Instance.Select_StartReward();
        }
    }
}
