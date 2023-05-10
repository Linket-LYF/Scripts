using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageHandle
{
    private static MessageHandle instance = new();
    public static MessageHandle Instance => instance;

    byte[] data = new byte[40960];
    int msgLength = 0;

    public void CopyToData(byte[] buffer, int length)
    {
        Array.Copy(buffer, 0, data, msgLength, length);
        msgLength += length;
        Handle();
    }
    private void Handle()
    {
        //包体大小(4)
        //协议ID(4)
        //包体(byte[])
        if (msgLength >= 8)
        {
            var _size = new byte[4];
            Array.Copy(data, 0, _size, 0, 4);
            var size = BitConverter.ToInt32(_size, 0);
            //本次要拿的长度
            var _length = 8 + size;
            if (msgLength >= _length)
            {
                //拿出id
                byte[] _id = new byte[4];
                Array.Copy(data, 4, _id, 0, 4);
                int id = BitConverter.ToInt32(_id, 0);

                //包体
                byte[] body = new byte[size];
                Array.Copy(data, 8, body, 0, size);

                if (msgLength > _length)
                {
                    for (int i = 0; i < msgLength - _length; i++)
                    {
                        data[i] = data[_length + i];
                    }
                }
                msgLength -= _length;
                Debug.Log($"收到服务器请求:{id}");
                //根据协议ID处理业务
                MsgMap.Instance.Handle(id, body);
            }
        }
    }
}
