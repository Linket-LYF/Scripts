using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ScheduleDetails : IComparable<ScheduleDetails>
{
    public int hour, minute, day;
    public int priority; //优先级越小优先执行
    public Season season;
    public string targetScene;
    public Vector2Int targetGridPos;
    public AnimationClip animationClip;
    public bool interactable;//可交互的
    public int Time => (hour * 100) + minute;

    public ScheduleDetails(int hour, int minute, int day, int priority, Season season, string targetScene, Vector2Int targetGridPos,
     AnimationClip animationClip, bool interactable)
    {
        this.hour = hour;
        this.minute = minute;
        this.day = day;
        this.priority = priority;
        this.season = season;
        this.targetScene = targetScene;
        this.targetGridPos = targetGridPos;
        this.animationClip = animationClip;
        this.interactable = interactable;
    }

    int IComparable<ScheduleDetails>.CompareTo(ScheduleDetails other)
    {
        if (Time == other.Time)
        {
            if (priority > other.priority)
                return 1;
            else
                return -1;
        }
        else if (Time > other.Time)
            return 1;
        else if (Time < other.Time)
            return -1;
        return 0;
    }
}
