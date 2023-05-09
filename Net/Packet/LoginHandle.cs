using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using Newtonsoft.Json;
using MyFarm.Save;
public class LoginHandle
{
    //单例模式
    private static LoginHandle instance = new();
    public static LoginHandle Instance => instance;
    public void Login(byte[] data)
    {
        LoginMsgS2C response = JsonHelper.ToObject<LoginMsgS2C>(data);
        if (response.result == 0)
        {
            GameManager.Instance.Player.playerId = response.account;
            //开始游戏
            SaveLoadManager.Instance.Load(0);
            MenuManager.Instance.ShowTips("登录成功");
        }
        else
        {
            MenuManager.Instance.ShowTips("登录失败");
        }

    }

    public void Quit(byte[] data)
    {
        //QuitResponse response = QuitResponse.Parser.ParseFrom(data);
        //Debug.Log($"收到服务器的响应:{response}");
        // //跳转到登录场景
        // SceneManager.LoadScene("Login");
    }


}