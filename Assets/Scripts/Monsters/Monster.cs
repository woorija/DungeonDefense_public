using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterStatus status { get; private set; }
    [SerializeField] MonsterHPbar Hpbar;

    int dieSoundNum;
    int effectYpos;

    CircleCollider2D hitbox;
    Animator animator;

    float stunnedTime; // 몬스터 경직 시간
    bool isDie;

    int direction;
    int currentWaypointNumber;
    
    Coroutine DieCheckCoroutine;

    List<GameObject> effectList;

    private void Awake()
    {
        status = GetComponent<MonsterStatus>();
        hitbox = GetComponent<CircleCollider2D>();
        animator = GetComponentInChildren<Animator>();
        effectList = new List<GameObject>();
    }

    #region DataInit
    public void Init(int _type,int _path)
    {
        status.Init(_type);
        stunnedTime = 0;
        currentWaypointNumber = 0;
        transform.position = Waypoints.Instance.waypoints[0].transform.position; // 위치 초기값
        //이벤트 등록
        GameManager.Instance.synergyManager.OnMonsterDefSynergyApply += status.ApplySynergyDef;
        GameManager.Instance.synergyManager.OnMonsterSpeedSynergyApply += status.ApplySynergySpeed;
        GameManager.Instance.stageManager.onGameReset += RemoveMonsterEvent;

        animator.runtimeAnimatorController = ResourceManager.GetAnimator("Monsters/" + DataBase.MonsterPathDB[_path]);


        status.SetRewardMana(_path);
        SetData(_path); //이펙트좌표,사망사운드 설정

        Hpbar.SetOnOff(true);
        Hpbar.UIInit(status.hp);
        Hpbar.YposInit(_path); //hpbar좌표 설정

        hitbox.offset = Vector2.zero;

        isDie = false;
        DieCheckCoroutine = null;
    }

    void SetData(int _path)
    {
        switch (_path)
        {
            case 11: //클레오 잡몹
            case 12:
            case 13:
                dieSoundNum = 0;
                effectYpos = 60;
                break;
            case 21: // 카드 잡몹
            case 22:
            case 23:
                dieSoundNum = 1;
                effectYpos = 40;
                break;
            case 31: // 셰어 잡몹
            case 32:
            case 33:
                dieSoundNum = 2;
                effectYpos = 60;
                break;
            case 994: // 길가
                dieSoundNum = 6;
                effectYpos = 70;
                status.SetArmor(true);
                break;
            case 14: //클레오
                dieSoundNum = 3;
                effectYpos = 70;
                status.SetArmor(true);
                break;
            case 24: // 카드
                dieSoundNum = 4;
                effectYpos = 70;
                status.SetArmor(true);
                break;
            case 34: // 셰어
                dieSoundNum = 5;
                effectYpos = 70;
                status.SetArmor(true);
                break;
            default:
                dieSoundNum = 0;
                effectYpos = 0;
                break;
        }
    }
    #endregion

    private void Update()
    {
        if (!isDie && stunnedTime <= 0f)
        {
            Move();
            if (status.direction != direction)
            {
                SetAnimater();
                direction = status.direction;
            }
        }
        else
        {
            stunnedTime -= Time.deltaTime;
            if(stunnedTime <= 0f)
            {
                animator.speed = 1f;
            }
        }
    }
    private void LateUpdate()
    {
        if (!isDie)
        {
            Hpbar.UIPosUpdata(transform.position); // hp바 위치적용
        }
    }
    void ChangeDirection() // 방향전환
    {
        status.ChangeDirection(Waypoints.Instance.waypoints[currentWaypointNumber].Get_Dir2way());
        currentWaypointNumber++;
        if (currentWaypointNumber >= Waypoints.Instance.waypoints.Length)
        {
            return;
        }
    }

    public void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, Waypoints.Instance.waypoints[currentWaypointNumber].transform.position, status.finalSpeed * 120 * Time.deltaTime);
        if (currentWaypointNumber < Waypoints.Instance.waypoints.Length - 1)
        {
            if (Mathf.Abs(Vector2.SqrMagnitude(transform.position - Waypoints.Instance.waypoints[currentWaypointNumber].transform.position)) <= 0.01f)
            {
                ChangeDirection();
            }
        }
    }
    void PositionReset()
    {
        currentWaypointNumber = 0;
        transform.position = Waypoints.Instance.waypoints[currentWaypointNumber].transform.position;
        status.ChangeDirection(-1);
    }

    void SetAnimater() //웨이포인트 방향에 따라 애니메이터 설정
    {
        switch (status.direction)
        {
            case 0:
                animator.SetFloat("MoveX", 0f);
                animator.SetFloat("MoveY", 1f);
                break;
            case 1:
                animator.SetFloat("MoveX", 0f);
                animator.SetFloat("MoveY", -1f);
                break;
            case 2:
                animator.SetFloat("MoveX", -1f);
                animator.SetFloat("MoveY", 0f);
                break;
            default://case 3
                animator.SetFloat("MoveX", 1f);
                animator.SetFloat("MoveY", 0f);
                break;
        }
        
    }
    int CalcDamage(int _damage)
    {
        return Mathf.Max((int)(_damage - (_damage * (status.finalDef / (status.finalDef + 1000f)))), 1);
    }
    void Hit(int _damage)
    {
        int damage = CalcDamage(_damage);
        isDie = status.ReduceHp(damage);
        Hpbar.UIUpdate(status.hp);
    }
    public void Stun(float _time)
    {
        if (status.superArmor) return;
        if (_time > 0.01f && _time > stunnedTime)
        {
            stunnedTime = _time;
            animator.speed = 0f;
        }
    }
    void PlayEffect(int _hiteffect)
    {
        if (_hiteffect < 12)
        {
            GameObject temp = EffectManager.Instance.EffectPlay(_hiteffect, transform, effectYpos);
            effectList.Add(temp);
        }
    }
    public void HitToNormal(int _damage,int _type, int _hiteffect) //아군타입에 따라 어빌리티 차이 체크, 피격시
    {
        Hit(_damage);
        PlayEffect(_hiteffect);
        if (isDie)
        {
            if (DieCheckCoroutine == null)
            {
                DieCheckCoroutine = StartCoroutine(Die());
                switch (_type) // 킬타입 체크
                {
                    case 1:
                        break;
                    case 2:
                        GameManager.Instance.abilityManager.OnGhostTypeKilled?.Invoke();
                        break;
                    case 3:
                        GameManager.Instance.abilityManager.KillByVampire();
                        break;
                    case 4:
                        break;
                }
            }
        }
    }

    public void HitToZombie(int _damage, int _hiteffect) // 시너지효과를 받은 좀비에게 피격시
    {
        Hit(_damage);
        PlayEffect(_hiteffect);
        if (isDie)
        {
            if (DieCheckCoroutine == null)
            {
                PoolManager.Instance.GetExplosion(transform.position);
                DieCheckCoroutine = StartCoroutine(Die());
            }
        }
    }

    public void HitToExplosion(float _damage) // 폭발데미지입을때
    {
        int damage = (int)(status.hp * _damage);
        isDie = status.ReduceHp(damage);
        Hpbar.UIUpdate(status.hp);
    }

    void DeleteEffect()
    {
        for(int i = 0; i < effectList.Count; i++)
        {
            if(effectList[i] != null)
            {
                Destroy(effectList[i]);
            }
        }
        effectList.Clear();
    }

    IEnumerator Die() // 몬스터 hp가 다 달아서 죽었을때
    {
        GameManager.Instance.moneyManager.GetMana(status.rewardMp); // 플레이어 마나 획득
        //이벤트 제거
        GameManager.Instance.synergyManager.OnMonsterDefSynergyApply -= status.ApplySynergyDef;
        GameManager.Instance.synergyManager.OnMonsterSpeedSynergyApply -= status.ApplySynergySpeed;
        GameManager.Instance.stageManager.onGameReset -= RemoveMonsterEvent;
        //사망사운드재생
        SoundManager.Instance.PlayDieSfx(dieSoundNum);
        //판정범위 제거
        hitbox.offset = new Vector2(3000, 3000);
        isDie = true;
        Hpbar.SetOnOff(false);
        animator.SetTrigger("isDie");
        yield return new WaitForSeconds(1f);
        animator.runtimeAnimatorController = null;
        DieCheckCoroutine = null;
        DeleteEffect();
        GameManager.Instance.stageManager.ReduceMonsterCount();
        PoolManager.Instance.ReturnMonster(this);
    }

    void Goalin() // 아크리치에 접근시
    {
        if (ArtifactManager.Instance.hasArtifacts[18])
        {
            PositionReset();
            ArtifactManager.Instance.UseMonsterExitArtifact();
            return;
        }
        UIManager.Instance.GameOver();
    }

    void RemoveMonsterEvent() // 리스타트 버튼을 통해 제거될때
    {
        animator.runtimeAnimatorController = null;
        isDie = true;
        GameManager.Instance.synergyManager.OnMonsterDefSynergyApply -= status.ApplySynergyDef;
        GameManager.Instance.synergyManager.OnMonsterSpeedSynergyApply -= status.ApplySynergySpeed;
        hitbox.offset = new Vector2(3000, 3000);
        GameManager.Instance.stageManager.onGameReset -= RemoveMonsterEvent;
        DeleteEffect();
        PoolManager.Instance.ReturnMonster(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WaypointEnd"))
        {
            Goalin();
        }
        else if (collision.CompareTag("Units"))
        {
            collision.GetComponentInParent<UnitBase>().AddList(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Units"))
        {
            UnitBase temp = collision.GetComponentInParent<UnitBase>();
            if (temp == null) return;
            temp.RemoveList(this);
        }
    }
}
