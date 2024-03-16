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
    float fullBeat;
    public GameObject notePrefab;
    public Transform spawnPoint, endPoint;
    public static Vector2 spawnPosition, endPosition;
    public TMP_Text countdownText, accuracyText, comboText;
    public AudioClip countIn;
    AudioSource audioSource;
    int countdown = 4;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spawnPosition = spawnPoint.position;
        endPosition = endPoint.position;
        fullBeat = 60 / BPM;
        beat = fullBeat;
        noteSpeed = Vector2.Distance(endPosition, spawnPosition) / speed / 2;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (CountIn())
        {
            countdownText.enabled = false;

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

    bool CountIn()
    {        
        if (countdown >= 0)
        {            
            beat -= Time.deltaTime;
            if (beat <= 0) {
                countdown--;
                if (countdown >=0)
                {
                    audioSource.PlayOneShot(countIn);
                }     
                beat = fullBeat;
                countdownText.text = (countdown > 0) ? countdown.ToString(): "GO!";                
            }
        }
        
        return countdown < 0;
    }
}
