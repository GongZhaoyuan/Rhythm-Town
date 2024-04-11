using System.Collections;
using System.Collections.Generic;
using Ricimi;
using UnityEngine;

public class EnterBuilding : MonoBehaviour
{
    private bool inRange;
    public static bool isPopupOpened;

    void Start()
    {
        inRange = false;
        isPopupOpened = false;
    }

    void Update()
    {
        if (!inRange) return;
        if (Input.GetKeyDown(KeyCode.E) && !isPopupOpened)
        {
            Enter();
        }
    }

    void Enter()
    {
        PlayerMovement.RecordLastPosition();
        GetComponent<PopupOpener>().OpenPopup();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {inRange = true;};
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && inRange) {inRange = false;};
    }

    void OnMouseDown()
    {
        if (!isPopupOpened)
        {
            Enter();
        }
    }
}
