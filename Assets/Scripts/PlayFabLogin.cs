using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PlayFab;
using PlayFab.ClientModels;


public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] Text m_IncorrectText;
    [SerializeField] InputField m_Email;
    [SerializeField] InputField m_Password;
    [SerializeField] GameObject m_UserActivity;
    [SerializeField] GameObject m_LoginPage;
    public void OnLoginClicked()
    {
        LoginWithEmailAddressRequest req = new LoginWithEmailAddressRequest
        {
            // You should use the values entered from input field
            Email = m_Email.text.ToString(),
            Password = m_Password.text.ToString()
        };
        PlayFabClientAPI.LoginWithEmailAddress(req,
        // Another way to register the callback function to handle
        // success and failure cases
        OnLoginSuccess, // Function defined below
        OnLoginFailure // Function defined below
        );
    }
    private void OnLoginSuccess(LoginResult result)
    {
        // Debug.Log("Congratulations, you made your first successful API call!");
        m_IncorrectText.gameObject.SetActive(false);
        m_UserActivity.gameObject.SetActive(true);
        m_LoginPage.gameObject.SetActive(false);
        Debug.Log("Login is successful");
    }
    private void OnLoginFailure(PlayFabError error)
    {
        // Debug.LogWarning("Something went wrong with your first API call. :(");
        // Debug.LogError("Here's some debug information:");
        m_IncorrectText.gameObject.SetActive(true);
        Debug.LogError("Login failed with error: \n" + error.GenerateErrorReport());
    }

}
