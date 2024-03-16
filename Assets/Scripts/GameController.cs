using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static float BPM = 142f;
    public static int noteCount = 0;    
    public static float grade = 0f;
    public static float accuracy = 100.0f;
    public static int comboCount = 0;
    float beat;
    public GameObject sushiPrefab;
    public TMP_Text accuracyText;
    public TMP_Text comboText;

    // Start is called before the first frame update
    void Start()
    {
        beat = 240 / BPM;
    }

    // Update is called once per frame
    void Update()
    {
        beat -= Time.deltaTime;
        if (beat <= 0)
        {
            beat = 240 / BPM;
            GameObject sushiObject = Instantiate(sushiPrefab, new Vector3(10f, 0.4f, 0), Quaternion.identity);
        }
        accuracy = Mathf.Round(grade / noteCount * 10000) / 100f;
        if (float.IsNaN(accuracy))
        {
            accuracy = 100f;
        }
        accuracyText.text = "Accuracy: " + accuracy + "%";
        comboText.text = (comboCount == 0) ? "" : comboCount + "\nCombo";
    }
}
