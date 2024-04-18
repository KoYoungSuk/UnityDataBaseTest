using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour 
{
    private string id = string.Empty;
    private string pw = string.Empty;

    [SerializeField] private TMP_InputField[] textarray = null;
    [SerializeField] private TextMeshProUGUI statustext = null;
    [SerializeField] private Button loginbutton = null; 

    private void Start()
    {
        if(PlayerPrefs.HasKey("id"))
        {
            textarray[0].text = PlayerPrefs.GetString("id"); 
        }
    }

    #region["ID 입력"] 
    public void OnValueChanged_ID()
    {
        id = textarray[0].text; 
    }
    #endregion

    #region["PW 입력"] 
    public void OnValueChanged_Password()
    {
        pw = textarray[1].text; 
    }
    #endregion

    #region["로그인"] 
    public void LoginButton()
    {
        StartCoroutine("LoginCoroutine"); 
    }
    #endregion

    #region["회원가입 화면으로 이동"] 
    public void GoSignup()
    {
        SceneManager.LoadScene("SignupScene"); 
    }
    #endregion

    #region["프로그램 종료"] 
    public void ExitProgram()
    {
        Application.Quit(); 
    }
    #endregion

    #region["Login Coroutine"] 
    private IEnumerator LoginCoroutine()
    {
        Global g = new Global();
        string phpname = "login.php";
        string url = g.GetUrl() + phpname; 

        if(!String.IsNullOrEmpty(id) || !String.IsNullOrEmpty(pw))
        {
            WWWForm form = new WWWForm();

            form.AddField("id", id);
            form.AddField("pw", pw);

            UnityWebRequest unityweb = UnityWebRequest.Post(url, form);
            yield return unityweb.SendWebRequest();
            if (unityweb.result == UnityWebRequest.Result.ConnectionError || unityweb.result == UnityWebRequest.Result.ProtocolError)
            {
                statustext.text = unityweb.error;
            }
            else
            {
                string status = unityweb.downloadHandler.text;
                Debug.Log("status: " + status);
                if (status.Contains("SUCCESS"))
                {
                    statustext.text = "Successfully Logined at [" + DateTime.Now.ToString() + "]";
                    loginbutton.interactable = false; 
                    yield return new WaitForSeconds(1.5f);
                    PlayerPrefs.SetString("id", status.Split(' ')[1]);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene("InformationScene");
                }
                else
                {
                    statustext.text = "Login Error: Check your ID and password!";
                }
            }
        }
        else
        {
            statustext.text = "You must input id and password."; 
        }
    }
    #endregion

}