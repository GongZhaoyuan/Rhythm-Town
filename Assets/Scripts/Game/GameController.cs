using System.Collections.Generic;
using Ricimi;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : NetworkBehaviour
{
    public float musicBPM, speed = 10f;
    public int musiclength;
    public static float BPM, noteSpeed, generateSpeed, grade, accuracy;
    public static int noteCount, comboCount;
    public static List<GameObject> noteObjects;
    static float beat, countdownBeat, fullBeat;
    public static bool isPaused, isOver, isGameEnd;
    public GameObject notePrefab;
    public Transform spawnPoint, endPoint, checkPoint;
    public RectTransform accuracyBar;
    public static Vector2 spawnPosition, endPosition, checkPosition;
    public TMP_Text countdownText, accuracyText, comboText;
    public AudioClip countIn;
    AudioSource audioSource, bgmAudioSource;
    static int countdown = 4, barLength;
    protected int noteID, difficulty = 1;
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

        difficulty = GameStartManager.lastDifficulty;
        List<int> barLengths = new List<int> {2, 4, 4, 8};
        barLength = barLengths[difficulty];
        generateSpeed = barLength / 4f;
        score = ScoreGenerator.GetScore(musiclength, barLength, 42);

        noteSpeed = Vector2.Distance(endPosition, spawnPosition) / speed / 2;
        noteObjects = new List<GameObject>();

        accuracyBar.anchorMax = new Vector2(1, 1);
        accuracyText.text = "100%";

        isPaused = false;
        isOver = false;
        isGameEnd = false;

        grade = 0f;
        noteID = 0;
        noteCount = 0;
        comboCount = 0;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isGameEnd) return;

        if (isOver) {
            beat -= Time.deltaTime;
            if (beat <= 0)
            {
                beat = fullBeat / generateSpeed;
                countdown--;
            }
            if (countdown <= 0)
            {
                GetComponent<SceneTransition>().PerformTransition();
                isGameEnd = true;                
            }
            return;        
        }

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
                accuracy = AccuracyCalculate();
                DetectClick();
                if (score.Count <= 0)
                {                    
                    isOver = true;
                    countdown = barLength * 2;
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

    float AccuracyCalculate()
    {
        float accuracy = grade / noteCount;
        if (float.IsNaN(accuracy)) { accuracy = 1f; }
        accuracyBar.anchorMax = Vector2.Lerp(accuracyBar.anchorMax, new Vector2(accuracy, 1), 3 * Time.deltaTime);
        accuracy = Mathf.Round(accuracy * 10000) / 100f;
        accuracyText.text = $"{ accuracy }%";
        comboText.text = (comboCount == 0) ? "" : $"{comboCount}\nCombo";

        return accuracy;
    }

    void DetectClick()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            //If something was hit, the RaycastHit2D.collider will not be null.
            if (hit.collider != null && hit.collider.CompareTag("Clickable"))
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