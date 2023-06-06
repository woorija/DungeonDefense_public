using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    int unitcount = 70;
    int monstercount = 30;
    int explosioncount = 20;
    int firecount = 30;
    int bustcount = 40;

    Queue<GameObject> UnitPool;
    Queue<Monster> MonsterPool;
    Queue<Explosion> ExplosionPool;
    Queue<Fire_field> FireFieldPool;
    Queue<VampireBust> BustPool;

    [SerializeField] GameObject unitprefab;
    [SerializeField] Monster monsterprefab;
    [SerializeField] Explosion explosionprefab;
    [SerializeField] Fire_field firefieldprefab;
    [SerializeField] VampireBust bustprefab;
    private void Awake()
    {
        Instance= this;
        PoolInit();
    }
    void PoolInit()
    {
        UnitPool= new Queue<GameObject>(unitcount);
        MonsterPool = new Queue<Monster>(monstercount);
        ExplosionPool = new Queue<Explosion>(explosioncount);
        FireFieldPool= new Queue<Fire_field>(firecount);
        BustPool = new Queue<VampireBust>(bustcount);
        for (int i=0; i < unitcount; i++)
        {
            GameObject unit = Instantiate(unitprefab);
            UnitPool.Enqueue(unit);
            unit.SetActive(false);
        }
        for (int i = 0; i < monstercount; i++)
        {
            Monster monster = Instantiate(monsterprefab);
            MonsterPool.Enqueue(monster);
            monster.gameObject.SetActive(false);
        }
        for (int i = 0; i < explosioncount; i++)
        {
            Explosion explosion = Instantiate(explosionprefab);
            ExplosionPool.Enqueue(explosion);
            explosion.gameObject.SetActive(false);
        }
        for (int i = 0; i < firecount; i++)
        {
            Fire_field firefield = Instantiate(firefieldprefab);
            FireFieldPool.Enqueue(firefield);
            firefield.gameObject.SetActive(false);
        }
        for(int i = 0; i < bustcount; i++)
        {
            VampireBust bust = Instantiate(bustprefab);
            BustPool.Enqueue(bust);
            bust.gameObject.SetActive(false);
        }
    }
    public GameObject GetUnit()
    {
        var temp = UnitPool.Dequeue();
        temp.SetActive(true);
        return temp;
    }
    public GameObject GetUnit(Vector3 _pos)
    {
        var temp = UnitPool.Dequeue();
        temp.SetActive(true);
        temp.transform.position = _pos;
        return temp;
    }
    public Monster GetMonster()
    {
        var temp = MonsterPool.Dequeue();
        temp.gameObject.SetActive(true);
        return temp;
    }
    public Monster GetMonster(Vector3 _pos)
    {
        Monster temp = MonsterPool.Dequeue();
        temp.gameObject.SetActive(true);
        temp.gameObject.transform.position = _pos;
        return temp;
    }
    public Explosion GetExplosion()
    {
        var temp = ExplosionPool.Dequeue();
        temp.gameObject.SetActive(true);
        return temp;
    }
    public Explosion GetExplosion(Vector3 _pos)
    {
        var temp = ExplosionPool.Dequeue();
        temp.gameObject.SetActive(true);
        temp.gameObject.transform.position = _pos;
        return temp;
    }
    public Fire_field GetFireField()
    {
        var temp = FireFieldPool.Dequeue();
        temp.gameObject.SetActive(true);
        return temp;
    }
    public Fire_field GetFireField(Vector3 _pos)
    {
        var temp = FireFieldPool.Dequeue();
        temp.gameObject.SetActive(true);
        temp.gameObject.transform.position = _pos;
        return temp;
    }
    public VampireBust GetBust()
    {
        var temp = BustPool.Dequeue();
        temp.gameObject.SetActive(true);
        return temp;
    }
    public VampireBust GetBust(Vector3 _pos)
    {
        var temp = BustPool.Dequeue();
        temp.gameObject.SetActive(true);
        temp.gameObject.transform.position = _pos;
        return temp;
    }

    public void ReturnUnit(GameObject unit)
    {
        UnitPool.Enqueue(unit);
        unit.SetActive(false);
    }
    public void ReturnMonster(Monster monster)
    {
        MonsterPool.Enqueue(monster);
        monster.gameObject.SetActive(false);
    }
    public void ReturnExplosion(Explosion explosion)
    {
        ExplosionPool.Enqueue(explosion);
        explosion.gameObject.SetActive(false);
    }
    public void ReturnFireField(Fire_field firefield)
    {
        FireFieldPool.Enqueue(firefield);
        firefield.gameObject.SetActive(false);
    }
    public void ReturnBust(VampireBust bust)
    {
        BustPool.Enqueue(bust);
        bust.gameObject.SetActive(false);
    }
}
