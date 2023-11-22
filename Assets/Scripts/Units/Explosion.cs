using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    List<Monster> monsters;
    ParticleSystem particle;

    float stunTime;

    private void Awake()
    {
        monsters = new List<Monster>();
        particle = GetComponent<ParticleSystem>();
        stunTime = 0.4f;
    }

    private void OnEnable()
    {
        StartCoroutine(ExplosionStart());
        particle.Play();
    }

    void explosion()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            monsters[i].HitToExplosion(GameManager.Instance.synergyManager.zombieExplosionDamage);
            monsters[i].Stun(stunTime);
        }
    }

    IEnumerator ExplosionStart()
    {
        yield return new WaitForSeconds(0.03f);
        explosion();
        yield return new WaitForSeconds(1.0f);
        particle.Stop();
        PoolManager.Instance.ReturnExplosion(this);
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
