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

        if (toggleName == "Toggle4")
        {
            playerInstance = Instantiate(G1PlayerPrefab, Vector2.zero, Quaternion.identity);

            Debug.Log("prefab4");
        }
        else if (toggleName == "Toggle2")
        {
            playerInstance = Instantiate(B1PlayerPrefab, Vector2.zero, Quaternion.identity);
            Debug.Log("prefab2");
        }
    }
}