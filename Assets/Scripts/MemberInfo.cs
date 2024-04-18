using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using TMPro;
using System;


public class MemberInfo : MonoBehaviour
{ 
    [SerializeField] private TextMeshProUGUI statustext = null;
    [SerializeField] private TextMeshProUGUI[] membertext_arr = null;

    private void Start()
    {
        StartCoroutine(GetMemberInfoCoroutine());
    }

    #region["로그아웃"] 
    public void Logout()
    {
        PlayerPrefs.DeleteAll(); 
        SceneManager.LoadScene("LoginScene"); 
    }
    #endregion

    #region["닫기"]
    public void CloseProgram()
    {
        Application.Quit(); 
    }
    #endregion


    #region["회원정보 가져오기"] 
    private IEnumerator GetMemberInfoCoroutine() 
    {
        Global g = new Global();
       
        string phpname = "information.php";
        string url = g.GetUrl() + phpname;
        Debug.Log("PlayerPrefs: " + PlayerPrefs.GetString("id")); 
        string id = PlayerPrefs.GetString("id");
        Debug.Log("ID from Login: " + id); 
        WWWForm form = new WWWForm();
        form.AddField("id", id); 
        UnityWebRequest unityweb = UnityWebRequest.Post(url, form);
        yield return unityweb.SendWebRequest();
        if (unityweb.result == UnityWebRequest.Result.ConnectionError || unityweb.result == UnityWebRequest.Result.ProtocolError)
        {   //Failed 
            statustext.text = unityweb.error;
        }
        else
        {
            string data = unityweb.downloadHandler.text;
            Debug.Log("data: " + data); 
            //JsonConvert를 이용한 역직렬화 
            List<MemberDTO> memberlist = JsonConvert.DeserializeObject<List<MemberDTO>>(data);  

            if(memberlist != null)
            {
                foreach (MemberDTO memberdto in memberlist)
                {
                    membertext_arr[0].text = "ID: " + memberdto.Id;
                    membertext_arr[1].text = "PASSWORD: " + memberdto.Password;
                    membertext_arr[2].text = "Name: " + memberdto.Name;
                    membertext_arr[3].text = "Birthday: " + memberdto.Birthday;
                }
                statustext.text = "SUCCESSFULLY LOADED AT [" + DateTime.Now.ToString() + "]"; 
            }
            else
            {
                statustext.text = "ERROR AT LOADING"; 
            }
        }
    }
    #endregion

}