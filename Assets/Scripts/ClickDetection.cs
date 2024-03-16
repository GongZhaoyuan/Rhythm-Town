using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDetection : MonoBehaviour
{
    public static bool clickFlag = false;
    Animator animator;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("Clicked");
        }
    }
}
