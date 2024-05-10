using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ricimi;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

public class RelayManager : NetworkBehaviour
{
    public struct PlayerData : INetworkSerializable, IEquatable<PlayerData>
    {
        public int avatarID;
        public FixedString32Bytes nickname;
        public bool hasJoined, isReady;        

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref avatarID);
            serializer.SerializeValue(ref nickname);
            serializer.SerializeValue(ref isReady);
            serializer.SerializeValue(ref hasJoined);
        }

        public bool Equals(PlayerData other) { throw new NotImplementedException(); }
    }
    
    [SerializeField] TMP_InputField joinCodeInputField;
    [SerializeField] Button hostButton, joinButton, startButton, pauseButton;
    [SerializeField] TMP_Text startButtonText, hintText, dialogueText;
    [SerializeField] List<GameObject> difficultyIcons, playerIcons;
    [SerializeField] GameObject preparationUI;
    [SerializeField] MultiplayerGameController multiplayerGameController;
    [SerializeField] Color readyColor;
    public NetworkList<PlayerData> players = new NetworkList<PlayerData>();
    NetworkVariable<int> difficulty = new NetworkVariable<int>(0);
    public static NetworkVariable<bool> GameStarted = new NetworkVariable<bool>(false);
    public static NetworkVariable<bool> GamePaused = new NetworkVariable<bool>(true);
    int playerNo;

    public override void OnNetworkSpawn()
    {
        GameStarted.OnValueChanged += GameStartedState;
        GamePaused.OnValueChanged += GamePausedState;
        FinalizeConnection();
    }

    void GameStartedState(bool previous, bool current)
    {
        if (GameStarted.Value)
        {
            int seed = joinCodeInputField.text.ToUpper().ToIntArray().Sum();
            multiplayerGameController.GenerateScore(seed);
            preparationUI.SetActive(false);
            ResumeGameServerRpc();
            GameController.Resume();
        }
    }

    void GamePausedState(bool previous, bool current)
    {
        if (GamePaused.Value)
        {
            pauseButton.GetComponent<PopupOpener>().OpenPopup();
            GameController.Pause();            
        }
        else
        {
            Debug.Log("Pause Change Listened");
            GameObject.FindWithTag("Pause").GetComponent<Popup>().Close();
            GameController.Resume();            
        }
    }

    void Update()
    {
        if (startButtonText.text == "Start")
        {
            startButton.interactable = players[0].isReady && players[1].isReady;            
        }
        for (int i = 0; i < players.Count; i++)
        {
            playerIcons[i].transform.parent.gameObject.SetActive(players[i].hasJoined);
            playerIcons[i].GetComponent<Image>().color = Color.Lerp(playerIcons[i].GetComponent<Image>().color, players[i].isReady? readyColor : Color.white, 0.1f);
        }
    }

    public async void StartRelay()
    {
        try
        {
            DisplayHint();
            string joinCode = await StartHostWithRelay();
            joinCodeInputField.text = joinCode;
            playerNo = 0;
        }
        catch (Exception)
        {
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.text = "Something went wrong! Please check your input or try again later.";
        }
    }
    
    public async void JoinRelay()
    {
        DisplayHint();
        Debug.Log(joinCodeInputField.text.ToUpper());
        await StartClientWithRelay(joinCodeInputField.text.ToUpper());
        playerNo = 1;
    }

    async Task<string> StartHostWithRelay()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Anonymous");
        }

        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "wss"));
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        return NetworkManager.Singleton.StartHost()? joinCode : null;
    }

    async Task<bool> StartClientWithRelay(string joinCode)
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Anonymous");
        }

        JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "wss"));
        return !string.IsNullOrEmpty(joinCode) && NetworkManager.Singleton.StartClient();
    }

    void FinalizeConnection()
    {
        hintText.enabled = false;
        if (IsHost)
        {
            difficulty.Value = GameStartManager.lastDifficulty;
            players.Add(new PlayerData{avatarID = 0, nickname = "", isReady = false, hasJoined = false});
            players.Add(new PlayerData{avatarID = 0, nickname = "", isReady = false, hasJoined = false});
        }
        PlayerData temp = new PlayerData{avatarID = players[playerNo].avatarID, nickname = players[playerNo].nickname, isReady = false, hasJoined = true};
        SetPlayerDataServerRpc(playerNo, temp);
        for (int i = 0; i < difficultyIcons.Count; i++)
        {
            difficultyIcons[i].SetActive(i == difficulty.Value);
        }
        multiplayerGameController.difficulty = difficulty.Value;
        joinCodeInputField.interactable = false;
        joinButton.interactable = false;
        hostButton.interactable = false;
        startButton.interactable = true;
    }

    public void SetReady()
    {
        if (startButtonText.text == "Ready")
        {
            PlayerData temp = new PlayerData{avatarID = players[playerNo].avatarID, nickname = players[playerNo].nickname, isReady = true, hasJoined = true};
            SetPlayerDataServerRpc(playerNo, temp);
            startButtonText.text = "Start";
        }
        else
        {
            StartGameServerRpc();
        }
    }

    void DisplayHint()
    {
        hintText.maxVisibleCharacters = 0;
        hintText.text = "Loading...";
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerDataServerRpc(int index, PlayerData playerData) { players[index] = playerData; }

    [ServerRpc(RequireOwnership = false)]
    public void StartGameServerRpc() { GameStarted.Value = true; }
    [ServerRpc(RequireOwnership = false)]
    public void QuitGameServerRpc() { GameStarted.Value = false; }
    [ServerRpc(RequireOwnership = false)]
    public void PauseGameServerRpc() { GamePaused.Value = true; }
    [ServerRpc(RequireOwnership = false)]
    public void ResumeGameServerRpc()
    {
        Debug.Log("Before Change Paused");
        GamePaused.Value = false;
        Debug.Log("ResumeRpc Called"); }
}
