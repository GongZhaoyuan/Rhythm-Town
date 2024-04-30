using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceBell : MonoBehaviour
{
    public static bool clickFlag = false;
    Animator animator;
    AudioSource audioSource;
    public AudioClip soundFX;

    protected virtual void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("Clicked");
            audioSource.PlayOneShot(soundFX);
        }
    }
}
