using UnityEngine;
using System.Text.RegularExpressions;

public class CSVReader
{
    public static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))"; // 큰따옴표 정규식
    public static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r"; // 줄바꿈 정규식
    public static char[] TRIM_CHARS = { '\"' }; //큰따옴표체크

    public static string[] Line_Split(string _file)
    {
        TextAsset data = Resources.Load<TextAsset>("CSV/" + _file); //csv불러오기

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        return lines;
    }
    static string Data_Trim(string _data)
    {
        string value = _data;
        value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", ""); //공백제거
        value = value.Replace("<br>", "\n"); //줄바꿈 치환
        value = value.Replace("<c>", ","); // ,치환
        return value;
    }

    public static int GetIntData(string _data){
        string value = Data_Trim(_data);
        int n;
        if (int.TryParse(value, out n)) //int형변환
        {
            return n;
        }
        return 0;
    }

    public static float GetFloatData(string _data)
    {
        string value = Data_Trim(_data);
        float f;
        if (float.TryParse(value, out f)) //int형변환
        {
            return f;
        }
        return 0;
    }
    public static bool GetBoolData(string _data)
    {
        string value = Data_Trim(_data);
        bool b;
        if (bool.TryParse(value, out b)) //int형변환
        {
            return b;
        }
        return false;
    }

    public static string GetStringData(string _data)
    {
        return Data_Trim(_data);
    }
}