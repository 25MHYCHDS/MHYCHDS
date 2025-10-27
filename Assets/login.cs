using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class loginl : MonoBehaviour
{
    public TMP_InputField userName;
    public TMP_InputField userMiMa;
    public Text missTiShi;
    public void OnLoginButtonClick()
    {
        string username = userName.text;
        string password = userMiMa.text;
        if (username == "MiHoYo" && password == "666") 
        {
            SceneManager.LoadScene("scene0");
        }
        else 
        {
            missTiShi.text = "Please try again";
            missTiShi.enabled = true;
        }
    }
}
