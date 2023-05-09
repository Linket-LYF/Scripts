using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    OtherPlayer[] ohterPlayers;
    Dictionary<int,OtherPlayer> otherPlayers = new Dictionary<int, OtherPlayer>();

    public Dictionary<string,bool> playerReady = new Dictionary<string, bool>();

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
}
