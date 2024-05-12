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
    public static List<float> bestRecordList = new List<float> {-1, -1, -1, -1, -1};
    [SerializeField] Button multiplayerButton;
    [SerializeField] List<Image> starIcons;
    Dictionary<string, New_BookManager.BookData> skillsDict;
    public static string skillType;

    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    async void Start()
    {
        await LoadProp();
        foreach (float record in bestRecordList)
        {
            if (record < 0)
            {
                await LoadRecord();
                break;
            }
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
        bestRecordText.text = bestRecord == -1 ? "Loading...": bestRecord == 0? "None" : ($"{bestRecord}" + (isInfinite? "" : "%"));
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

    async Task LoadRecord()
    {
        try
        {
            string[] recordLabels = {"Easy", "Normal", "Hard", "Expert", "Infinite"};
            for (int i = 0; i < recordLabels.Length; i++)
                recordLabels[i] = $"Record_{gameName}_{recordLabels[i]}";
            var recordData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string>(recordLabels));
            for (int i = 0; i < recordLabels.Length; i++)
            {
                bestRecordList[i] = 0;
                if (recordData.TryGetValue(recordLabels[i], out var record))
                {                        
                    if (float.TryParse(record.Value.GetAsString(), out float recordValue))
                        bestRecordList[i] = recordValue;
                }
                starIcons[i].sprite = Resources.Load<Sprite>("Stars/Star " + (bestRecordList[i] < (i < 4? 60 : 2000)? "Gray" :
                    bestRecordList[i] < (i < 4? 90 : 3000)? "Yellow" : "Pink"));
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Record Exception: "+ex);
        }
    }
}