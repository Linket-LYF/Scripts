using System.Collections;
using System.Collections.Generic;
using MyFarm.Save;
using UnityEngine;
using UnityEngine.UI;

public class LogicButton : MonoBehaviour
{
    public InputField inputfield;

    public void SendLogicMsg()
    {
        var loginMsgC2S = new LoginMsgC2S
        {
            text = inputfield.text
        };
        //EventHandler.CallStartNewGame(1);
        
        Client.Instance.SendToClient(MsgTypes.Login, JsonHelper.ToJson(loginMsgC2S));
    }
}

