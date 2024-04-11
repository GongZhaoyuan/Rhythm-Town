using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    public ToggleGroup toggleGroup;
    public Button confirmButton;

    public static string selectedToggleName;

    private void Start() { }

    public async void HandleConfirm()
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
                await UpdateRecordAsync(selectedToggleName);
                SceneManager.LoadScene("Map");
            }
        }
        Debug.Log("button pressed");
    }

    public string GetSelectedToggleName()
    {
        return selectedToggleName;
    }

    public async Task UpdateRecordAsync(string selectedToggleName)
    {
        String sendback = GetSelectedToggleName();
        Debug.Log("avatar" + selectedToggleName + " saved to cloud");
        // Save the objectLevelRecord_Cloud to the cloud or perform any other necessary updates
        var data = new Dictionary<string, object> { { "Avatar", selectedToggleName }, };

        // Task CloudSaveService.Instance.Data.Player.SaveAsync(Dictionary<string, object> data)；
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
    }
}
