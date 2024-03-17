using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoteController : MonoBehaviour
{
    Vector2 position, endPosition, checkPosition;
    public TMP_Text gradeText;
    public GameObject ghost, checkObject;
    float speed, ghostSpeed, beat, fullBeat, distanceThreshold, destroyTimer;
    float perfectThreshold = 5f;
    float goodThreshold = 10f;
    bool isDetected, isTarget;
    public bool isClicked = false;
    Rigidbody2D rb, ghostRb;
    
    // Start is called before the first frame update
    void Start()
    {
        fullBeat = 60 / GameController.BPM;
        beat = fullBeat;
        position = GameController.spawnPosition;
        endPosition = GameController.endPosition;
        checkPosition = GameController.checkPosition;
        ghostSpeed = GameController.noteSpeed;
        speed = GameController.noteSpeed * Time.fixedDeltaTime / fullBeat;
        perfectThreshold *= Time.fixedDeltaTime;
        distanceThreshold = ghostSpeed;
        goodThreshold *= Time.fixedDeltaTime;
        destroyTimer = -1f;
        isTarget = Random.value > 0.5f;
        if (isTarget)
        {
            ghost.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sushi/Sushi Orange");
        }        
        rb = GetComponent<Rigidbody2D>();
        ghostRb = ghost.GetComponent<Rigidbody2D>();
        rb.MovePosition(position);
    }

    // Update is called once per frame
    void Update()
    {
        beat -= Time.deltaTime;
        if (destroyTimer < 0)
        {
            if (beat <= 0)
            {
                beat = fullBeat;
                ghostRb.MovePosition(Vector2.MoveTowards(ghostRb.position, endPosition, ghostSpeed));            
            }
            rb.MovePosition(Vector2.MoveTowards(rb.position, endPosition, speed));
        }
        else
        {
            destroyTimer -= Time.deltaTime;
            if (destroyTimer <= 0)
                Destroy(gameObject);
        }            
        
        if (isDetected && isClicked)
        {
            float timing = beat;
            rb.MovePosition(new Vector2(0, -2));                    
            ghostRb.MovePosition(new Vector2(0, -2));
            destroyTimer = fullBeat / 2;
            GameController.noteCount += 1;
            if (isTarget)
            {                    
                bool isPerfect = getDistance(checkPosition) < distanceThreshold &&
                    (timing >= fullBeat - perfectThreshold || timing <= perfectThreshold);
                GameController.grade += (isPerfect) ? 1f : 0.9f;
                gradeText.text = (isPerfect) ? "Perfect!" : "Good!";
                GameController.comboCount += 1;
            }
            else
            {
                gradeText.text = "Wrong!";
                GameController.comboCount = 0;
            }
        }        
        
        if (transform.position.x == endPosition.x)
        {
            Destroy(gameObject);
        }
    }

    public float getDistance(Vector2 position)
    {
        return Vector2.Distance(rb.position, position);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        isDetected = true;
        GameController.noteObjects.Add(this.gameObject);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        isDetected = false;

        if (isTarget && !isClicked)
        {
            gradeText.text = "Miss!";
            GameController.noteCount += 1;
            GameController.comboCount = 0;
        }

        GameController.noteObjects.Remove(this.gameObject);
    }
}