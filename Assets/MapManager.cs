using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;
using UnityEngine;
using System.Threading.Tasks;
using Cinemachine;
using System;

public class MapManager : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public GameObject virtualCamera;

    public static GameObject playerInstance;
    private string toggleName;
    GameObject playerPrefab;

    private async void Start()
    {
        if (playerPrefab == null)
        {
            playerPrefab = playerPrefabs[0];
            await LoadData();
        }        
        Vector2 spawnPosition = (PlayerMovement.lastPosition == null) ? Vector2.zero : PlayerMovement.lastPosition;
        playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = playerInstance.transform;
    }

    public async Task LoadData()
    {
        try
        {
            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> {"Avatar"});
            if (playerData.ContainsKey("Avatar"))
            {
                Item item = playerData["Avatar"];
                toggleName = item.Value.GetAsString();
                Debug.Log("toggleName value: " + toggleName);            
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }

        switch (toggleName)
        {
            case "G1":
                playerPrefab = playerPrefabs[0];                
                Debug.Log("prefab4");
                break;

            case "B1":
                playerPrefab = playerPrefabs[1];
                Debug.Log("prefab2");
                break;

            default:
                playerPrefab = playerPrefabs[0];
                Debug.Log("default");
                break;
        }     
    }
}