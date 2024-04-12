using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignUpPageCB : MonoBehaviour
{
    [Header("REST URL")]
    [SerializeField] private string createNewPlayerUrl;
    [Header("UI Elements")]
    [SerializeField] private TMP_InputField userNameInput;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private GameObject signUpButton;
    [SerializeField] private GameObject progressIcon;
    [SerializeField] private TMP_Text debugText;

    #region Button Handlers
    public void HandleSignUpButton()
    {
        //validate username
        if (ValidateUserName())
        {
            //upload new user to database
            Player newPlayer = new Player()
            {
                id = Guid.NewGuid().ToString(),
                username = userNameInput.text,
                email = emailInput.text,
                HighScore = 0,
            };

            
            PlayerStatsManager.Instance.CreateNewPlayer(newPlayer,HandleCreateNewPlayerError,HandleCreateNewPlayerSuccess);

        }
        else
        {

        }
    }

    private void HandleCreateNewPlayerError()
    {
        debugText.text = "error creating player, kindly check internet connectin.Or contanct developer";
    }

    private void HandleCreateNewPlayerSuccess()
    {
        debugText.text = "success";
        SceneManager.LoadScene("Dashboard");
    }


    #endregion

    #region Validation Logic
    private bool ValidateUserName()
    {
       string userName = userNameInput.text;
        string email = emailInput.text;
        if(!string.IsNullOrEmpty(userName) ||
            !string.IsNullOrEmpty(email))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
