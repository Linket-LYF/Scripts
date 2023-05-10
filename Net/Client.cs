using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net.Sockets;
using System.Text;
using System;

public class Client
{
    static Client instance = new Client();
    //实现五秒的心跳包
    public int heartBeatTime = 5;
    public static Client Instance => instance;
    TcpClient client;
    public async void Start()
    {
        client = new TcpClient();
        try
        {
            //打开等待连接ui
            //
            await client.ConnectAsync("127.0.0.1", 2052);
            //关闭等待连接ui

            Debug.Log("连接成功");
            Receive();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            throw;
        }
    }
    public async void Connect()
    {
        try
        {
            await client.ConnectAsync("127.0.0.1", 2052);
            Debug.Log("连接成功");
            Receive();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            throw;
        }
    }
    public async void Send(byte[] data)
    {
        if (client == null)
        {
            Debug.Log("客户端未连接");
            return;
        }
        try
        {
            await client.GetStream().WriteAsync(data, 0, data.Length);
            Debug.Log("发送成功");
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            CloseClient();
        }
    }
    public async void Receive()
    {
        while (client.Connected)
        {
            byte[] buff = new byte[4096];
            try
            {
                int length = await client.GetStream().ReadAsync(buff, 0, buff.Length);
                if (length > 0)
                {
                    Debug.Log($"接收到的数据长度:{length}");
                    MessageHandle.Instance.CopyToData(buff, length);
                    Array.Clear(buff, 0, buff.Length);
                }
                else
                {
                    Debug.Log("服务器断开连接");
                    CloseClient();
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
                CloseClient();
            }
        }
    }
    //按照格式封装后发送消息
    public void SendToClient(int id, string str)
    {
        //转换成byte[]
        var body = Encoding.UTF8.GetBytes(str);
        //包体大小(4) 消息ID(4) 包体内容
        byte[] send_buff = new byte[body.Length + 8];
        int size = body.Length;
        var _size = BitConverter.GetBytes(size);
        var _id = BitConverter.GetBytes(id);

        Array.Copy(_size, 0, send_buff, 0, 4);
        Array.Copy(_id, 0, send_buff, 4, 4);
        Array.Copy(body, 0, send_buff, 8, body.Length);
        Debug.Log(str);
        Send(send_buff);
    }
    public void SendToClient(int id,byte[] data)
    {
        //包体大小(4) 消息ID(4) 包体内容
        byte[] send_buff = new byte[data.Length + 8];
        int size = data.Length;
        var _size = BitConverter.GetBytes(size);
        var _id = BitConverter.GetBytes(id);

        Array.Copy(_size, 0, send_buff, 0, 4);
        Array.Copy(_id, 0, send_buff, 4, 4);
        Array.Copy(data, 0, send_buff, 8, data.Length);
        print("发送数据长度:"+send_buff.Length);
        Send(send_buff);
    }
    public void CloseClient()
    {
        client.Close();
        //TODO等待几秒尝试重新连接
    }
    //心跳包
    public async void HeartBeat()
    {
        while (client.Connected)
        {
            await System.Threading.Tasks.Task.Delay(heartBeatTime * 1000);
            SendToClient(1000, "");
        }
    }
}

// using System;
// using System.Net.Sockets;

// public class NetworkManager {

//     private TcpClient tcpClient;
//     private NetworkStream networkStream;

//     public void Connect(string ipAddress, int port, Action onConnectedCallback) {
//         tcpClient = new TcpClient();
//         tcpClient.BeginConnect(ipAddress, port, (result) => {
//             if (tcpClient.Connected) {
//                 networkStream = tcpClient.GetStream();
//                 onConnectedCallback?.Invoke(); // invoke the callback function if it's not null
//             }
//             else {
//                 Debug.LogError("Failed to connect to server.");
//             }
//         }, null);
//     }

//     public void SendData(byte[] data) {
//         if (networkStream != null) {
//             networkStream.BeginWrite(data, 0, data.Length, (result) => {
//                 networkStream.EndWrite(result);
//             }, null);
//         }
//     }

//     public void ReceiveData(int bufferSize, Action<byte[]> onDataReceivedCallback) {
//         byte[] buffer = new byte[bufferSize];
//         if (networkStream != null) {
//             networkStream.BeginRead(buffer, 0, bufferSize, (result) => {
//                 int bytesRead = networkStream.EndRead(result);
//                 byte[] receivedData = new byte[bytesRead];
//                 Array.Copy(buffer, receivedData, bytesRead);
//                 onDataReceivedCallback?.Invoke(receivedData); // invoke the callback function with the received data if it's not null
//             }, null);
//         }
//     }

//     public void CloseConnection() {
//         if (networkStream != null) {
//             networkStream.Close();
//             networkStream = null;
//         }
//         if (tcpClient != null) {
//             tcpClient.Close();
//             tcpClient = null;
//         }
//     }
// }
