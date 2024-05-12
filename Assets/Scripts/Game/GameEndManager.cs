using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using Unity.Services.Economy;
using UnityEngine;
using UnityEngine.UI;

public class GameEndManager : MonoBehaviour
{
    [SerializeField] string gameName;
    [SerializeField] TMP_Text accuracyText, bestRecordText, coinText, expText;
    [SerializeField] List<GameObject> difficultyIcons;
    [SerializeField] List<Image> starIcons;
    int coinAwarded, expAwarded, energyConsumed;
    
    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        coinAwarded = 100;
        energyConsumed = 10;
    }

    async void Start()
    {
        accuracyText.text = $"{ GameController.accuracy }" + (GameStartManager.isInfinite? "" : "%");
        if (GameController.accuracy > GameStartManager.bestRecord)
        {
            GameStartManager.bestRecordList[GameStartManager.lastDifficulty] = GameController.accuracy;
            bestRecordText.text = $"{ GameController.accuracy }" + (GameStartManager.isInfinite? "" : "%");
            GameStartManager.bestRecordList[GameStartManager.lastDifficulty] = GameController.accuracy;
            string[] recordLabels = {"Easy", "Normal", "Hard", "Expert", "Infinite"};
            var recordData = new Dictionary<string, object> {
                { $"Record_{gameName}_{recordLabels[GameStartManager.lastDifficulty]}", GameController.accuracy }
            };
            await CloudSaveService.Instance.Data.Player.SaveAsync(recordData);
            expAwarded = 10;
        }
        else{
            bestRecordText.text = $"{ GameStartManager.bestRecord }" + (GameStartManager.isInfinite? "" : "%");
            expAwarded = 1;
        }
        
        for (int i = 0; i < starIcons.Count; i ++)
            starIcons[i].sprite = Resources.Load<Sprite>("Stars/Star " + (GameStartManager.bestRecordList[i] < (i < 4? 60 : 1000)? "Gray" :
                GameStartManager.bestRecordList[i] < (i < 4? 90 : 1500)? "Yellow" : "Pink"));

        for (int i = 0; i < difficultyIcons.Count; i++)
        {
            difficultyIcons[i].SetActive(i == GameStartManager.lastDifficulty);
        }
        switch (GameStartManager.skillType)
        {
            case "Coin":
                coinAwarded += GameStartManager.skillLevel * 5;
                break;

            case "Energy":
                energyConsumed -= GameStartManager.skillLevel;
                break;
            
            default:
                break;
        }
        
        coinText.text = $"<size=80%>Coin\n<size=100%>+ {coinAwarded}";
        expText.text = $"<size=80%>Sense of Rhythm\n<size=100%>+ {expAwarded}";
        Award("COINS", coinAwarded);
        Award("EXP", expAwarded);
        ConsumeEnergy(energyConsumed);
    }

    public async void Award(string currencyID, int amount)
    {
        await EconomyService.Instance.PlayerBalances.IncrementBalanceAsync(
            currencyID,
            amount
        ); 
    }

    public async void ConsumeEnergy(int amount)
    {
        await EconomyService.Instance.PlayerBalances.DecrementBalanceAsync(
            "ENERGY",
            amount
        ); 
    }
}
