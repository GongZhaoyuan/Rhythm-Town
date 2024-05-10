using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;

public class GameEndManager : MonoBehaviour
{
    [SerializeField] string gameName;
    public TMP_Text accuracyText, bestRecordText;
    [SerializeField] List<GameObject> difficultyIcons;
    
    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    async void Start()
    {
        accuracyText.text = $"{ GameController.accuracy }%";
        if (GameController.accuracy > GameStartManager.bestRecord)
        {
            bestRecordText.text = $"{ GameController.accuracy }%";
            string[] recordLabels = {"Easy", "Normal", "Hard", "Expert", "Infinity"};
            var recordData = new Dictionary<string, object> {
                { $"Record_{gameName}_{recordLabels[GameStartManager.lastDifficulty]}", GameController.accuracy }
            };
            await CloudSaveService.Instance.Data.Player.SaveAsync(recordData);
        }
        else{
            bestRecordText.text = $"{ GameStartManager.bestRecord }%";
        }
        for (int i = 0; i < difficultyIcons.Count; i++)
        {
            difficultyIcons[i].SetActive(i == GameStartManager.lastDifficulty);
        }
    }
}
