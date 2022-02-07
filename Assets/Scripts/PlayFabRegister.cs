using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PlayFab;
using PlayFab.ClientModels;
using System.Text.RegularExpressions;

public class PlayFabRegister : MonoBehaviour
{
    [SerializeField] int m_MinimumPassLength = 5;
    [SerializeField] int m_MaximumPassLength = 18;
    [SerializeField] InputField m_DisplayName;
    [SerializeField] InputField m_Email;
    [SerializeField] InputField m_Password;
    [SerializeField] InputField m_ConfirmPassword;
    [SerializeField] Text m_EmptyField;
    [SerializeField] Text m_LengthNotMet;
    [SerializeField] Text m_RequirementsNotMet;
    [SerializeField] Text m_PasswordDontMatch;
    [SerializeField] Text m_UserRegistered;

    public void OnRegisterClicked()
    {
        if (m_DisplayName.text == "" || m_Email.text == "" || m_Password.text == "" || m_ConfirmPassword.text == "")
        {
            // One or more of your input fields are empty
            m_EmptyField.gameObject.SetActive(true);
            m_UserRegistered.gameObject.SetActive(false);

            Debug.Log("One of the fields are empty");
            return;
        }
        m_EmptyField.gameObject.SetActive(false);
        m_UserRegistered.gameObject.SetActive(false);
        int num = CheckPassword(m_Password, m_ConfirmPassword);
        if(num != 0)
        {
            return;
        }

        RegisterPlayFabUserRequest req = new RegisterPlayFabUserRequest
        {
            // You should use the values entered from the input field to register
            // There are certain email addresses not available
            // e.g. abc@test.com so ensure you try something else
            Email = m_Email.text.ToString(),
            DisplayName = m_DisplayName.text.ToString(),
            Password = m_Password.text.ToString(),
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(req,
        // Callback function to handle successful registration
        // You can display the messages to the interface from here
        res =>
        {
            Debug.Log("User is successfully registered");
            m_UserRegistered.gameObject.SetActive(true);
        },
        // Callback function for any error encountered
        // You can display the messages to the interface from here
        err =>
        {
            Debug.Log("Error: " + err.ErrorMessage);
        }
        );
    }

    public int CheckPassword(InputField password, InputField confirmPassword)
    {
        // 0 match 1 dont match 2 length not met 3 requirements not met 
        if (password.text == confirmPassword.text)
        {
            if (password.text.ToString().Length >= m_MinimumPassLength && password.text.ToString().Length <= m_MaximumPassLength)
            {
                if (Regex.IsMatch(password.text.ToString(), @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{4,20}$"))
                {
                    Debug.Log("Password meets requirements");
                    m_LengthNotMet.gameObject.SetActive(false);
                    m_PasswordDontMatch.gameObject.SetActive(false);
                    m_RequirementsNotMet.gameObject.SetActive(false);
                    return 0;
                }
                Debug.Log("Password does not meet requirements");
                m_LengthNotMet.gameObject.SetActive(false);
                m_PasswordDontMatch.gameObject.SetActive(false);
                m_RequirementsNotMet.gameObject.SetActive(true);
                return 3;
            }
            Debug.Log("Password does not meet length");
            m_LengthNotMet.gameObject.SetActive(true);
            m_PasswordDontMatch.gameObject.SetActive(false);
            m_RequirementsNotMet.gameObject.SetActive(false);
            return 2;
        }
        Debug.Log("Password does not match");
        m_LengthNotMet.gameObject.SetActive(false);
        m_PasswordDontMatch.gameObject.SetActive(true);
        m_RequirementsNotMet.gameObject.SetActive(false);
        return 1;
    }
}
