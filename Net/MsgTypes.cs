using System;
using System.Collections.Generic;
using UnityEngine;

internal class MsgTypes
{
    //C2S
    public const int TimeInit = 1001;
    public const int TimeChange = 1002;
    public const int Move = 1003;
    public const int SwitchAnim = 1004;
    public const int UIOpen = 1005;
    public const int UIClose = 1006;
    public const int Login = 1007;

    public static Dictionary<int, Action<byte[]>> msgC2SHandles = new Dictionary<int, Action<byte[]>>
    {
        {TimeInit, TimeHandle.Instance.TimeInit},
        {TimeChange, TimeHandle.Instance.TimeChange},
        {Move, PlayerHandle.Instance.Move},
        {SwitchAnim, PlayerHandle.Instance.SwitchAnim},
        {UIOpen, UIHandle.Instance.OpenUI},
        {UIClose, UIHandle.Instance.CloseUI},
        {Login, LoginHandle.Instance.Login},
    };
    public static Dictionary<int, Action<byte[]>> msgS2CHandles = new Dictionary<int, Action<byte[]>>
    {
        {TimeInit, TimeHandle.Instance.TimeInit},
        {TimeChange, TimeHandle.Instance.TimeChange},
        {Move, PlayerHandle.Instance.Move},
        {SwitchAnim, PlayerHandle.Instance.SwitchAnim},
        {UIOpen, UIHandle.Instance.OpenUI},
        {UIClose, UIHandle.Instance.CloseUI},
        {Login, LoginHandle.Instance.Login},
    };
}

public class LoginMsgC2S
{
    public string text;
}
public class LoginMsgS2C
{
    public string account;
    public int result;//0成功 1失败
}