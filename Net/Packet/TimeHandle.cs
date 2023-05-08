using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TimeHandle
{
    //单例模式
    private static TimeHandle instance = new();
    public static TimeHandle Instance => instance;
    public void TimeInit(byte[] data)
    {
    }
    public void TimeChange(byte[] data)
    {
    }
}