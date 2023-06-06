using System.Text.RegularExpressions;
using UnityEngine;

public class UnitDB : MonoBehaviour,ICSVRead
{
    public UnitSO[] UnitDataBase;
    public void Init()
    {
        ReadCSV("UnitData");
    }

    public void ReadCSV(string _file)
    {
        string[] lines = CSVReader.Line_Split(_file);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], CSVReader.SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;
            UnitDataBase[i - 1].SetPath(CSVReader.GetStringData(values[0]));
            UnitDataBase[i - 1].SetType(CSVReader.GetIntData(values[1]));
            UnitDataBase[i - 1].SetPower(CSVReader.GetIntData(values[2]));
            UnitDataBase[i - 1].SetCooltime(CSVReader.GetFloatData(values[3]));
            UnitDataBase[i - 1].SetRange(CSVReader.GetFloatData(values[4]));
            UnitDataBase[i - 1].SetStuntime(CSVReader.GetFloatData(values[5]));
            UnitDataBase[i - 1].SetSound(CSVReader.GetIntData(values[6]));
        }
    }
}
