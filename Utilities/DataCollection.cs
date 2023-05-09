
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ItemDetails
{
    public int itemID;
    public string itemname;
    public ItemType itemType;
    public Sprite itemIcon;
    public Sprite itemOnWorldSprite;
    public string itemDescription;
    //可使用网格范围
    public int itemUseRadius;
    public bool canPickUp;
    public bool canDrop;
    public bool canCarry;
    public int itemPrice;
    [Range(0, 1)]
    public float sellPercentage;
}
[System.Serializable]
public struct InventoryItem
{
    public int itemID;
    public int itemAmount;
}
[System.Serializable]
public class SerializableVector3
{
    public float x, y, z;
    public SerializableVector3(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }
    public Vector3 ToVertor3()
    {
        return new Vector3(x, y, z);
    }
    public Vector2Int ToVector2Int()
    {
        return new Vector2Int((int)x, (int)y);
    }
}
//场景物品信息
[System.Serializable]
public class SceneItem
{
    public int itemID;
    public SerializableVector3 position;
    public int clientID;
}
//场景建造信息
[System.Serializable]
public class SceneFurniture
{
    public int itemID;
    public SerializableVector3 position;
    public int boxID;
    public int clientID;
}
//网格
[System.Serializable]
public class TileProperty
{
    public Vector2Int tileCoordinate;
    public GridType gridType;
    public bool boolTypeValue;
}
//瓦片信息
[System.Serializable]
public class TileDetails
{
    public int gridX, gridY;
    public bool canDig;
    public bool canDrop;
    public bool canPlaceFurniture;
    public bool isNPCObstacle;
    public int daySinceDig = -1;
    public int daySinceWater = -1;
    public int seedItemID = -1;
    public int growthDays = -1;
    public int daysSinceLastHarvest = -1;
}
//NPC信息
[System.Serializable]
public class NPCPosition
{
    public Transform npcPos;
    public string startSCene;
    public Vector3 oriPos;//初始坐标
}
[System.Serializable]
//场景路线
public class SceneRoute
{
    public string fromScene;
    public string toScene;
    public List<ScenePath> scenePathList;
}
[System.Serializable]
//场景路径
public class ScenePath
{
    public string sceneName;
    public Vector2Int fromPos;
    public Vector2Int toPos;
}

