using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryBag_SO", menuName = "Inventory/InventoryBag_SO")]
public class InventoryBag_SO : ScriptableObject
{
    public List<InventoryItem> itemBagList;
    public InventoryItem Find(int id)
    {
        return itemBagList.Find(i => i.itemID == id);
    }
}
