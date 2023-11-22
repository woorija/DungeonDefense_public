using System;
using UnityEngine;
using UnityEngine.Events;

public class AbilityManager : MonoBehaviour //유닛 어빌리티 매니저
{
    int VampireTypeKillcount; // 뱀파이어로드 특수능력 카운팅

    //이벤트 관리
    public Action OnGhostTypeKilled;
    public Action OnVampireTypeKilled;

    //매니저 초기화
    public void Init()
    {
        VampireTypeKillcount = 0;
    }
    public void KillByVampire()
    {
        VampireTypeKillcount++;
        if (VampireTypeKillcount >= HasVampireLordArtifact())
        {
            VampireTypeKillcount = 0;
            OnVampireTypeKilled?.Invoke();
        }
    }
    int HasVampireLordArtifact() //뱀로 유물 소유여부로 킬카운트 결정
    {
        return ArtifactManager.Instance.hasArtifacts[12] ? 3 : 5;
    }
}
