using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    public int furnitureID;
    public int clientID;
    private void Awake()
    {
        clientID = NetClientID.Instance.GetClientID();
        GameManager.Instance.sceneFurnitures.Add(this);
    }
}
