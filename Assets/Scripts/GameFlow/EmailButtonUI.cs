using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EmailButtonUI : MonoBehaviour
{
    [SerializeField]
    private string GameLevel;

    public GameObject dialogBox;

    public void NewGameButton()
    {
        SceneManager.LoadScene(GameLevel);
    }

    public void ClosePage()
    {
        dialogBox.SetActive(false);
    }
}
