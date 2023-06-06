using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireBust : MonoBehaviour
{
    List<Monster> monsters;

    float stuntime;

    private void Awake()
    {
        monsters = new List<Monster>();
        stuntime = 0.1f;
    }

    private void OnEnable()
    {
        StartCoroutine(explosion_start());
    }

    void Bust()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            monsters[i].Hit_to_normal(200, 3, 11);
            monsters[i].stun(stuntime);
        }
    }

    IEnumerator explosion_start()
    {
        yield return new WaitForSeconds(0.03f);
        Bust();
        yield return new WaitForSeconds(0.06f);
        PoolManager.Instance.ReturnBust(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monsters"))
        {
            monsters.Add(collision.GetComponent<Monster>());
        }
    }
    private void OnDisable()
    {
        monsters.Clear();
    }
}
