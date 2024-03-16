using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoteController : MonoBehaviour
{
    public TMP_Text gradeText;
    float BPM = GameController.BPM;
    float beat;
    bool isTarget;
    int clickedFlag = 0;
    Rigidbody2D rb;
    Vector2 position;
    
    // Start is called before the first frame update
    void Start()
    {
        isTarget = Random.value > 0.5f;
        if (isTarget)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sushi/Sushi Orange");
        }
        beat = 60 / BPM;
        rb = GetComponent<Rigidbody2D>();
        position = rb.position;
        position.x = 10f;
        position.y = 0.4f;
        rb.MovePosition(position);
    }

    // Update is called once per frame
    void Update()
    {
        beat -= Time.deltaTime;
        if (beat <= 0)
        {            
            beat = 60 / BPM;
            switch (clickedFlag)
            {
                case 1:
                // clicked early
                    clickedFlag = -1;
                    break;
                
                case -1:
                    Destroy(this.gameObject);
                    break;

                                    
                default:
                    position.x -= 1;
                    break;
            }
        }        
        else
        {
            rb.MovePosition(position);
        }

        if (transform.position.x < -1f && isTarget)
        {
            if (clickedFlag == 0)
            {
                gradeText.text = "Miss!";
                GameController.noteCount += 1;
                GameController.comboCount = 0;
                clickedFlag = -2;
            }
        }

        if (transform.position.x < -10.0f)
        {
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
       if (transform.position.x <= 1f)
       {
            float timing = beat;
            position.x = 0;
            position.y = -2;
            rb.MovePosition(position);
            clickedFlag = (timing > 30/BPM) ? -1 : 1;
            GameController.noteCount += 1;
            if (isTarget)
            {                
                GameController.grade += (timing >= 60 / BPM - 10*Time.deltaTime || timing <= 10*Time.deltaTime) ? 1f : 0.9f;   
                gradeText.text = (timing >= 60 / BPM - 10*Time.deltaTime || timing <= 10*Time.deltaTime) ? "Perfect!" : "Good!";
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
