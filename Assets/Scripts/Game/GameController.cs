using System.Collections.Generic;
using Ricimi;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : NetworkBehaviour
{
    public float musicBPM, speed = 10f, generateSpeed = 0.25f;
    public static float BPM, noteSpeed, grade;
    public static int noteCount, comboCount;
    public static List<GameObject> noteObjects;
    static float beat, countdownBeat, fullBeat;
    public static bool isPaused, isOver;
    public GameObject notePrefab;
    public Transform spawnPoint, endPoint, checkPoint;
    public RectTransform accuracyBar;
    public static Vector2 spawnPosition, endPosition, checkPosition;
    public TMP_Text countdownText, accuracyText, comboText;
    public AudioClip countIn;
    AudioSource audioSource, bgmAudioSource;
    static int countdown = 4;
    protected int noteID;
    protected Queue<bool> score;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        BPM = musicBPM;
        audioSource = GetComponent<AudioSource>();
        bgmAudioSource = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
        spawnPosition = spawnPoint.position;
        endPosition = endPoint.position;
        checkPosition = checkPoint.position;
        fullBeat = 60 / BPM;
        beat = fullBeat;
        countdownBeat = fullBeat;
        noteSpeed = Vector2.Distance(endPosition, spawnPosition) / speed / 2;
        noteObjects = new List<GameObject>();
        score = ScoreGenerator.GetScore(320, 42);
        accuracyBar.anchorMax = new Vector2(1, 1);
        accuracyText.text = "100%";
        isPaused = false;
        grade = 0f;
        noteID = 0;
        noteCount = 0;
        comboCount = 0;
        isOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOver) return;
        
        if (isPaused)
        {
            countdownText.text = "";
            if (bgmAudioSource.isPlaying) { bgmAudioSource.Pause(); }
        }
        else
        {
            if (CountIn())
            {
                beat -= Time.deltaTime;
                if (beat <= 0)
                {
                    beat = fullBeat / generateSpeed;
                    GenerateNote();
                }
                AccuracyCalculate();
                DetectClick();
                if (score.Count <= 0)
                {
                    GetComponent<SceneTransition>().PerformTransition();
                    isOver = true;
                }
            }
        }
    }

    bool CountIn()
    {
        countdownText.enabled = countdown >= 0;
        if (countdown >= 0)
        {            
            countdownBeat -= Time.deltaTime;
            if (countdownBeat <= 0) {
                countdown--;
                if (countdown >= 0)
                {
                    audioSource.PlayOneShot(countIn);
                }     
                countdownBeat = fullBeat;
                countdownText.text = (countdown > 0) ? countdown.ToString(): "GO!";                
            }
        }
        else
        {
            if (!bgmAudioSource.isPlaying) { bgmAudioSource.Play(); }
        }
        
        NoteController.isPaused = countdown >= 0;
        return countdown < 0;
    }

    void AccuracyCalculate()
    {
        float accuracy = grade / noteCount;
        if (float.IsNaN(accuracy)) { accuracy = 1f; }
        accuracyBar.anchorMax = Vector2.Lerp(accuracyBar.anchorMax, new Vector2(accuracy, 1), 3 * Time.deltaTime);
        accuracyText.text = $"{ Mathf.Round(accuracy * 10000) / 100f }%";
        comboText.text = (comboCount == 0) ? "" : $"{comboCount}\nCombo";
    }

    void DetectClick()
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
                    RecordNote(noteObjects[0]);
                }
            }
        }
    }

    protected virtual void RecordNote(GameObject noteObject) {}

    protected virtual void GenerateNote()
    {
        GameObject newNote = Instantiate(notePrefab, spawnPosition, notePrefab.transform.rotation);
        newNote.GetComponent<NoteController>().isTarget = score.Dequeue();
        newNote.GetComponent<NoteController>().noteID = noteID++;
    }
    
    public void Pause()
    {
        isPaused = true;
        NoteController.isPaused = true;
        countdown = 4;
        countdownBeat = fullBeat;
    }

    public void Resume()
    {
        isPaused = false;        
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        SceneManager.LoadScene("Map");
    }
}