using UnityEngine;

public class ServiceBell : MonoBehaviour
{
    public static bool clickFlag = false;
    protected Animator animator;
    protected AudioSource audioSource;
    public AudioClip soundFX;

    protected virtual void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    
    protected virtual void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("Clicked");
            audioSource.PlayOneShot(soundFX);
        }
    }
}