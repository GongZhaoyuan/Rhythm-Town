using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BookManager : MonoBehaviour
{
    public GameObject missObject;
    public GameObject coinObject;
    public GameObject rangeObject;
    public GameObject energyObject;

    public TMP_Text displayText;
    public Button equipButton;

    GameObject clickedObject;

    private Dictionary<GameObject, string> objectTextMap;
    public Dictionary<GameObject, bool> loanDisplayStatus;

    private void Start()
    {
        objectTextMap = new Dictionary<GameObject, string>()
        {
            { missObject, "Miss: Help you reduce the number of missed note!" },
            { coinObject, "Coin: Earn more coin for each game!" },
            { rangeObject, "Range: The accuracy detection rage will increase!" },
            { energyObject, "Energy: Less energy needed for each game!" }
        };
        loanDisplayStatus = new Dictionary<GameObject, bool>()
        {
            { missObject, false },
            { coinObject, false },
            { rangeObject, false },
            { energyObject, false }
        };
        equipButton.onClick.AddListener(OnEquipButtonClick);
        equipButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("detecting.........");
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                clickedObject = hit.collider.gameObject;

                if (objectTextMap.ContainsKey(clickedObject))
                {
                    string objectText = objectTextMap[clickedObject];
                    //Debug.Log("Object Clicked: " + hitObject.name);
                    //Debug.Log("Text: " + objectText);

                    displayText.gameObject.SetActive(true);
                    displayText.text = objectText;

                    equipButton.gameObject.SetActive(true);
                }
            }
            else
            {
                displayText.text = "Welcome to the library! How can I help?";
                // equipButton.gameObject.SetActive(false);
            }
        }
    }

    public void OnEquipButtonClick()
    {
        Debug.Log("Button in function", clickedObject);

        if (loanDisplayStatus.ContainsKey(clickedObject))
        {
            loanDisplayStatus[clickedObject] = true;
        }
    }
}
