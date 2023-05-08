using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MsgMap
{
    //单例模式
    private static MsgMap instance = new();
    public static MsgMap Instance => instance;
    private static Dictionary<int, Action<byte[]>> msgMap = new();
    public void Init()
    {
        //往msgMap里面添加协议ID和对应的处理方法
        foreach (var item in MsgTypes.msgC2SHandles)
        {
            //添加重复检查
            if (msgMap.ContainsKey(item.Key))
            {
                Debug.LogError($"协议ID:{item.Key}已经存在");
                continue;
            }
            else
            {
                msgMap.Add(item.Key, item.Value);
            }
        }
    }
    //往msgMap里面添加协议ID和对应的处理方法
    public void Add(int id, Action<byte[]> action)
    {
        if (!msgMap.ContainsKey(id))
        {
            msgMap.Add(id, action);
        }
        else
        {
            Debug.LogError($"协议ID:{id}已经存在");
        }
    }
    public void Handle(int id, byte[] data)
    {
        if (msgMap.ContainsKey(id))
        {
            msgMap[id]?.Invoke(data);
        }
        else
        {
            Debug.LogError($"没有找到协议ID:{id}");
        }
    }
}
