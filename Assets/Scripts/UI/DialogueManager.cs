using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private static TMP_Text textbox;
    private bool isSet;

    // Start is called before the first frame update
    void Start()
    {
        isSet = false;
        textbox = GetComponent<TMP_Text>();
        textbox.maxVisibleCharacters = 0;
    }

    public static void SetText(string textContent)
    {
        textbox.maxVisibleCharacters = 0;
        textbox.text = textContent;
    }
}
