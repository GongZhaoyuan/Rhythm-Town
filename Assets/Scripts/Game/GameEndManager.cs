using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using Unity.Services.Economy;
using UnityEngine;

public class GameEndManager : MonoBehaviour
{
    [SerializeField] string gameName;
    [SerializeField] TMP_Text accuracyText, bestRecordText, coinText, expText;
    [SerializeField] List<GameObject> difficultyIcons;
    
    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    async void Start()
    {
        accuracyText.text = $"{ GameController.accuracy }" + (GameStartManager.isInfinite? "" : "%");
        if (GameController.accuracy > GameStartManager.bestRecord)
        {
            bestRecordText.text = $"{ GameController.accuracy }" + (GameStartManager.isInfinite? "" : "%");
            GameStartManager.bestRecordList[GameStartManager.lastDifficulty] = GameController.accuracy;
            string[] recordLabels = {"Easy", "Normal", "Hard", "Expert", "Infinite"};
            var recordData = new Dictionary<string, object> {
                { $"Record_{gameName}_{recordLabels[GameStartManager.lastDifficulty]}", GameController.accuracy }
            };
            await CloudSaveService.Instance.Data.Player.SaveAsync(recordData);
            Award("EXP", 10);
        }
        else{
            bestRecordText.text = $"{ GameStartManager.bestRecord }" + (GameStartManager.isInfinite? "" : "%");
            Award("EXP", 1);
        }
        for (int i = 0; i < difficultyIcons.Count; i++)
        {
            difficultyIcons[i].SetActive(i == GameStartManager.lastDifficulty);
        }
        Award("COINS", 100);
    }

    public async void Award(string currencyID, int amount)
    {
        await EconomyService.Instance.PlayerBalances.IncrementBalanceAsync(
            currencyID,
            amount
        ); 
    }

    // public async void GainCoin(int amount)
    // {

    //     string currencyID = "COINS";

    //     PlayerBalance newBalance =
    //         await EconomyService.Instance.PlayerBalances.IncrementBalanceAsync(
    //             currencyID,
    //             amount
    //         );
    // }
}
