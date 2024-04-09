using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    const string LAST_USERNAME_KEY = "LAST_USERNAME",
        LAST_PASSWORD_KEY = "LAST_PASSWORD";

    [SerializeField]
    TMP_Text signUpNickName;

    [SerializeField]
    TMP_Text signUpPassword;

    [SerializeField]
    TMP_Text logInNickName;

    [SerializeField]
    TMP_Text logInPassword;

    public async void OnRegisterPressed()
    {
        await SignUpWithUsernamePasswordAsync(signUpNickName.text, signUpPassword.text);
    }

    async Task SignUpWithUsernamePasswordAsync(string username, string password)
    {
        await UnityServices.InitializeAsync();
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(
                username,
                password
            );
            Debug.Log("SignUp is successful.");
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
