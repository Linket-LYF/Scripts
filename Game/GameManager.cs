using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFarm.Inventory;
public class GameManager : Singleton<GameManager>
{
    Player player;
    OtherPlayer[] ohterPlayers;
    Dictionary<int,OtherPlayer> otherPlayers = new Dictionary<int, OtherPlayer>();

    public Dictionary<string,bool> playerReady = new Dictionary<string, bool>();
    public List<Item> sceneItems = new List<Item>();
    public List<SceneFurniture> sceneFurnitures = new List<SceneFurniture>();

    public Player Player
    {
        get => player;
        set => player = value;
    }
    
    public Dictionary<string, OtherPlayer> OtherPlayers
    {
        get => otherPlayers;
        set => otherPlayers = value;
    }
    public void SetPlayerReady(int id, bool ready)
    {
        playerReady[id] = ready;
    }

    public bool PlayerJoin(int id)
    {
        playerReady[id]=true;
        foreach (var item in playerReady)
        {
            if (!item.Value)
            {
                return false;
            }
        }
        return true;
    }

    public void OtherPlayerJoin(string id)
    {
        otherPlayers[id] = new OtherPlayer();
        EventHandler.CallOtherPlayerJoin(id);
    }
    public void OtherPlayerLeave(string id)
    {
        otherPlayers[id].Destroy();
        otherPlayers.Remove(id);
    }
    public Item GetItem(string clientID)
    {
        foreach (var item in sceneItems)
        {
            if (item.clientID == clientID)
            {
                return item;
            }
        }
        return null;
    }
    public void RemoveItem(string clientID)
    {
        Item item = GetItem(clientID);
        if (item != null)
        {
            sceneItems.Remove(item);
        }
    }
}
