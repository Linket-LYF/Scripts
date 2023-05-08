using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFarm.CropPlant;

namespace MyFarm.Inventory
{
    public class Item : MonoBehaviour
    {
        public int itemID;
        private SpriteRenderer spriteRenderer;
        public ItemDetails itemDetails;
        private BoxCollider2D coll;


        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            coll = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            if (itemID != 0)
            {
                InitItem(itemID);
            }
        }
        //初始化物品
        public void InitItem(int ID)
        {
            itemID = ID;
            itemDetails = InventoryManager.instance.GetItemDetails(itemID);
            if (itemDetails != null)
            {
                spriteRenderer.sprite = itemDetails.itemOnWorldSprite != null ? itemDetails.itemOnWorldSprite : itemDetails.itemIcon;
                //修改碰撞体尺寸
                Vector2 newSize = new Vector2(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y);
                coll.size = newSize;
                coll.offset = new Vector2(0, spriteRenderer.sprite.bounds.center.y);
            }
            if (itemDetails.itemType == ItemType.ReapableScenery)
            {
                gameObject.AddComponent<ReapItem>();
                gameObject.GetComponent<ReapItem>().InitCorpData(itemID);
                gameObject.AddComponent<ItemShake>();

            }
        }




    }
}


