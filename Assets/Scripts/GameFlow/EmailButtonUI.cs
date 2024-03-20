using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EmailButtonUI : MonoBehaviour
{
    [SerializeField]
    private string MailGameLevel = "Mail";

    public GameObject dialogBox;

    public void NewGameButton()
    {
        SceneManager.LoadScene(MailGameLevel);
    }

    public void ClosePage()
    {
        dialogBox.SetActive(false);
    }
}
