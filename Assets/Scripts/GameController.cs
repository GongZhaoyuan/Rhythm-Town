using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float musicBPM;
    public static float BPM;
    public float speed = 10f;
    public static float noteSpeed;
    public float generateSpeed = 0.25f;
    public static int noteCount = 0;    
    public static float grade = 0f;
    public static int comboCount = 0;
    public static List<GameObject> noteObjects;
    float beat, fullBeat;
    public GameObject notePrefab;
    public Transform spawnPoint, endPoint, checkPoint;
    public static Vector2 spawnPosition, endPosition, checkPosition;
    public TMP_Text countdownText, accuracyText, comboText;
    public AudioClip countIn;
    AudioSource audioSource;
    int countdown = 4;
    Queue<bool> score;

    // Start is called before the first frame update
    void Start()
    {
        BPM = musicBPM;
        audioSource = GetComponent<AudioSource>();
        spawnPosition = spawnPoint.position;
        endPosition = endPoint.position;
        checkPosition = checkPoint.position;
        fullBeat = 60 / BPM;
        beat = fullBeat;
        noteSpeed = Vector2.Distance(endPosition, spawnPosition) / speed / 2;
        noteObjects = new List<GameObject>();
        score = ScoreGenerator.GetScore(320, 42);
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
                beat = fullBeat / generateSpeed;
                GameObject newNote = Instantiate(notePrefab, spawnPosition, notePrefab.transform.rotation);
                newNote.GetComponent<NoteController>().isTarget = score.Dequeue();
            }
            accuracyText.text = $"Accuracy: {AccuracyCalculate()}%";
            comboText.text = (comboCount == 0) ? "" : $"{comboCount}\nCombo";
            ClickNote();
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

    float AccuracyCalculate()
    {
        float accuracy = Mathf.Round(grade / noteCount * 10000) / 100f;
        if (float.IsNaN(accuracy))
        {
            accuracy = 100f;
        }
        
        return accuracy;
    }

    void ClickNote()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            //If something was hit, the RaycastHit2D.collider will not be null.
            if (hit.collider != null)
            {
                if (noteObjects.Count != 0)
                {
                    noteObjects.Sort((o1, o2) => (int)(o1.GetComponent<NoteController>().getDistance(checkPosition) - o2.GetComponent<NoteController>().getDistance(checkPosition)));
                    NoteController nearestNote = noteObjects[0].GetComponent<NoteController>();
                    nearestNote.clickSource = hit.collider.gameObject.name;
                    nearestNote.isClicked = true;
                }
            }
        }
    }
}