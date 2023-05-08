using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;

public class Client_UDP : MonoBehaviour
{
    UdpClient _udpClient;
    void Start()
    {
        _udpClient = new UdpClient();
        Receive();
    }

    private void Receive()
    {

    }

    public async void Send(byte[] data)
    {
        if (_udpClient != null)
        {
            try
            {
                int length = await _udpClient.SendAsync(data, data.Length, "127.0.0.1", 5088);
                if (data.Length == length)
                {
                    Debug.Log("发送成功");
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                _udpClient.Close();
                throw;
            }
        }
        else
        {
            Debug.Log("未连接");
        }
    }
    private void Update() {
        
    }
}
