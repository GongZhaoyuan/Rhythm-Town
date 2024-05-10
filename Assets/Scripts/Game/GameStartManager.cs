using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using System;
using Unity.Services.Core;
using Unity.Services.Authentication;

public class GameStartManager : MonoBehaviour
{
    public string gameName;
    public static float bestRecord;
    public static bool isInfinite;
    public static int lastDifficulty;
    public int skillNo, skillLevel;
    private static TMP_Text bestRecordText;
    private static List<float> bestRecordList;
    [SerializeField] Button multiplayerButton;

    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    async void Start()
    {
        bestRecordText = GameObject.Find("BestRecordText").GetComponent<TMP_Text>();
        bestRecordList = new List<float>{0, 0, 0, 0, 0};
        try
        {
            string[] recordLabels = {"Easy", "Normal", "Hard", "Expert", "Infinity"};
            for (int i = 0; i < recordLabels.Length; i++)
            {
                recordLabels[i] = $"Record_{gameName}_{recordLabels[i]}";
            }
            var recordData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string>(recordLabels));
            for (int i = 0; i < recordLabels.Length; i++)
            {
                if (recordData.TryGetValue(recordLabels[i], out var record))
                {
                    if (float.TryParse(record.Value.GetAsString(), out float recordValue))
                        bestRecordList[i] = recordValue;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
    
    public void SetGameMode(int gameMode)
    {
        lastDifficulty = gameMode;
        isInfinite = gameMode == 4;
        bestRecord = bestRecordList[gameMode];
        bestRecordText.text = $"{bestRecord}%";
        multiplayerButton.interactable = !isInfinite;
    }
}