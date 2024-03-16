using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    [SerializeField]
    private string newGameLevel = "Sushi";

    public GameObject dialogBox;

    public void NewGameButton()
    {
        SceneManager.LoadScene(newGameLevel);
    }

    public void ClosePage()
    {
        dialogBox.SetActive(false);
    }
}
