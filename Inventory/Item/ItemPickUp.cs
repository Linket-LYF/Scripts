using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyFarm.Inventory
{
    public class ItemPickUp : MonoBehaviour
    {
        /// <summary>
        /// Sent when another object enters a trigger collider attached to this
        /// object (2D physics only).
        /// </summary>
        /// <param name="other">The other Collider2D involved in this collision.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            Item item = other.GetComponent<Item>();
            if (item != null)
            {
                if (item.itemDetails.canPickUp)
                {
                    //拾取物品
                    InventoryManager.Instance.AddItem(item, true);
                    EventHandler.CallGetSoundDeatil(soundName.PickUp);
                }
            }
        }
    }
}

