using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    List<Monster> monsters;
    ParticleSystem particle;

    float stuntime;

    private void Awake()
    {
        monsters = new List<Monster>();
        particle = GetComponent<ParticleSystem>();
        stuntime = 0.4f;
    }

    private void OnEnable()
    {
        StartCoroutine(explosion_start());
        particle.Play();
    }

    void explosion()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            monsters[i].Hit_to_explosion(GameManager.Instance.Synergy_manager.zombie_explosion_damage);
            monsters[i].stun(stuntime);
        }
    }

    IEnumerator explosion_start()
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
