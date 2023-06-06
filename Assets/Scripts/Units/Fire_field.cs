using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_field : MonoBehaviour
{
    List<Monster> monsters;
    float stuntime;

    private void Awake()
    {
        monsters = new List<Monster>();
        stuntime = 0.15f;
    }

    private void Start()
    {
        GameManager.Instance.Stage_manager.Add_FieldList(Destroy);
        InvokeRepeating("Attack", 0f, 0.5f);
    }

    void Attack()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            monsters[i].Hit_to_normal(1, 0, 14);
            monsters[i].stun(stuntime);
        }
    }

    public void Destroy()
    {
        GameManager.Instance.Stage_manager.Remove_FiledList(Destroy);
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
