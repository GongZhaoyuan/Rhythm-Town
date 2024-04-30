using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    public GameObject SignUpPanel;
    public GameObject SignInPanel;
    public GameObject AvatarChoosing;
    public TMP_Text displayText;

    static int ErrorCode;
    bool isSignUp = false;

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
        displayText.text = "Welcome to the Rhythm Town! May I have your ID?";
    }

    void Update()
    {
        int errorFirstDigit = Convert.ToInt32(ErrorCode.ToString().Substring(0, 1));
        Debug.Log("***********errorFirstDigit" + errorFirstDigit);
        if ((errorFirstDigit == 4) && (isSignUp = true))
        {
            displayText.text =
                "The username requires a minimum of 3-20 characters and only supports letters, numbers and symbols like ., -, @ or _"
                + "The password requires 8-30 characters and at least 1 lowercase letter, 1 uppercase letter, 1 number, and 1 symbol.";
        }
        else if ((errorFirstDigit == 4) && (isSignUp = false))
        {
            displayText.text = "Incorrect username or password.";
        }
        else if (errorFirstDigit == 5)
        {
            displayText.text = "Unity server error";
        }
        else
        {
            displayText.text = "Oops something went wrong.";
        }
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
        isSignUp = true;
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

            Debug.Log("authentication");
            Debug.LogException(ex);
            Debug.Log("(SignUp)authentication exception: " + ex.ErrorCode + "()msg: " + ex.Message);
            // ErrorCode = ex.ErrorCode;
            Debug.Log("--------------------current error code: " + ErrorCode);

            var webRequestException = ex.InnerException as WebRequestException;
            if (webRequestException != null)
            {
                // Handle the WebRequestException
                Debug.Log("WebRequestException: " + ex.InnerException.Message);

                // Access the response code
                long responseCode = webRequestException.GetResponseCode();
                Debug.Log("Response Code: " + responseCode);
            }
            else
            {
                // Handle other types of AuthenticationException
                Debug.Log("AuthenticationException: " + ex.Message);
            }
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.Log("requestfailed");
            Debug.LogException(ex);
            Debug.Log("(SignUp)request exception: " + ex.ErrorCode + "()msg: " + ex.Message);
            // ErrorCode = ex.ErrorCode;
            Debug.Log("--------------------current error code: " + ErrorCode);
        }
        catch (WebException ex)
        {
            Debug.Log("&&&&&&&&&webException: " + ex.Response);
        }
    }

    async Task SignInWithUsernamePasswordAsync(string username, string password)
    {
        isSignUp = false;
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
            Debug.Log("(SignIn)authentication exception: " + ex.ErrorCode + "()msg: " + ex.Message);
            ErrorCode = ex.ErrorCode;
            Debug.Log("--------------------current error code: " + ErrorCode);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
            Debug.Log("(SignIn)request exception: " + ex.ErrorCode + "()msg: " + ex.Message);
            ErrorCode = ex.ErrorCode;
            Debug.Log("--------------------current error code: " + ErrorCode);
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
