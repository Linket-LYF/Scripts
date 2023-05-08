using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MapHandle
{
    //单例模式
    private static MapHandle instance = new();
    public static MapHandle Instance => instance;
    public void Load(byte[] data)
    {

    }
    public void Generate(byte[] data)
    {

    }
    public void Destroy(byte[] data)
    {

    }

}