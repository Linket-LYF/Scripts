using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicButton : MonoBehaviour
{
    public InputField inputfield;

    public void SendLogicMsg()
    {
        print(inputfield.text);
        var loginMsgC2S = new LoginMsgC2S
        {
            text = inputfield.text
        };
        Client.Instance.SendToClient(MsgTypes.Login, JsonHelper.ToJson(loginMsgC2S));
    }
}

