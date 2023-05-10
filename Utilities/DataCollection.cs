
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

    public InventoryItemMsg InventoryItem2InventoryItemMsg(InventoryItem inventoryItem)
    {
        InventoryItemMsg inventoryItemMsg = new InventoryItemMsg();
        inventoryItemMsg.ItemID = inventoryItem.itemID;
        inventoryItemMsg.ItemAmount = inventoryItem.itemAmount;
        return inventoryItemMsg;
    }
    public InventoryItem InventoryItemMsg2InventoryItem(InventoryItemMsg inventoryItemMsg)
    {
        InventoryItem inventoryItem = new InventoryItem();
        inventoryItem.itemID = inventoryItemMsg.ItemID;
        inventoryItem.itemAmount = inventoryItemMsg.ItemAmount;
        return inventoryItem;
    }
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
    
    public SceneItemMsg SceneItem2SceneItemMsg(SceneItem sceneItem)
    {
        SceneItemMsg sceneItemMsg = new SceneItemMsg();
        sceneItemMsg.ItemID = sceneItem.itemID;
        sceneItemMsg.Pos = new Vector3Msg { X = sceneItem.position.x, Y = sceneItem.position.y, Z = sceneItem.position.z };
        sceneItemMsg.ClientID = sceneItem.clientID;
        return sceneItemMsg;
    }
    public SceneItem SceneItemMsg2SceneItem(SceneItemMsg sceneItemMsg)
    {
        SceneItem sceneItem = new SceneItem();
        sceneItem.itemID = sceneItemMsg.ItemID;
        sceneItem.position = new SerializableVector3(new Vector3(sceneItemMsg.Pos.X, sceneItemMsg.Pos.Y, sceneItemMsg.Pos.Z));
        sceneItem.clientID = sceneItemMsg.ClientID;
        return sceneItem;
    }
}

//场景建造信息
[System.Serializable]
public class SceneFurniture
{
    public int itemID;
    public SerializableVector3 position;
    public int boxID;
    public int clientID;

    public SceneFurnitureMsg SceneFurniture2SceneFurnitureMsg(SceneFurniture sceneFurniture)
    {
        SceneFurnitureMsg sceneFurnitureMsg = new SceneFurnitureMsg();
        sceneFurnitureMsg.ItemID = sceneFurniture.itemID;
        sceneFurnitureMsg.Pos = new Vector3Msg { X = sceneFurniture.position.x, Y = sceneFurniture.position.y, Z = sceneFurniture.position.z };
        sceneFurnitureMsg.BoxID = sceneFurniture.boxID;
        sceneFurnitureMsg.ClientID = sceneFurniture.clientID;
        return sceneFurnitureMsg;
    }
    public SceneFurniture SceneFurnitureMsg2SceneFurniture(SceneFurnitureMsg sceneFurnitureMsg)
    {
        SceneFurniture sceneFurniture = new SceneFurniture();
        sceneFurniture.itemID = sceneFurnitureMsg.ItemID;
        sceneFurniture.position = new SerializableVector3(new Vector3(sceneFurnitureMsg.Pos.X, sceneFurnitureMsg.Pos.Y, sceneFurnitureMsg.Pos.Z));
        sceneFurniture.boxID = sceneFurnitureMsg.BoxID;
        sceneFurniture.clientID = sceneFurnitureMsg.ClientID;
        return sceneFurniture;
    }
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

    public TileDetailsMsg TileDetails2TileDetailsMsg(TileDetails tileDetails)
    {
        TileDetailsMsg tileDetailsMsg = new TileDetailsMsg();
        tileDetailsMsg.GridX = tileDetails.gridX;
        tileDetailsMsg.GridY = tileDetails.gridY;
        tileDetailsMsg.CanDig = tileDetails.canDig;
        tileDetailsMsg.CanDrop = tileDetails.canDrop;
        tileDetailsMsg.CanPlaceFurniture = tileDetails.canPlaceFurniture;
        tileDetailsMsg.IsNPCObstacle = tileDetails.isNPCObstacle;
        tileDetailsMsg.DaySinceDig = tileDetails.daySinceDig;
        tileDetailsMsg.DaySinceWater = tileDetails.daySinceWater;
        tileDetailsMsg.SeedItemID = tileDetails.seedItemID;
        tileDetailsMsg.GrowthDays = tileDetails.growthDays;
        tileDetailsMsg.DaysSinceLastHarvest = tileDetails.daysSinceLastHarvest;
        return tileDetailsMsg;
    }
    public TileDetails TileDetailsMsg2TileDetails(TileDetailsMsg tileDetailsMsg)
    {
        TileDetails tileDetails = new TileDetails();
        tileDetails.gridX = tileDetailsMsg.GridX;
        tileDetails.gridY = tileDetailsMsg.GridY;
        tileDetails.canDig = tileDetailsMsg.CanDig;
        tileDetails.canDrop = tileDetailsMsg.CanDrop;
        tileDetails.canPlaceFurniture = tileDetailsMsg.CanPlaceFurniture;
        tileDetails.isNPCObstacle = tileDetailsMsg.IsNPCObstacle;
        tileDetails.daySinceDig = tileDetailsMsg.DaySinceDig;
        tileDetails.daySinceWater = tileDetailsMsg.DaySinceWater;
        tileDetails.seedItemID = tileDetailsMsg.SeedItemID;
        tileDetails.growthDays = tileDetailsMsg.GrowthDays;
        tileDetails.daysSinceLastHarvest = tileDetailsMsg.DaysSinceLastHarvest;
        return tileDetails;
    }
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

