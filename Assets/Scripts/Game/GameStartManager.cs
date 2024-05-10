using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using System;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class GameStartManager : MonoBehaviour
{
    public string gameName;
    public static float bestRecord;
    public static bool isInfinite;
    public static int lastDifficulty, skillLevel;
    [SerializeField] TMP_Text bestRecordText;
    private static List<float> bestRecordList;
    [SerializeField] Button multiplayerButton;
    Dictionary<string, New_BookManager.BookData> skillsDict;
    public static string skillType;

    private async void Awake()
    {
        bestRecordList = new List<float> {0, 0, 0, 0, 0};
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Anonymous");
        }
    }

    async void Start()
    {
        await LoadProp();
        try
        {
            string[] recordLabels = {"Easy", "Normal", "Hard", "Expert", "Infinite"};
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
            Debug.Log("Record Exception: "+ex);
        }
        foreach (string skillLabel in skillsDict.Keys)
        {
            if (skillsDict[skillLabel].IsEquipped)
            {
                skillType = skillLabel;
                skillLevel = skillsDict[skillLabel].Level;
            }
        }
        Debug.Log($"{bestRecordList[1]}");
        Debug.Log($"{skillType}, Level {skillLevel}");
    }
    
    public void SetGameMode(int gameMode)
    {
        lastDifficulty = gameMode;
        isInfinite = gameMode == 4;
        bestRecord = bestRecordList[gameMode];
        bestRecordText.text = bestRecord == 0? "None" : $"{bestRecord}%";
        multiplayerButton.interactable = !isInfinite;
    }

    async Task LoadProp()
    {
        var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(
            new HashSet<string> { "objectLevelRecord_Cloud" }
        );

        if (playerData.ContainsKey("objectLevelRecord_Cloud"))
        {
            Item item = playerData["objectLevelRecord_Cloud"];
            skillsDict = item.Value.GetAs<Dictionary<string, New_BookManager.BookData>>();
            Debug.Log($"Dictionary Loaded:{skillsDict["Coin"].Level}");
        }
    }
}