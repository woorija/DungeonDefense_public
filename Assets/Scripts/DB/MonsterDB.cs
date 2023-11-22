using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
public struct MonsterData
{
    public float base_speed { get; private set; }
    public int base_hp { get; private set; }
    public int base_def { get; private set; }
    public MonsterData(float _spd, int _hp, int _def)
    {
        base_speed = _spd;
        base_hp = _hp;
        base_def = _def;
    }
}
public struct StageData
{
    //타입 순서 : normal | hp | speed | armor | random
    public int normaltype_count { get; private set; }
    public int hptype_count { get; private set; }
    public int speedtype_count { get; private set; }
    public int armortype_count { get; private set; }
    public int randomtype_count { get; private set; }
    public StageData(int _normal,int _hp, int _speed, int _armor, int _random)
    {
        normaltype_count = _normal;
        hptype_count = _hp;
        speedtype_count = _speed;
        armortype_count = _armor;
        randomtype_count = _random;
    }
}
public class MonsterDB : MonoBehaviour,ICSVRead
{
    Dictionary<int, MonsterData> MonsterBD;
    Dictionary<int, StageData> StageBD;

    public MonsterSO[] monsterdata;
    [HideInInspector] public List<int> monsterspawndata = new List<int>(16);

    public void SetData(int _wave)
    {
        SetMonsterData(_wave);
        SetSpawnData(_wave);
    }
    void SetMonsterData(int _wave)//웨이브마다 몬스터 능력치 재설정
    {
        //노말타입 능력치 배율 1 1 1
        monsterdata[0].SetHp(MonsterBD[_wave].base_hp);
        monsterdata[0].SetSpeed(MonsterBD[_wave].base_speed);
        monsterdata[0].SetDef(MonsterBD[_wave].base_def);
        //HP타입 능력치 배율 2 0.8 1
        monsterdata[1].SetHp(MonsterBD[_wave].base_hp * 2);
        monsterdata[1].SetSpeed(MonsterBD[_wave].base_speed * 0.8f);
        monsterdata[1].SetDef(MonsterBD[_wave].base_def);
        //SPEED타입 능력치 배율 0.8 2 1
        monsterdata[2].SetHp((int)(MonsterBD[_wave].base_hp * 0.8f));
        monsterdata[2].SetSpeed(MonsterBD[_wave].base_speed * 2.0f);
        monsterdata[2].SetDef(MonsterBD[_wave].base_def);
        //ARMOR타입 능력치 배율 1 1 +500
        monsterdata[3].SetHp(MonsterBD[_wave].base_hp);
        monsterdata[3].SetSpeed(MonsterBD[_wave].base_speed);
        monsterdata[3].SetDef(MonsterBD[_wave].base_def + 500);
    }
    void SetSpawnData(int _wave)
    {
        monsterspawndata.Clear();
        for(int i = 0; i < StageBD[_wave].normaltype_count; i++)
        {
            monsterspawndata.Add(0);
        }
        for (int i = 0; i < StageBD[_wave].hptype_count; i++)
        {
            monsterspawndata.Add(1);
        }
        for (int i = 0; i < StageBD[_wave].speedtype_count; i++)
        {
            monsterspawndata.Add(2);
        }
        for (int i = 0; i < StageBD[_wave].armortype_count; i++)
        {
            monsterspawndata.Add(3);
        }
        for (int i = 0; i < StageBD[_wave].randomtype_count; i++)
        {
            monsterspawndata.Add(4);
        }
        Shuffle();
    }
    public bool ContainsStage(int _num)
    {
        return StageBD.ContainsKey(_num);
    }
    void Shuffle()
    {
        for (int index = 0; index < monsterspawndata.Count; index++)
        {
            int random_Index = Random.Range(index, monsterspawndata.Count);
            int temp = monsterspawndata[index];
            monsterspawndata[index] = monsterspawndata[random_Index];
            monsterspawndata[random_Index] = temp;
        }
    }
    public void Init()
    {
        MonsterBD = new Dictionary<int, MonsterData>(64);
        StageBD = new Dictionary<int, StageData>(64);
        switch (GameManager.Instance.gameMode)
        {
            case GameMode.Story:
                ReadCSV("StoryMonsterData");
                ReadCSV("StoryStageData");
                break;
            case GameMode.Infinity:
                ReadCSV("InfinityMonsterData");
                ReadCSV("InfinityStageData");
                break;
        }  
    }
    public void ReadCSV(string _file)
    {
        switch (_file)
        {
            case "StoryMonsterData":
            case "InfinityMonsterData":
                ReadMonsterCSV(_file);
                break;
            case "StoryStageData":
            case "InfinityStageData":
                ReadStageCSV(_file);
                break;
        }
        
    }
    void ReadMonsterCSV(string _file)
    {
        string[] lines = CSVReader.Line_Split(_file);
        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], CSVReader.SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;
            MonsterBD.Add(CSVReader.GetIntData(values[0]), new MonsterData(CSVReader.GetFloatData(values[2]), CSVReader.GetIntData(values[1]), CSVReader.GetIntData(values[3])));
        }
    }
    void ReadStageCSV(string _file)
    {
        string[] lines = CSVReader.Line_Split(_file);
        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], CSVReader.SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;
            StageBD.Add(CSVReader.GetIntData(values[0]), new StageData(CSVReader.GetIntData(values[1]), CSVReader.GetIntData(values[2]), CSVReader.GetIntData(values[3]), CSVReader.GetIntData(values[4]), CSVReader.GetIntData(values[5])));
        }
    }
}
