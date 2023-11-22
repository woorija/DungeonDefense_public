using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireField : MonoBehaviour
{
    List<Monster> monsters;
    float stunTime;

    private void Awake()
    {
        monsters = new List<Monster>();
        stunTime = 0.15f;
    }

    private void Start()
    {
        GameManager.Instance.stageManager.onGameReset += Destroy;
        InvokeRepeating("Attack", 0f, 0.5f);
    }

    void Attack()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            monsters[i].HitToNormal(1, 0, 14);
            monsters[i].Stun(stunTime);
        }
    }

    public void Destroy()
    {
        GameManager.Instance.stageManager.onGameReset -= Destroy;
        PoolManager.Instance.ReturnFireField(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monsters"))
        {
            monsters.Add(collision.GetComponent<Monster>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monsters"))
        {
            monsters.Remove(collision.GetComponent<Monster>());
        }
    }
    private void OnDisable()
    {
        monsters.Clear();
    }
}
