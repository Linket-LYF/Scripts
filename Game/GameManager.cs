using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    OtherPlayer[] ohterPlayers;

    public Player Player
    {
        get => player;
        set => player = value;
    }
}
