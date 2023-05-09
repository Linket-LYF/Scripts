using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameUI : MonoBehaviour
{
    //实现跟随玩家头顶
    public Transform player;
    private void Update()
    {
        transform.position = player.position + new Vector3(0, 1f, 0);
    }

}
