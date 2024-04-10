using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    public GameObject AvatarChoosing;
    public GameObject SignUpPanel;

    const string LAST_USERNAME_KEY = "LAST_USERNAME",
        LAST_PASSWORD_KEY = "LAST_PASSWORD";

    [SerializeField]
    TMP_Text signUpEmailAsUserName;

    [SerializeField]
    TMP_Text signUpPassword;

    [SerializeField]
    TMP_Text logInEmail;

    [SerializeField]
    TMP_Text logInPassword;

    public async void On_SignUp_SignUpPressed()
    {
        await SignUpWithUsernamePasswordAsync(signUpEmailAsUserName.text, signUpPassword.text);
    }

    public async void On_LogIn_LogInPressed()
    {
        await SignInWithUsernamePasswordAsync(logInEmail.text, logInPassword.text);
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
            AvatarChoosing.gameObject.SetActive(true);
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
}
