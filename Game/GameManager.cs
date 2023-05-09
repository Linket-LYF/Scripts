using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFarm.Inventory;
public class GameManager : Singleton<GameManager>
{
    Player player;
    Dictionary<string, OtherPlayer> otherPlayers = new();

    public Dictionary<string, bool> playerReady = new();
    public List<Item> sceneItems = new();
    public List<SceneFurniture> sceneFurnitures = new();

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
    public void SetPlayerReady(string id, bool ready)
    {
        playerReady[id] = ready;
    }

    public bool PlayerJoin(string id)
    {
        playerReady[id] = true;
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
        Destroy(otherPlayers[id].gameObject);
        otherPlayers.Remove(id);
    }
    public Item GetItem(int clientID)
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
    public void RemoveItem(int clientID)
    {
        Item item = GetItem(clientID);
        if (item != null)
        {
            sceneItems.Remove(item);
        }
    }
}
