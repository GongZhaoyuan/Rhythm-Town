using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    public ToggleGroup toggleGroup;
    public Button confirmButton;

    public static string selectedToggleName;

    private void Start()
    {
    }

    public void HandleConfirm()
    {
        // 获取选中的Toggle
        Toggle activeToggle = toggleGroup.ActiveToggles().FirstOrDefault();

        if (activeToggle != null)
        {
            switch (activeToggle.name)
            {
                case "Toggle1":
                    Debug.Log("Toggle 1 selected");
                    break;
                case "Toggle2":
                    Debug.Log("Toggle 2 selected");
                    break;
                case "Toggle3":
                    Debug.Log("Toggle 3 selected");
                    break;
                case "Toggle4":
                    Debug.Log("Toggle 4 selected");
                    break;
            }
            if (activeToggle != null)
            {
                selectedToggleName = activeToggle.name;

                SceneManager.LoadScene("Map");
            }
        }
        Debug.Log("button pressed");

    }
    public string GetSelectedToggleName()
    {
        return selectedToggleName;
    }

}