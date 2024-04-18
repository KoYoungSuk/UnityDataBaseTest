using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Signup : MonoBehaviour
{
    //Signup Information 
    private string id = string.Empty;
    private string password = string.Empty;
    private string passwordc = string.Empty;
    private string Name = string.Empty;
    private string birthday = string.Empty;

    [SerializeField] private TMP_InputField[] textarray = null;
    [SerializeField] private TextMeshProUGUI statustext = null;
    [SerializeField] private Button signupbutton = null; 


    #region["아이디 입력"] 
    public void OnValueChanged_ID()
    {
        id = textarray[0].text; 
    }
    #endregion

    #region["비밀번호 입력"] 
    public void OnValueChanged_PW()
    {
        password = textarray[1].text; 
    }
    #endregion

    #region["비밀번호 확인 입력"] 
    public void OnValueChanged_PWC()
    {
        passwordc = textarray[2].text;  
    }
    #endregion

    #region["이름 입력"] 
    public void OnValueChanged_NAME()
    {
        name = textarray[3].text; 
    }
    #endregion

    #region["생년월일 입력"] 
    public void OnValueChanged_BIRTHDAY()
    {
        birthday = textarray[4].text; 
    }
    #endregion

    #region["회원가입"] 
    public void SignupButton() 
    {
        StartCoroutine("SignupCoroutine"); 
    }
    #endregion

    #region["뒤로가기"]
    public void GoBack()
    {
        SceneManager.LoadScene("LoginScene"); 
    }
    #endregion

    #region["회원가입 코루틴"] 
    private IEnumerator SignupCoroutine()
    {
        Global g = new Global();
        string url = g.GetUrl() + "signup.php";

        if (!string.IsNullOrEmpty(id)) //아이디가 NULL인지 아닌지 검사 
        {
            if(password.Equals(passwordc)) //비밀번호와 비밀번호 확인이 같은지 검사 
            {
                if (isPasswordCheck(password)) //길이 및 영문, 숫자, 특수문자 검사 
                {
                    WWWForm form = new WWWForm();
                    form.AddField("id", id);
                    form.AddField("pw", password);
                    form.AddField("name", name);
                    form.AddField("birthday", birthday);

                    UnityWebRequest unityweb = UnityWebRequest.Post(url, form);
                    yield return unityweb.SendWebRequest();
                    if (unityweb.result == UnityWebRequest.Result.ConnectionError || unityweb.result == UnityWebRequest.Result.ProtocolError)
                    {   //회원가입 실패 
                        statustext.text = unityweb.error;
                    }
                    else
                    {   //회원가입 성공 
                        string status = unityweb.downloadHandler.text;
                        Debug.Log(status); 
                        if(status.Equals("SUCCESS"))
                        {
                            statustext.text = "SUCCESSFULLY SIGNUPED AT [" + DateTime.Now.ToString() + "]";
                            signupbutton.interactable = false; 
                            yield return new WaitForSeconds(3f);
                            SceneManager.LoadScene("LoginScene");
                        }
                        else
                        {
                            statustext.text = "Signup Error"; 
                        }
                    }
                }
                else
                {
                    statustext.text = "Error: You need to input Lower Case English Character and Special Character. also Password's Length must be between 5 and 30."; 
                }
            }
            else
            {
                statustext.text = "Error: Password and Password Confirmed are not same!"; 
            }
        }
        else
        {
            statustext.text = "Error: ID is Empty."; 
        }
    }
    #endregion


    #region["비밀번호 검사"] 
    private bool isPasswordCheck(string _password)
    {
        if(string.IsNullOrEmpty(_password) || _password.Length <= 5 || _password.Length >= 30)
        {
            return false; 
        }
        //숫자, 영문, 특수문자 체크: 참고 사이트 https://easy-coding.tistory.com/87
        Regex regexPass = new Regex(@"^(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{9,}$", RegexOptions.IgnorePatternWhitespace);
        return regexPass.IsMatch(_password);
        //return true; 
    }
    #endregion
}

