using UnityEngine;
using TMPro;
using System.Collections.Generic;


public class GameStartManager : MonoBehaviour
{
    public string gameName;
    public static string bestRecord;
    public static bool isInfinite;
    public static int lastDifficulty;
    public int skillNo, skillLevel;
    private static TMP_Text bestRecordText;
    private static List<float> bestRecordList;

    public static void SetGameMode(int gameMode)
    {
        lastDifficulty = gameMode;
        isInfinite = gameMode == 4;
        bestRecord = $"{ bestRecordList[gameMode] }%";
        bestRecordText.text = bestRecord;
    }

    void Awake()
    {
        bestRecordList = new List<float>();
        for (int i = 0; i < 5; i++)
        {
            bestRecordList.Add(100 - i * 1.01f);
        }
        bestRecordText = GameObject.Find("BestRecordText").GetComponent<TMP_Text>();
    }
    
    void Update()
    {
        //Debug.Log(lastDifficulty);
    }

}
