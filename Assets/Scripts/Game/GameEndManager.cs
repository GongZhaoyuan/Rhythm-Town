using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameEndManager : MonoBehaviour
{
    public TMP_Text accuracyText, bestRecordText;
    public GameObject easy, normal, hard, expert, infinite;
    // Start is called before the first frame update
    void Start()
    {
        accuracyText.text = $"{ GameController.accuracy }%";
        bestRecordText.text = $"{ GameStartManager.bestRecord }";
        List<GameObject> difficulty = new List<GameObject>{easy, normal, hard, expert, infinite};
        for (int i = 0; i < difficulty.Count; i++)
        {
            difficulty[i].SetActive(i == GameStartManager.lastDifficulty);
        }
    }
}
