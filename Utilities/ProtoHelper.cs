using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using System;

public class ProtoHelper
{
    //序列化接口
    public static byte[] ToBytes(object message)
    {
        return ((Google.Protobuf.IMessage)message).ToByteArray();

    }
    //反序列化接口
    public static T ToMessage<T>(byte[] bytes) where T : Google.Protobuf.IMessage, new()
    {
        T message = Activator.CreateInstance<T>();
        message.MergeFrom(bytes);
        return message;
    }
}
