using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject G1PlayerPrefab;
    public GameObject B1PlayerPrefab;

    public static GameObject playerInstance;


    private void Start()
    {
        LoadData();
    }

    public async void LoadData()
    {
        var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> {
          "Avatar"
        });

        string toggleName = string.Empty;

        if (playerData != null && playerData.ContainsKey("Avatar"))
        {
            toggleName = playerData["Avatar"].ToString();
        }




        switch (toggleName)
        {
            case "Toggle4":
                playerInstance = Instantiate(G1PlayerPrefab, Vector2.zero, Quaternion.identity);
                Debug.Log("prefab4");
                break;

            case "Toggle2":
                playerInstance = Instantiate(B1PlayerPrefab, Vector2.zero, Quaternion.identity);
                Debug.Log("prefab2");
                break;

            default:
                playerInstance = Instantiate(G1PlayerPrefab, Vector2.zero, Quaternion.identity);
                Debug.Log("default");
                break;
        }

    }

}