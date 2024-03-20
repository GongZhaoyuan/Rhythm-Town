using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountIn : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip countIn;
    public static bool finished = false;
    int countdown = 3;
    float beat;
    float fullBeat;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        fullBeat = 60 / GameController.BPM;
        beat = fullBeat;
    }

    // Update is called once per frame
    void Update()
    {        
        if (countdown >= 0)
        {
            beat -= Time.deltaTime;
            if (beat <= 0) {
                audioSource.PlayOneShot(countIn);
                countdown--;
                beat = fullBeat;
                Debug.Log(countdown);
            }
        }
        else
        {
            finished = true;
        }
    }
}
