using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginScript : MonoBehaviour
{
    NetListen_Login m_netListen = new NetListen_Login();
    public InputField m_inputField_account;
    public InputField m_inputField_psw;

    // Use this for initialization
    void Start ()
    {        
        //StartCoroutine(IGetData());
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void onClickLogin()
    {
        SceneLogScript.setLog("1");
        if ((m_inputField_account.text.CompareTo("") == 0) || (m_inputField_psw.text.CompareTo("") == 0))
        {
            SceneLogScript.setLog("2");
            ToastScript.createToast("请输入账号密码");

            return;
        }
        SceneLogScript.setLog("3");
        {
            JObject jo = new JObject();
            jo.Add("tag", "Login");
            jo.Add("account", m_inputField_account.text);
            jo.Add("psw", m_inputField_psw.text);
            SceneLogScript.setLog("4");
            NetUtil.getInstance().reqNet(m_netListen, "Login", jo.ToString());
            SceneLogScript.setLog("5");
        }
    }
    
    public void onClickRegister()
    {
        if ((m_inputField_account.text.CompareTo("") == 0) || (m_inputField_psw.text.CompareTo("") == 0))
        {
            ToastScript.createToast("请输入账号密码");

            return;
        }

        {
            JObject jo = new JObject();
            jo.Add("tag", "Register");
            jo.Add("account", m_inputField_account.text);
            jo.Add("psw", m_inputField_psw.text);

            NetUtil.getInstance().reqNet(m_netListen, "Register", jo.ToString());
        }
    }

    //----------------------------------------------------------------------
    IEnumerator IGetData()
    {
        // 从网络拉取
        if (true)
        {
            WWW www = new WWW(NetConfig.s_qiniuDownloadUrl + "code.json");
            yield return www;//等待Web服务器的反应

            // 错误
            if (www.error != null)
            {
                Debug.Log(www.error);
                yield return null;
            }

            SceneLogScript.setLog("0");
            //Debug.Log(www.text);
            SceneLogScript.setLog(www.text);
            Entity_code.getInstance().init(www.text);
        }
        // 从本地拉取
        else
        {
            yield return null;
            StreamReader sr = new StreamReader("Assets\\Resources\\Files\\code.json", System.Text.Encoding.GetEncoding("utf-8"));
            string jsonData = sr.ReadToEnd().ToString();
            sr.Close();

            //Debug.Log(jsonData);
            Entity_code.getInstance().init(jsonData);
        }
    }
}

class NetListen_Login : NetListen
{
    override public void onNetListen(string tag, string data)
    {
        int code = JsonUtils.getJsonValueOfInt(data, "code");
        Debug.Log("收到请求回调：tag = " + tag + "，data = " + data);

        //Debug.Log("code含义：" + Entity_code.getInstance().getCodeInfo(code));

        if (tag.CompareTo("Login") == 0)
        {
            if (code == (int)Consts_Code.Code.Code_Ok)
            {
                Debug.Log("登录成功");

                // 进入主界面
                SceneManager.LoadScene("MainScene");
            }
            else
            {
                Debug.Log("登录失败");
            }
        }
        else if (tag.CompareTo("Register") == 0)
        {
            if (code == (int)Consts_Code.Code.Code_Ok)
            {
                Debug.Log("注册成功");
            }
            else
            {
                Debug.Log("注册失败");
            }
        }
    }

    override public void onNetListenError(string tag)
    {
        Debug.Log("网络请求出错：tag = " + tag);
    }
}
