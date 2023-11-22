using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus : MonoBehaviour
{
    public MonsterSO BaseData { get; private set; }

    [Header("effects")]
    [SerializeField] ParticleSystem defEffect;
    [SerializeField] ParticleSystem speedEffect;
    [SerializeField] SpriteRenderer monsterType;

    public int finalDef { get; private set; }
    public float finalSpeed { get; private set; }
    public int hp { get; private set; }
    public float rewardMp { get; private set; }
    public int direction { get; private set; }
    public bool superArmor { get; private set; }
    public void Init(int _type)
    {
        InitType(_type);
        InitHp();
        InitDef();
        InitSpeed();
        direction = -1;
    }
    void InitType(int _type)
    {
        if(_type == 4)//랜덤타입이면
        {
            int rand = Random.Range(1, 4);//노멀타입제외 나머지 타입중 랜덤
            BaseData = DataBase.Instance.monsterDB.monsterdata[rand];
            monsterType.material = DataBase.Instance.MonsterMaterials[rand];
            InitArmor(rand);
        }
        else
        {
            BaseData = DataBase.Instance.monsterDB.monsterdata[_type];
            monsterType.material = DataBase.Instance.MonsterMaterials[_type];
            InitArmor(_type);
        }
    }
    void InitArmor(int _type)
    {
        if(_type == 3)
        {
            SetArmor(true);
        }
        else
        {
            SetArmor(false);
        }
    }
    public void SetArmor(bool _superarmor)
    {
        superArmor = _superarmor;
    }
    void InitHp()
    {
        hp = BaseData.baseHp;
    }
    void InitDef()
    {
        finalDef = BaseData.baseDef;
        ApplySynergyDef(GameManager.Instance.synergyManager.reduceMonsterDef);
    }
    void InitSpeed()
    {
        finalSpeed = BaseData.baseSpeed;
        ApplySynergySpeed(GameManager.Instance.synergyManager.reduceMonsterSpeed);
    }
    public void SetRewardMana(int _path) //몬스터별 마나 획득량 조절
    {
        if (_path == 994) 
        {
            rewardMp = ArtifactManager.Instance.hasArtifacts[16] ? 30f : 20f;//길가
        }
        else if (_path % 10 == 4)
        {
            rewardMp = ArtifactManager.Instance.hasArtifacts[16] ? 15f : 10f; //일반보스
        }
        else
        {
            rewardMp = ArtifactManager.Instance.hasArtifacts[16] ? 3f : 2f; // 잡몹
        }
    }

    void Die()
    {
        finalSpeed = 0;
    }
    public bool ReduceHp(int _num) // 사망 체크
    {
        hp -= _num;
        if(hp <= 0)
        {
            Die();
            return true;
        }
        return false;
    }
    public void ChangeDirection(int _direction)
    {
        direction= _direction;
    }

    public void ApplySynergyDef(float _synergy)
    {
        finalDef = (int)(BaseData.baseDef * (1 - _synergy));
        if (_synergy != 0f)
        {
            defEffect.Play();
        }
    }
    public void ApplySynergySpeed(float _synergy)
    {
        finalSpeed = BaseData.baseSpeed * (1 - _synergy);
        if (_synergy != 0f)
        {
            speedEffect.Play();
        }
    }
}
