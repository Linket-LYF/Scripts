using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ItemHandle
{
    //单例模式
    private static ItemHandle instance = new();
    public static ItemHandle Instance => instance;
    public void Load(byte[] data)
    {
    }

    public void GetItem(byte[] data)
    {
    }
    public void UseItem(byte[] data)
    {
    }
    public void DropItem(byte[] data)
    {
    }
    public void PickItem(byte[] data)
    {
    }
    public void DeleteItem(byte[] data)
    {
    }
    public void UpdateItem(byte[] data)
    {
    }

}
public struct LoadC2S{
    public int id;
}
public struct LoadS2C{
    public int id;
}
public struct GetItemC2S{
    public int id;
}
public struct GetItemS2C{
    public int id;
}
public struct UseItemC2S{
    public int id;
    public int num;
}
public struct UseItemS2C{
    public int id;
    public int num;
}
public struct DropItemC2S{
    public int id;
    public int num;
    public Vector3 pos;
}
public struct DropItemS2C{
    public int id;
    public int num;
    public Vector3 pos;
}
public struct PickItemC2S{
    public int id;
    public int num;
}
public struct PickItemS2C{
    public int id;
    public int num;
}
public struct DeleteItemC2S{
    public int id;
    public int num;
}
public struct DeleteItemS2C{
    public int id;
    public int num;
}
public struct UpdateItemC2S{
    public int id;
    public int num;
}
public struct UpdateItemS2C{
    public int id;
    public int num;
}
