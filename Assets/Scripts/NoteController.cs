using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoteController : MonoBehaviour
{
    Vector2 position, endPosition;
    public TMP_Text gradeText;
    public GameObject ghost, checkObject;
    float speed, ghostSpeed, beat, fullBeat;
    float perfectThreshold = 5f;
    float goodThreshold = 10f;
    bool isDetected, isTarget;
    bool isClicked = false;
    Rigidbody2D rb, ghostRb;
    
    // Start is called before the first frame update
    void Start()
    {
        fullBeat = 60 / GameController.BPM;
        beat = fullBeat;
        position = GameController.spawnPosition;
        endPosition = GameController.endPosition;
        ghostSpeed = GameController.noteSpeed;
        speed = GameController.noteSpeed * Time.fixedDeltaTime / fullBeat;
        perfectThreshold *= Time.fixedDeltaTime;
        goodThreshold *= Time.fixedDeltaTime;
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
        rb.MovePosition(Vector2.MoveTowards(rb.position, endPosition, speed));
        if (beat <= 0)
        {
            beat = fullBeat;
            if (isClicked)
            {
                Destroy(gameObject);
            }
            else
            {
                ghostRb.MovePosition(Vector2.MoveTowards(ghostRb.position, endPosition, ghostSpeed));
            }
        }
        if (isDetected)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

                //If something was hit, the RaycastHit2D.collider will not be null.
                if (hit.collider.gameObject.name == checkObject.name)
                {
                    isClicked = true;
                }
            }

            if (isClicked)
            {
                if (transform.position.x <= 1f)
                {
                    float timing = beat;
                    rb.MovePosition(new Vector2(0, -2));
                    GameController.noteCount += 1;
                    if (isTarget)
                    {
                        bool isPerfect = timing >= fullBeat - perfectThreshold || timing <= perfectThreshold;
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
            }
        }
        
        if (transform.position.x == endPosition.x)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        isDetected = true;
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
    }
}