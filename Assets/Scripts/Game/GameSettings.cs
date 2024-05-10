using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [SerializeField] Slider musicSlider, soundSlider;
    [SerializeField] TMP_Text musicText, soundText, offsetText;
    public static int musicVolume, soundVolume, offset;
    string[] settingsLabels = {"Settings_MusicVolume", "Settings_SoundVolume", "Settings_Offset"};

    void Awake()
    {
        musicSlider.value = musicVolume;
        soundSlider.value = soundVolume;
        Display();
    }

    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Anonymous");
        }
        var settingsData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> (settingsLabels));
        if (settingsData.TryGetValue(settingsLabels[0], out var data))
        {
            if (!int.TryParse(data.Value.GetAsString(), out musicVolume))
                musicVolume = 100;
        }
        if (settingsData.TryGetValue(settingsLabels[1], out data))
        {
            if (!int.TryParse(data.Value.GetAsString(), out soundVolume))
                soundVolume = 100;
        }
        if (settingsData.TryGetValue(settingsLabels[2], out data))
        {
            if (!int.TryParse(data.Value.GetAsString(), out offset))
                offset = 0;
        }
        musicSlider.value = musicVolume;
        soundSlider.value = soundVolume;
    }

    // Update is called once per frame
    void Update()
    {
        musicVolume = (int) musicSlider.value;
        soundVolume = (int) soundSlider.value;
        Display();
    }

    void Display()
    {
        musicText.text = musicVolume.ToString();
        soundText.text = soundVolume.ToString();
        offsetText.text = offset.ToString();
    }

    public async void SaveSettings()
    {
        var settingsData = new Dictionary<string, object> {
            { settingsLabels[0], musicVolume },
            { settingsLabels[1], soundVolume },
            { settingsLabels[2], offset }
        };
        await CloudSaveService.Instance.Data.Player.SaveAsync(settingsData);
    }

    public void OffsetAdjust(int amount) { offset += amount; }
}
