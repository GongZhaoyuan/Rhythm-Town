using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    float BPM = 142f;
    float beat;
    bool isTarget;
    bool isClicked = false;
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
            position.x -= 1;
            beat = 60 / BPM;
            if (isClicked)
            {
                Destroy(this.gameObject);
            }
        }        
        else
        {
            rb.MovePosition(position);
        }

        if (transform.position.x < -10.0f)
        {
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        position.x = 0;
        position.y = -2;
        rb.MovePosition(position);
        isClicked = true;
        if (isTarget)
        {
            GameController.score += 1;
        }
        else 
        {
            GameController.score -= 1;
        }
    }
}
