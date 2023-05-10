using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using System;
using UnityEngine;
using Google.Protobuf.Collections;

public class ProtoHelper
{
    //序列化接口
    public static byte[] ToBytes(object message)
    {
        Debug.Log(((Google.Protobuf.IMessage)message).ToByteArray().Length);
        return ((Google.Protobuf.IMessage)message).ToByteArray();

    }
    //反序列化接口
    public static T ToMessage<T>(byte[] bytes) where T : Google.Protobuf.IMessage, new()
    {
        T message = Activator.CreateInstance<T>();
        message.MergeFrom(bytes);
        return message;
    }
    //protobuf中的map转换为c#中的dictionary
    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(MapField<TKey, TValue> map)
    {
        Dictionary<TKey, TValue> dictionary = new();
        foreach (var item in map)
        {
            dictionary.Add(item.Key, item.Value);
        }
        return dictionary;
    }
    public static Vector3 Vector3Msg2Vector3(Vector3Msg vector3Msg)
    {
        return new Vector3(vector3Msg.X, vector3Msg.Y, vector3Msg.Z);
    }
    public static Vector2Int Vector3Msg2Vector2Int(Vector3Msg vector3Msg)
    {
        return new Vector2Int((int)vector3Msg.X, (int)vector3Msg.Y);
    }

    public static List<T> ListMsg2List<T>(RepeatedField<T> repeatedField)
    {
        List<T> list = new();
        foreach (var item in repeatedField)
        {
            list.Add(item);
        }
        return list;
    }

}
