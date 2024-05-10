using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;
using System.Threading.Tasks;
using Cinemachine;
using System;

using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.UI;


public class economySystem : MonoBehaviour
{

    public TMP_Text coinTextComponent;

    public TMP_Text timerText;
    public TMP_Text enemgyTexComonent;

    public TMP_Text levelTextComponent;


    private float timer = 0f;
    private float interval = 600f; // 30 minutes in seconds

    int currentEXP;

    int currentEnergy;

    public int maxEXP = 100;
    // Start is called before the first frame update

    async void Awake()
    {
        //get the coin data
        GetBalancesOptions options = new GetBalancesOptions { ItemsPerFetch = 4, };

        GetBalancesResult getBalancesResult =
            await EconomyService.Instance.PlayerBalances.GetBalancesAsync(options);

        if (getBalancesResult.Balances.Count > 0)
        {

            //coin
            PlayerBalance coin = getBalancesResult.Balances[0];
            string coinText = coin.Balance.ToString();
            coinTextComponent.text = coinText;

            //energy
            PlayerBalance energy = getBalancesResult.Balances[1];
            string enemgyText = energy.Balance.ToString();
            currentEnergy = int.Parse(enemgyText);


            //exp
            PlayerBalance exp = getBalancesResult.Balances[2];
            string expText = exp.Balance.ToString();
            currentEXP = int.Parse(expText);
            MaskController.instance.SetValue(currentEXP / (float)maxEXP);

            //level
            PlayerBalance level = getBalancesResult.Balances[3];
            string levelText = level.Balance.ToString();
            levelTextComponent.text = levelText;
        }

    }
    private async void Start()
    {

        string currencyID = "EXP";

        PlayerBalance newBalance =
            await EconomyService.Instance.PlayerBalances.IncrementBalanceAsync(
                currencyID,
                5
            );
    }

    // Update is called once per frame
    private async void Update()
    {
        if (currentEnergy < 5)
        {
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                timer = 0f;
                string currencyID = "ENERGY";

                PlayerBalance newBalance =
                    await EconomyService.Instance.PlayerBalances.IncrementBalanceAsync(
                        currencyID,
                        1
                    );
            }
        }
        // 计算剩余时间
        float remainingTime = interval - timer;
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        string timeText = string.Format("{0:00}:{1:00}", minutes, seconds);

        timerText.text = timeText;

        GetBalancesOptions options = new GetBalancesOptions { ItemsPerFetch = 4, };

        GetBalancesResult getBalancesResult =
            await EconomyService.Instance.PlayerBalances.GetBalancesAsync(options);

        if (getBalancesResult.Balances.Count > 0)
        {
            //energy
            PlayerBalance energy = getBalancesResult.Balances[1];
            string enemgyText = energy.Balance.ToString();

            enemgyTexComonent.text = enemgyText + "/5";

        }

    }
    public async void addEXP(int amount)
    {

        string currencyID = "EXP";
        string currencyLevel = "LEVEL";

        GetBalancesOptions options = new GetBalancesOptions { ItemsPerFetch = 4, };

        GetBalancesResult getBalancesResult =
            await EconomyService.Instance.PlayerBalances.GetBalancesAsync(options);

        PlayerBalance exp = getBalancesResult.Balances[2];
        string expText = exp.Balance.ToString();
        currentEXP = int.Parse(expText);

        if (currentEXP + amount > 100)
        {
            PlayerBalance newBalance =
                        await EconomyService.Instance.PlayerBalances.IncrementBalanceAsync(
                            currencyLevel,
                            1
                        );
            PlayerBalance EXPnewBalance = await EconomyService.Instance.PlayerBalances.SetBalanceAsync(currencyID, 0);
        }
        else
        {
            PlayerBalance newBalance =
                        await EconomyService.Instance.PlayerBalances.IncrementBalanceAsync(
                            currencyID,
                            amount
                        );
        }

    }


    public async void addCoin(int amount)
    {

        string currencyID = "COINS";

        PlayerBalance newBalance =
            await EconomyService.Instance.PlayerBalances.IncrementBalanceAsync(
                currencyID,
                amount
            );
    }
}
