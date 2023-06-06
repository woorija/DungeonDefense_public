using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbilityManager : MonoBehaviour //유닛 어빌리티 매니저
{
    int Vampire_killcount; // 뱀파이어로드 특수능력 카운팅

    //이벤트 관리
    UnityEvent E_Phantom_inc_damage;
    UnityEvent E_VampireLord_bonus_attack;

    //매니저 초기화
    public void Init()
    {
        Vampire_killcount = 0;

        E_Phantom_inc_damage = new UnityEvent();
        E_VampireLord_bonus_attack = new UnityEvent();
    }

    #region 팬텀 어빌리티
    public void Add_ghost_list(UnityAction _function)
    {
        E_Phantom_inc_damage.AddListener(_function);
    }

    public void Remove_ghost_list(UnityAction _function)
    {
        E_Phantom_inc_damage.RemoveListener(_function);
    }

    public void Kill_by_Ghost()
    {
        E_Phantom_inc_damage.Invoke();
    }
    #endregion

    #region 뱀파이어로드 어빌리티
    public void Add_VL_list(UnityAction _function)
    {
        E_VampireLord_bonus_attack.AddListener(_function);
    }

    public void Remove_VL_list(UnityAction _function)
    {
        E_VampireLord_bonus_attack.RemoveListener(_function);
    }

    public void Kill_by_Vampire()
    {
        Vampire_killcount++;
        if (Vampire_killcount >= have_VL_Artifact())
        {
            Vampire_killcount = 0;
            E_VampireLord_bonus_attack.Invoke();
        }
    }

    int have_VL_Artifact() //뱀로 유물 소유여부로 킬카운트 결정
    {
        return ArtifactManager.Instance.have_Artifact[12] ? 3 : 5;
    }
    #endregion
}
