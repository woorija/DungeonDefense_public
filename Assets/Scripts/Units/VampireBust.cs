using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireBust : MonoBehaviour
{
    List<Monster> monsters;

    float stunTime;

    private void Awake()
    {
        monsters = new List<Monster>();
        stunTime = 0.1f;
    }

    private void OnEnable()
    {
        StartCoroutine(ExplosionStart());
    }

    void Bust()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            monsters[i].HitToNormal(200, 3, 11);
            monsters[i].Stun(stunTime);
        }
    }

    IEnumerator ExplosionStart()
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
