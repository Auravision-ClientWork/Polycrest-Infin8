using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LandingPageCB : MonoBehaviour
{
    public void HandleSignUpButton()
    {
        SceneManager.LoadScene("SignUp");
    }
    public void HandleLogInButton()
    {
        SceneManager.LoadScene("Login");
    }
}
