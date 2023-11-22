using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitBase : MonoBehaviour
{
    #region 변수
    protected UnitSO BaseData;
    protected UnitattackRange range; // 공격범위
    protected Uniteffect effect;
    protected UnityEvent<int> AttackEvent; //공격타입을 저장할 이벤트

    protected float tempCooltime; // 기본공속 + 유물효과 적용 공격속도
    protected float finalCooltime; // tempTimer 에 시너지를 적용 후 1초 단위로 조절한 최종 공격속도

    protected float attackSpeed; // 공격 애니메이션 속도

    protected float currentCooltime; // 쿨타임체크

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
        range = GetComponent<UnitattackRange>();
        effect = GetComponent<Uniteffect>();
        animator = GetComponent<Animator>();

        range.RangeInit(BaseData.baseRange); // 유닛 공격타일칸수
        currentCooltime = 0;

        animator.runtimeAnimatorController = ResourceManager.GetAnimator("Units/" + BaseData.path); //런타임 애니메이터 변경
        animator.SetFloat("MoveX", -1f);
        animator.SetFloat("MoveY", 0f);
    }
    protected virtual void EventInit()
    {
        GameManager.Instance.synergyManager.OnUnitAtkspdSynergyApply += ApplySynergyOption;
        ArtifactManager.Instance.onGetArtifact += ApplyOption;

        ApplyOption();
    }

    protected void ApplyOption() // 유물 -> 시너지 순으로 옵션 적용
    {
        ApplyArtifactOption();
        ApplySynergyOption(GameManager.Instance.synergyManager.increaseUnitAtkspd);
    }

    protected virtual void ApplyArtifactOption() //해당 옵션들은 유물효과로 옵션이 바뀔수 있기 때문에 init하위 함수로 변경해줌
    {
        damage = BaseData.baseAttackPower;
        tempCooltime = BaseData.baseAttackCooltime + 0; //여기서 실제 적용할 변수로 변경
    }

    protected void ApplySynergyOption(float _synergy)
    {
        float tempValue = finalCooltime < 0.01f ? 1 : finalCooltime;
        float temp = 1 / (tempCooltime * _synergy);
        bool effectplay = tempValue > temp;
        finalCooltime = temp;
        if(_synergy != 1.0f && effectplay)
        {
            effect.SynergyEffectPlay();
        }
        attackSpeed = Mathf.Max(1, 1 / finalCooltime);//애니메이션 플레이 최대1초 고정용
        animator.SetFloat("AttackSpeed", attackSpeed);
    }

    public virtual void UnitDelete()
    {
        //이벤트 제거
        GameManager.Instance.synergyManager.OnUnitAtkspdSynergyApply -= ApplySynergyOption;
        ArtifactManager.Instance.onGetArtifact -= ApplyOption;
        monsters.Clear();
        PoolManager.Instance.ReturnUnit(gameObject);
        Destroy(this); //다음 유닛을 위해 스크립트 제거
    }

    protected virtual void Update() // 타이머 적용
    {
        currentCooltime += Time.deltaTime;
        if (currentCooltime >= finalCooltime) //쿨타임이 지났고
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
        currentCooltime = 0;
    }
    protected void AttackAllMonster()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            AttackEvent.Invoke(i);
        }
        SoundManager.Instance.PlayAttackSfx(BaseData.attackSoundNumber); //공격사운드 실행
    }

    protected void AttackOneMonster()
    {
        if (monsters.Count == 0) // 공격 전에 몬스터가 빠져나가는 경우를 예외처리
        {
            return;
        }
        DistanceCheck(); // 최단거리몬스터 다시 체크

        AttackEvent.Invoke(shortestDistanceMonsterIndex);

        SoundManager.Instance.PlayAttackSfx(BaseData.attackSoundNumber); //공격사운드 실행
    }
    protected void AttackMonster(int _index)
    {
        monsters[_index].HitToNormal(damage, BaseData.type, BaseData.attackSoundNumber);
        monsters[_index].Stun(BaseData.stunTime);
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
