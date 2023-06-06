using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MonsterStatus : MonoBehaviour
{
    public MonsterSO BaseData { get; private set; }

    [Header("effects")]
    [SerializeField] ParticleSystem def_effect;
    [SerializeField] ParticleSystem speed_effect;
    [SerializeField] SpriteRenderer monster_type;

    public int final_def { get; private set; }
    public float final_speed { get; private set; }
    public int hp { get; private set; }
    public float reward_mp { get; private set; }
    public int direction { get; private set; }
    public bool superarmor { get; private set; }
    public void Init(int _type)
    {
        Init_type(_type);
        Init_hp();
        Init_def();
        Init_speed();
        direction = -1;
    }
    void Init_type(int _type)
    {
        if(_type == 4)//랜덤타입이면
        {
            int rand = Random.Range(1, 4);//노멀타입제외 나머지 타입중 랜덤
            BaseData = DataBase.Instance.monsterDB.monsterdata[rand];
            monster_type.material = DataBase.Instance.MonsterMaterials[rand];
            Init_armor(rand);
        }
        else
        {
            BaseData = DataBase.Instance.monsterDB.monsterdata[_type];
            monster_type.material = DataBase.Instance.MonsterMaterials[_type];
            Init_armor(_type);
        }
    }
    void Init_armor(int _type)
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
        superarmor = _superarmor;
    }
    void Init_hp()
    {
        hp = BaseData.base_hp;
    }
    void Init_def()
    {
        final_def = BaseData.base_def;
        Synergy_apply_def(GameManager.Instance.Synergy_manager.monster_reduce_def);
    }
    void Init_speed()
    {
        final_speed = BaseData.base_speed;
        Synergy_apply_speed(GameManager.Instance.Synergy_manager.monster_reduce_speed);
    }
    public void SetRewardMana(int _path) //몬스터별 마나 획득량 조절
    {
        if (_path == 994) 
        {
            reward_mp = ArtifactManager.Instance.have_Artifact[16] ? 30f : 20f;//길가
        }
        else if (_path % 10 == 4)
        {
            reward_mp = ArtifactManager.Instance.have_Artifact[16] ? 15f : 10f; //일반보스
        }
        else
        {
            reward_mp = ArtifactManager.Instance.have_Artifact[16] ? 3f : 2f; // 잡몹
        }
    }

    void Die()
    {
        final_speed = 0;
    }
    public bool Reduce_HP(int _num) // 사망 체크
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

    public void Synergy_apply_def(float _synergy)
    {
        final_def = (int)(BaseData.base_def * (1 - _synergy));
        if (_synergy != 0f)
        {
            def_effect.Play();
        }
    }
    public void Synergy_apply_speed(float _synergy)
    {
        final_speed = BaseData.base_speed * (1 - _synergy);
        if (_synergy != 0f)
        {
            speed_effect.Play();
        }
    }
}
