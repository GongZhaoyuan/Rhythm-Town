using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    public GameObject SignUpPanel;
    public GameObject SignInPanel;
    public GameObject AvatarChoosing;

    const string LAST_USERNAME_KEY = "LAST_USERNAME",
        LAST_PASSWORD_KEY = "LAST_PASSWORD";

    [SerializeField]
    TMP_Text signUpEmailAsUserName;

    [SerializeField]
    // InputField signUpPassword;
    TMP_Text signUpPassword;

    [SerializeField]
    TMP_Text signUpNickName;

    [SerializeField]
    TMP_Text logInEmail;

    [SerializeField]
    TMP_Text logInPassword;

    void Start()
    {
        SignInPanel.gameObject.SetActive(true);
        SignUpPanel.gameObject.SetActive(false);
        AvatarChoosing.gameObject.SetActive(false);
    }

    public async void On_SignUp_SignUpPressed()
    {
        await SignUpWithUsernamePasswordAsync(
            signUpEmailAsUserName.text.Replace("\u200B", ""),
            signUpPassword.text.Replace("\u200B", "")
        );
    }

    public async void On_LogIn_LogInPressed()
    {
        await SignInWithUsernamePasswordAsync(
            logInEmail.text.Replace("\u200B", ""),
            logInPassword.text.Replace("\u200B", "")
        );
    }

    async Task SignUpWithUsernamePasswordAsync(string username, string password)
    {
        await UnityServices.InitializeAsync();
        try
        {
            Debug.Log("username:" + username + " password: " + password);
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(
                username,
                password
            );
            Debug.Log("SignUp is successful.");
            Debug.Log("PlayerID: " + AuthenticationService.Instance.PlayerId);
            SignUpPanel.gameObject.SetActive(false);
            // SignInPanel.gameObject.SetActive(true);
            AvatarChoosing.gameObject.SetActive(true);
            await UpdateRecordAsync(
                signUpNickName.text.Replace("\u200B", ""),
                signUpEmailAsUserName.text.Replace("\u200B", ""),
                signUpPassword.text.Replace("\u200B", "")
            );
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    async Task SignInWithUsernamePasswordAsync(string username, string password)
    {
        await UnityServices.InitializeAsync();
        try
        {
            Debug.Log("username:" + username + " password: " + password);
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(
                username,
                password
            );
            Debug.Log("SignIn is successful.");
            Debug.Log("PlayerID: " + AuthenticationService.Instance.PlayerId);
            SceneManager.LoadScene("Map");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    public async Task UpdateRecordAsync(string nickname, string username, string password)
    {
        Debug.Log(nickname + " saved to cloud");
        // Save the objectLevelRecord_Cloud to the cloud or perform any other necessary updates
        var data = new Dictionary<string, object>
        {
            { "inGameDisplayNickName", nickname },
            { "PlayerInfo_email", username },
            { "PlayerInfo_psw", password }
        };

        // Task CloudSaveService.Instance.Data.Player.SaveAsync(Dictionary<string, object> data)；
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
    }
}
