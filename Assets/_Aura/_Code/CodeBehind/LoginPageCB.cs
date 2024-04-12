using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginPageCB : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_Text debugText;

    #region Button Handlers

    /// <summary>
    /// Called when hop back in button is clicked
    /// does simple validation and clean up of email
    /// before checking if player is in the database
    /// </summary>
    public void HandleHopBackInButton()
    {
        if (!string.IsNullOrEmpty(emailInput.text))
        {
            //simple string preparation
            string email = emailInput.text.Trim();
            email = email.ToLower();

            PlayerStatsManager.Instance.GetCurrentPlayer(email, () =>
            {
                //runs when player login is successful
                //load next scene here or any other logic 
                debugText.text = "Succesfully logged in";
                SceneManager.LoadScene("Dashboard");
            },
            () => 
            {
                debugText.text = "No such player in the database. Kindly recheck your spelling or sign up"; 
            });
        }
        else
        {
            debugText.text = "email field cannot be blank";
        }
    }
    #endregion 
}
