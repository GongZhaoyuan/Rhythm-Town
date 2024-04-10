using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private GameObject player;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        player = MapManager.playerInstance; // 在 Start 方法中获取 playerInstance

        if (player != null)
        {
            virtualCamera.Follow = player.transform;
        }
        else
        {
            Debug.LogError("PlayerInstance not found!");
        }
    }
}