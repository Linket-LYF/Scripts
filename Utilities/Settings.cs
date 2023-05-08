using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Settings
{
    public const float itemFadeDuration = 0.35f;
    public const float fadeDuration = 1.5f;
    public const float targetAlpha = 0.75f;
    //时间相关
    public const float secondThreshold = 0.0001f;//数值越小时间越快
    public const int secondHold = 59;
    public const int minuteHold = 59;
    public const int hourHold = 23;
    public const int dayHold = 30;
    public const int seasonHold = 3;
    //灯光
    public const float lightChangeDuration = 25f;
    public static TimeSpan morningTime => new TimeSpan(5, 0, 0);
    public static TimeSpan nightTime => new TimeSpan(18, 0, 0);
    public static Vector3 playerStartPos = new Vector3(-1.75f, -5.7f, 0f);
}
