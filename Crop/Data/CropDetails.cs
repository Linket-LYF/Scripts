using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CropDetails
{
    public int seedItemID;
    [Header("不同阶段生长时间")]
    public int[] growthDay;
    public int TotalGrowthDays

    {
        get
        {
            int amount = 0;
            foreach (var day in growthDay)
            {
                amount += day;
            }
            return amount;
        }
    }
    [Header("不同生成阶段物品prefabs")]
    public GameObject[] growthPrefabs;
    public Sprite[] growthSprites;
    [Header("可种植季节")]
    public Season[] seasons;
    [Space]
    [Header("收割信息")]
    public int[] harvestToolItemID;
    [Header("每种工具使用次数")]
    public int[] requireActionCount;
    [Header("转换新物品ID")]
    public int transferItemID;
    [Header("收割果实信息")]
    public int[] producedItemID;
    public int[] producedMinAmount;
    public int[] producedMaxAmount;
    public Vector2 spawnRadius;
    [Header("再次生成时间")]
    public int daysToRegrow;
    public int regrowTimes;
    [Header("Options")]

    public bool generateAtPlayerPositon;
    public bool hasAnimation;
    public bool hasParticalEffect;
    //特效音效
    public soundName soundName;
    //检测当前工具是否可用
    public bool IsCanUse(int toolID)
    {
        foreach (var tool in harvestToolItemID)
        {
            if (tool == toolID)
                return true;
        }
        return false;
    }
    //  工具使用次数
    public int ToolUseNumber(int toolID)
    {
        for (int i = 0; i < harvestToolItemID.Length; i++)
        {
            if (harvestToolItemID[i] == toolID)
                return requireActionCount[i];
        }
        return -1;
    }
}


