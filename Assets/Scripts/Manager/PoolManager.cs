using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    int unitCount = 70;
    int monsterCount = 30;
    int explosionCount = 20;
    int fireCount = 30;
    int bustCount = 40;

    Queue<GameObject> UnitPool;
    Queue<Monster> MonsterPool;
    Queue<Explosion> ExplosionPool;
    Queue<FireField> FireFieldPool;
    Queue<VampireBust> BustPool;

    [SerializeField] GameObject unitprefab;
    [SerializeField] Monster monsterprefab;
    [SerializeField] Explosion explosionprefab;
    [SerializeField] FireField firefieldprefab;
    [SerializeField] VampireBust bustprefab;
    private void Awake()
    {
        Instance= this;
        PoolInit();
    }
    void PoolInit()
    {
        UnitPool= new Queue<GameObject>(unitCount);
        MonsterPool = new Queue<Monster>(monsterCount);
        ExplosionPool = new Queue<Explosion>(explosionCount);
        FireFieldPool= new Queue<FireField>(fireCount);
        BustPool = new Queue<VampireBust>(bustCount);
        for (int i=0; i < unitCount; i++)
        {
            GameObject unit = Instantiate(unitprefab);
            UnitPool.Enqueue(unit);
            unit.SetActive(false);
        }
        for (int i = 0; i < monsterCount; i++)
        {
            Monster monster = Instantiate(monsterprefab);
            MonsterPool.Enqueue(monster);
            monster.gameObject.SetActive(false);
        }
        for (int i = 0; i < explosionCount; i++)
        {
            Explosion explosion = Instantiate(explosionprefab);
            ExplosionPool.Enqueue(explosion);
            explosion.gameObject.SetActive(false);
        }
        for (int i = 0; i < fireCount;  i++)
        {
            FireField firefield = Instantiate(firefieldprefab);
            FireFieldPool.Enqueue(firefield);
            firefield.gameObject.SetActive(false);
        }
        for(int i = 0; i < bustCount; i++)
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
    public FireField GetFireField()
    {
        var temp = FireFieldPool.Dequeue();
        temp.gameObject.SetActive(true);
        return temp;
    }
    public FireField GetFireField(Vector3 _pos)
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
    public void ReturnFireField(FireField firefield)
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
