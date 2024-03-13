using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float BPM = 142f;
    float beat;
    public GameObject sushiPrefab;
    public static int score;
    public TMP_Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
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
        scoreText.text = "Score: " + score;
    }
}
