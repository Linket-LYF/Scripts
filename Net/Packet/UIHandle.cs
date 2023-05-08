using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
public class UIHandle
{
    //单例模式
    private static UIHandle instance = new();
    public static UIHandle Instance => instance;
    public void OpenUI(byte[] data)
    {
        // UIOpenS2C s2c = JsonConvert.DeserializeObject<UIOpenS2C>(System.Text.Encoding.UTF8.GetString(data));
        // Debug.Log($"收到服务器的响应:{s2c}");
        // UIManger.Instance.OpenUI(s2c.UIName, s2c.args);
    }
    public void CloseUI(byte[] data)
    {
        // UICloseS2C c2s = JsonConvert.DeserializeObject<UICloseS2C>(System.Text.Encoding.UTF8.GetString(data));
        // s2c.UIName = c2s.UIName;
        // Debug.Log($"收到服务器的响应:{s2c}");
        // UIManger.Instance.CloseUI(s2c.UIName, s2c.args);
    }
    public void DestroyUI(byte[] data)
    {
        // UIDestroyS2C c2s = JsonConvert.DeserializeObject<UIDestroyS2C>(System.Text.Encoding.UTF8.GetString(data));
        // s2c.UIName = c2s.UIName;
        // Debug.Log($"收到服务器的响应:{s2c}");
        // UIManger.Instance.DestroyUI(s2c.UIName, s2c.args);
    }

}
public struct UIOpenC2S
{
    public string UIName;
}
public struct UIOpenS2C
{
    public string UIName;
    public object[] args;
}
public struct UICloseC2S
{
    public string UIName;
}
public struct UICloseS2C
{
    public string UIName;
    public object[] args;
}
public struct UIDestroyC2S
{
    public string UIName;
}
public struct UIDestroyS2C
{
    public string UIName;
    public object[] args;
}