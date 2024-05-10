using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;
using UnityEngine;
using System.Threading.Tasks;
using Cinemachine;
using System;

using System.Collections;

using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.UI;



public class MapManager : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public GameObject virtualCamera;

    public static GameObject playerInstance;
    private int avatarNo;
    GameObject playerPrefab;

    public TMP_Text coinTextComponent;

    public TMP_Text timerText;
    public TMP_Text counterText;
    private int counter = 4;
    private float timer = 0f;
    private float interval = 1800f; // 30 minutes in seconds

    async void Awake()
    {
        //get the coin data
        GetBalancesOptions options = new GetBalancesOptions { ItemsPerFetch = 2, };

        GetBalancesResult getBalancesResult =
            await EconomyService.Instance.PlayerBalances.GetBalancesAsync(options);

        if (getBalancesResult.Balances.Count > 0)
        {

            //coin
            PlayerBalance coin = getBalancesResult.Balances[0];
            string coinText = coin.Balance.ToString();
            coinTextComponent.text = coinText;

            //energy
            //PlayerBalance energy = getBalancesResult.Balances[1];
            //string enemgyText = coin.Balance.ToString();
            //counterText.text = enemgyText;
        }
    }

    private async void Start()
    {
        if (playerPrefab == null) { await LoadData(); }
        Vector2 spawnPosition = (PlayerMovement.lastPosition == null) ? Vector2.zero : PlayerMovement.lastPosition;
        playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = playerInstance.transform;
    }

    private void Update()
    {
        if (counter < 5)
        {
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                timer = 0f;
                counter++;
                Debug.Log("enemgy add!");
            }
        }
        // 计算剩余时间
        float remainingTime = interval - timer;
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        string timeText = string.Format("{0:00}:{1:00}", minutes, seconds);

        timerText.text = timeText;
        counterText.text = counter.ToString() + "/5"; ;
    }

    public async Task LoadData()
    {
        try
        {
            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "Avatar" });
            if (playerData.ContainsKey("Avatar"))
            {
                Item item = playerData["Avatar"];
                if (!int.TryParse(item.Value.GetAsString(), out avatarNo)) { avatarNo = 0; }
                playerPrefab = playerPrefabs[avatarNo];
                Debug.Log("toggleName value: " + avatarNo);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
}