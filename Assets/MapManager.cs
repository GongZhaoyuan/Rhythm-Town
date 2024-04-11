using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject G1PlayerPrefab;
    public GameObject B1PlayerPrefab;

    public static GameObject playerInstance;

    private void Start()
    {
        Debug.Log("start");
        string toggleName = ToggleManager.selectedToggleName;

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