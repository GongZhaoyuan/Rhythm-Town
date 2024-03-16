using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static float BPM = 142f;
    public float speed = 10f;
    public static float noteSpeed;
    public static int noteCount = 0;    
    public static float grade = 0f;
    public static float accuracy = 100f;
    public static int comboCount = 0;
    float beat;
    public GameObject notePrefab;
    public Transform spawnPoint;
    public Transform endPoint;
    public static Vector2 spawnPosition;
    public static Vector2 endPosition;
    public TMP_Text accuracyText;
    public TMP_Text comboText;

    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = spawnPoint.position;
        endPosition = endPoint.position;
        beat = 240 / BPM;
        noteSpeed = Vector2.Distance(endPosition, spawnPosition) / speed / 2;
    }

    // Update is called once per frame
    void Update()
    {
        beat -= Time.deltaTime;
        if (beat <= 0)
        {
            beat = 240 / BPM;
            Instantiate(notePrefab, spawnPosition, Quaternion.identity);
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
