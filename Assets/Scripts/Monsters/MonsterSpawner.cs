using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterSpawner : MonoBehaviour
{
    Coroutine co_spawn; // 스폰코루틴관리함수
    bool IsBossWave;
    float spawntime = 1.3f;
    public int Get_Monstercount() // 웨이브별 몬스터 수 가져오기
    {
        return DataBase.Instance.monsterDB.monsterspawndata.Count;
    }
    void Spawn(int _num, int _type) // 몬스터 1마리를 데이터에 맞춰 스폰
    {
        Monster monster = PoolManager.Instance.GetMonster();
        monster.Init(_type, GameManager.Instance.Stage_manager.Get_current_monstercategory() * 10 + _num);
    }
    int SetMonsterType()
    {
        if (IsBossWave)
        {
            return 4;
        }
        else
        {
            return Random.Range(1, 4);
        }
    }
    public void SpawnStart(int _stage) // _stage = stage*100+wave
    {
        switch (GameManager.Instance.gamemode) // 보스웨이브 체크
        {
            case GameMode.Story:
                IsBosswave_story(_stage);
                break;
            case GameMode.Infinity:
                IsBosswave_infinity(_stage);
                break;
        }

        switch (_stage % 100) // 1웨이브는 추가 대기시간을 부여
        {
            case 1:
                co_spawn = StartCoroutine(SpawnStart(5.0f, spawntime));
                break;
            default:
                co_spawn = StartCoroutine(SpawnStart(2.0f, spawntime));
                break;
        }
    }
    void IsBosswave_story(int _stage)
    {
        if (_stage == 401)
        {
            IsBossWave = true;
        }
        else
        {
            switch (_stage % 100)
            {
                case 1:
                    IsBossWave = false;
                    break;
                case 4:
                    IsBossWave = true;
                    break;
            }
        }
    }
    void IsBosswave_infinity(int _stage)
    {
        if (_stage % 500 == 1) // 길가메시 스테이지 : 5 * x 스테이지
        {
            IsBossWave = true;
        }
        else if(_stage % 100 == 1) // 나머지 스테이지 1웨이브
        {
            IsBossWave = false;
        }
        else
        {
            int temp_bosswave = (_stage / 500) + 3;
            if(_stage % 100 == temp_bosswave)
            {
                IsBossWave = true;
            }
        }
    }

    public bool FindWave(int _wave) // 다음 스테이지로 넘어가기 위한 판별식
    {
        return DataBase.Instance.monsterDB.ContainsStage(_wave);
    }

    public void SpawnStop() // 스폰정지
    {
        if(co_spawn!=null)
            StopCoroutine(co_spawn);
    }

    IEnumerator SpawnStart(float _waittime, float _time) // _waittime초후 _stage스테이지웨이브몹을 _time초마다 소환
    {
        yield return YieldCache.WaitForSeconds(_waittime);
        for (int i = 0; i < DataBase.Instance.monsterDB.monsterspawndata.Count; i++)
        {
            Spawn(SetMonsterType(), DataBase.Instance.monsterDB.monsterspawndata[i]);
            yield return YieldCache.WaitForSeconds(_time);
        }
    }
}
