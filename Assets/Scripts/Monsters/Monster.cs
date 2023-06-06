using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterStatus status { get; private set; }
    [SerializeField] MonsterHPbar Hpbar;

    int die_sound_num;
    int effect_ypos;

    CircleCollider2D hitbox;
    Animator animator;

    float stunned_time; // 몬스터 경직 시간
    bool isDie;

    int direction;
    int current_waypoint_number;
    
    Coroutine Die_check;

    List<GameObject> effect_list;

    private void Awake()
    {
        status = GetComponent<MonsterStatus>();
        hitbox = GetComponent<CircleCollider2D>();
        animator = GetComponentInChildren<Animator>();
        effect_list = new List<GameObject>();
    }

    #region DataInit
    public void Init(int _type,int _path)
    {
        status.Init(_type);
        stunned_time = 0;
        current_waypoint_number = 0;
        transform.position = Waypoints.Instance.waypoints[0].transform.position; // 위치 초기값
        //이벤트 등록
        GameManager.Instance.Synergy_manager.Add_MonsterSynergy_def_list(status.Synergy_apply_def);
        GameManager.Instance.Synergy_manager.Add_MonsterSynergy_speed_list(status.Synergy_apply_speed);
        GameManager.Instance.Stage_manager.Add_MonsterList(RemoveMonsterEvent);

        animator.runtimeAnimatorController = ResourceManager.Get_Animator("Monsters/" + DataBase.MonsterPathDB[_path]);


        status.SetRewardMana(_path);
        SetData(_path); //이펙트좌표,사망사운드 설정

        Hpbar.SetOnOff(true);
        Hpbar.UIInit(status.hp);
        Hpbar.ypos_Init(_path); //hpbar좌표 설정

        hitbox.offset = Vector2.zero;

        isDie = false;
        Die_check = null;
    }

    void SetData(int _path)
    {
        switch (_path)
        {
            case 11: //클레오 잡몹
            case 12:
            case 13:
                die_sound_num = 0;
                effect_ypos = 60;
                break;
            case 21: // 카드 잡몹
            case 22:
            case 23:
                die_sound_num = 1;
                effect_ypos = 40;
                break;
            case 31: // 셰어 잡몹
            case 32:
            case 33:
                die_sound_num = 2;
                effect_ypos = 60;
                break;
            case 994: // 길가
                die_sound_num = 6;
                effect_ypos = 70;
                status.SetArmor(true);
                break;
            case 14: //클레오
                die_sound_num = 3;
                effect_ypos = 70;
                status.SetArmor(true);
                break;
            case 24: // 카드
                die_sound_num = 4;
                effect_ypos = 70;
                status.SetArmor(true);
                break;
            case 34: // 셰어
                die_sound_num = 5;
                effect_ypos = 70;
                status.SetArmor(true);
                break;
            default:
                die_sound_num = 0;
                effect_ypos = 0;
                break;
        }
    }
    #endregion

    private void Update()
    {
        if (!isDie && stunned_time <= 0f)
        {
            Move();
            if (status.direction != direction)
            {
                Set_animater();
                direction = status.direction;
            }
        }
        else
        {
            stunned_time -= Time.deltaTime;
            if(stunned_time <= 0f)
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
    void change_direction() // 방향전환
    {
        status.ChangeDirection(Waypoints.Instance.waypoints[current_waypoint_number].Get_Dir2way());
        current_waypoint_number++;
        if (current_waypoint_number >= Waypoints.Instance.waypoints.Length)
        {
            return;
        }
    }

    public void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, Waypoints.Instance.waypoints[current_waypoint_number].transform.position, status.final_speed * 120 * Time.deltaTime);
        if (current_waypoint_number < Waypoints.Instance.waypoints.Length - 1)
        {
            if (Mathf.Abs(Vector2.SqrMagnitude(transform.position - Waypoints.Instance.waypoints[current_waypoint_number].transform.position)) <= 0.01f)
            {
                change_direction();
            }
        }
    }
    void PositionReset()
    {
        current_waypoint_number = 0;
        transform.position = Waypoints.Instance.waypoints[current_waypoint_number].transform.position;
        status.ChangeDirection(-1);
    }

    void Set_animater() //웨이포인트 방향에 따라 애니메이터 설정
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
    int Calc_damage(int _damage)
    {
        return Mathf.Max((int)(_damage - (_damage * (status.final_def / (status.final_def + 1000f)))), 1);
    }
    void Hit(int _damage)
    {
        int damage = Calc_damage(_damage);
        isDie = status.Reduce_HP(damage);
        Hpbar.UIUpdate(status.hp);
    }
    public void stun(float _time)
    {
        if (status.superarmor) return;
        if (_time > 0.01f && _time > stunned_time)
        {
            stunned_time = _time;
            animator.speed = 0f;
        }
    }
    void PlayEffect(int _hiteffect)
    {
        if (_hiteffect < 12)
        {
            GameObject temp = EffectManager.Instance.EffectPlay(_hiteffect, transform, effect_ypos);
            effect_list.Add(temp);
        }
    }
    public void Hit_to_normal(int _damage,int _type, int _hiteffect) //아군타입에 따라 어빌리티 차이 체크, 피격시
    {
        Hit(_damage);
        PlayEffect(_hiteffect);
        if (isDie)
        {
            if (Die_check == null)
            {
                Die_check = StartCoroutine(Die());
                switch (_type) // 킬타입 체크
                {
                    case 1:
                        break;
                    case 2:
                        GameManager.Instance.Ability_manager.Kill_by_Ghost();
                        break;
                    case 3:
                        GameManager.Instance.Ability_manager.Kill_by_Vampire();
                        break;
                    case 4:
                        break;
                }
            }
        }
    }

    public void Hit_to_Zombie(int _damage, int _hiteffect) // 시너지효과를 받은 좀비에게 피격시
    {
        Hit(_damage);
        PlayEffect(_hiteffect);
        if (isDie)
        {
            if (Die_check == null)
            {
                PoolManager.Instance.GetExplosion(transform.position);
                Die_check = StartCoroutine(Die());
            }
        }
    }

    public void Hit_to_explosion(float _damage) // 폭발데미지입을때
    {
        int damage = (int)(status.hp * _damage);
        isDie = status.Reduce_HP(damage);
        Hpbar.UIUpdate(status.hp);
    }

    void delete_effect()
    {
        for(int i = 0; i < effect_list.Count; i++)
        {
            if(effect_list[i] != null)
            {
                Destroy(effect_list[i]);
            }
        }
        effect_list.Clear();
    }

    IEnumerator Die() // 몬스터 hp가 다 달아서 죽었을때
    {
        GameManager.Instance.Money_manager.Get_mana(status.reward_mp); // 플레이어 마나 획득
        //이벤트 제거
        GameManager.Instance.Synergy_manager.Remove_MonsterSynergy_def_list(status.Synergy_apply_def);
        GameManager.Instance.Synergy_manager.Remove_MonsterSynergy_speed_list(status.Synergy_apply_speed);
        GameManager.Instance.Stage_manager.Remove_MonsterList(RemoveMonsterEvent);
        //사망사운드재생
        SoundManager.Instance.PlayDieSfx(die_sound_num);
        //판정범위 제거
        hitbox.offset = new Vector2(3000, 3000);
        isDie = true;
        Hpbar.SetOnOff(false);
        animator.SetTrigger("isDie");
        yield return new WaitForSeconds(1f);
        animator.runtimeAnimatorController = null;
        Die_check = null;
        delete_effect();
        GameManager.Instance.Stage_manager.dec_monstercount();
        PoolManager.Instance.ReturnMonster(this);
    }

    void Goalin() // 아크리치에 접근시
    {
        if (ArtifactManager.Instance.have_Artifact[18])
        {
            PositionReset();
            ArtifactManager.Instance.Use_MonsterExit_Artifact();
            return;
        }
        UIManager.Instance.Gameover();
    }

    void RemoveMonsterEvent() // 리스타트 버튼을 통해 제거될때
    {
        animator.runtimeAnimatorController = null;
        isDie = true;
        GameManager.Instance.Synergy_manager.Remove_MonsterSynergy_def_list(status.Synergy_apply_def);
        GameManager.Instance.Synergy_manager.Remove_MonsterSynergy_speed_list(status.Synergy_apply_speed);
        hitbox.offset = new Vector2(3000, 3000);
        GameManager.Instance.Stage_manager.Remove_MonsterList(RemoveMonsterEvent);
        delete_effect();
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
