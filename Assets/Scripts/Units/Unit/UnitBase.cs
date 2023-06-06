using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitBase : MonoBehaviour
{
    #region 변수
    protected UnitSO BaseData;
    protected UnitattackRange _range; // 공격범위
    protected Uniteffect _effect;
    protected UnityEvent<int> AttackEvent; //공격타입을 저장할 이벤트

    protected float temp_cooltime; // 기본공속 + 유물효과 적용 공격속도
    protected float final_cooltime; // temp_timer 에 시너지를 적용 후 1초 단위로 조절한 최종 공격속도

    protected float attack_speed; // 공격 애니메이션 속도

    protected float current_cooltime; // 쿨타임체크

    protected int damage; // 최종공격력

    protected List<Monster> monsters; // 몬스터리스트 작성
    protected int shortestDistanceMonsterIndex; //가장 가까운몹

    protected Animator animator; // 유닛애니메이터
    #endregion


    public virtual void UnitInit(int _num)
    {
        DataInit(_num);
        EventInit();
    }
    protected virtual void DataInit(int _num)
    {
        BaseData = DataBase.Instance.unitDB.UnitDataBase[_num];
        monsters = new List<Monster>(); //타깃 리스트
        AttackEvent = new UnityEvent<int>();
        _range = GetComponent<UnitattackRange>();
        _effect = GetComponent<Uniteffect>();
        animator = GetComponent<Animator>();

        _range.RangeInit(BaseData.base_range); // 유닛 공격타일칸수
        current_cooltime = 0;

        animator.runtimeAnimatorController = ResourceManager.Get_Animator("Units/" + BaseData.path); //런타임 애니메이터 변경
        animator.SetFloat("MoveX", -1f);
        animator.SetFloat("MoveY", 0f);
    }
    protected virtual void EventInit()
    {
        GameManager.Instance.Synergy_manager.Add_UnitSynergy_atkspd_list(Apple_SynergyOption); //시너지 이벤트함수등록
        ArtifactManager.Instance.AddApply_list(ApplyOption); //유물 이벤트함수등록

        ApplyOption();
    }

    protected void ApplyOption() // 유물 -> 시너지 순으로 옵션 적용
    {
        Apply_ArtifactOption();
        Apple_SynergyOption(GameManager.Instance.Synergy_manager.unit_increase_atkspd);
    }

    protected virtual void Apply_ArtifactOption() //해당 옵션들은 유물효과로 옵션이 바뀔수 있기 때문에 init하위 함수로 변경해줌
    {
        damage = BaseData.base_attackpower;
        temp_cooltime = BaseData.base_attackcooltime + 0; //여기서 실제 적용할 변수로 변경
    }

    protected void Apple_SynergyOption(float _synergy)
    {
        float temp_value = final_cooltime < 0.01f ? 1 : final_cooltime;
        float temp = 1 / (temp_cooltime * _synergy);
        bool effectplay = temp_value > temp;
        final_cooltime = temp;
        if(_synergy != 1.0f && effectplay)
        {
            _effect.SynergyEffectPlay();
        }
        attack_speed = Mathf.Max(1, 1 / final_cooltime);//애니메이션 플레이 최대1초 고정용
        animator.SetFloat("AttackSpeed", attack_speed);
    }

    public virtual void UnitDelete()
    {
        //이벤트 제거
        GameManager.Instance.Synergy_manager.Remove_UnitSynergy_atkspd_list(Apple_SynergyOption);
        ArtifactManager.Instance.RemoveApply_list(ApplyOption);
        monsters.Clear();
        PoolManager.Instance.ReturnUnit(gameObject);
        Destroy(this); //다음 유닛을 위해 스크립트 제거
    }

    protected virtual void Update() // 타이머 적용
    {
        current_cooltime += Time.deltaTime;
        if (current_cooltime >= final_cooltime) //쿨타임이 지났고
        {
            if (monsters.Count != 0) // 몬스터가 1마리 이상일때
            {
                Attack();
            }
        }
    }

    #region 몬스터체크
    public void AddList(Monster temp) // 몬스터에서 리스트 관여
    {
        monsters.Add(temp);
    }

    public void RemoveList(Monster temp) // 몬스터에서 리스트 관여
    {
        monsters.Remove(temp);
    }
    #endregion
    protected virtual void PlayAttackAnimation() // 2방향 체크
    {
        if (transform.position.x < monsters[shortestDistanceMonsterIndex].transform.position.x)
        {
            animator.SetFloat("MoveX", 1f);
            animator.SetFloat("MoveY", 0f);
        }
        else
        {
            animator.SetFloat("MoveX", -1f);
            animator.SetFloat("MoveY", 0f);
        }
        animator.SetTrigger("isAttack"); //애니메이션 변경
    }

    protected virtual void Attack()
    {
        DistanceCheck();
        PlayAttackAnimation();
        current_cooltime = 0;
    }
    protected void Attack_All_Monster()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            AttackEvent.Invoke(i);
        }
        SoundManager.Instance.PlayAttackSfx(BaseData.attacksound_number); //공격사운드 실행
    }

    protected void Attack_One_Monster()
    {
        if (monsters.Count == 0) // 공격 전에 몬스터가 빠져나가는 경우를 예외처리
        {
            return;
        }
        DistanceCheck(); // 최단거리몬스터 다시 체크

        AttackEvent.Invoke(shortestDistanceMonsterIndex);

        SoundManager.Instance.PlayAttackSfx(BaseData.attacksound_number); //공격사운드 실행
    }
    protected void AttackMonster(int _index)
    {
        monsters[_index].Hit_to_normal(damage, BaseData.type, BaseData.attacksound_number);
        monsters[_index].stun(BaseData.stuntime);
    }
    protected void DistanceCheck()
    {
        if(monsters.Count == 1)
        {
            shortestDistanceMonsterIndex = 0;
        }
        else
        {
            float shortdistance = 9999999;
            for (int i = 0; i < monsters.Count; i++) // 가까운몹 체크
            {
                float distance = Vector2.SqrMagnitude(gameObject.transform.position - monsters[i].transform.position);
                if (distance < shortdistance)
                {
                    shortdistance = distance;
                    shortestDistanceMonsterIndex = i;
                }
            }
        }
    }

    protected virtual void SpecialAbility() // 3티어 특능
    {

    }

}
