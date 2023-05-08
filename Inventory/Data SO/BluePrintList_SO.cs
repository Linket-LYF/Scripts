using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BluePrintList_SO", menuName = "Inventory/BluePrintList_SO")]
public class BluePrintList_SO : ScriptableObject
{
    public List<BluePrint> bluePrintList;

    public BluePrint FindBluePrint(int id)
    {
        return bluePrintList.Find(b => b.itemID == id);
    }
}
[System.Serializable]
public class BluePrint
{
    public int itemID;
    public InventoryItem[] resourceItem = new InventoryItem[4];
    public GameObject bulidPrefab;
}
