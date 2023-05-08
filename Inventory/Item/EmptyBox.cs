using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFarm.Inventory
{
    public class EmptyBox : MonoBehaviour
    {
        public InventoryBag_SO boxBagTemplate;
        public InventoryBag_SO boxBag;
        private bool canOpen;
        private bool isOpen;
        public int boxID;
        private void OnEnable()
        {
            if (boxBag == null)
                boxBag = Instantiate(boxBagTemplate);
        }
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (!isOpen && Input.GetKeyDown(KeyCode.E))
            {
                EventHandler.CallBasicBagOpen(SlotType.Box, boxBag);

                isOpen = true;
            }
            if (isOpen && !canOpen)
            {
                EventHandler.CallCloseBasicBagOpen(SlotType.Box, boxBag);
                isOpen = false;
            }
            if (isOpen && Input.GetKeyDown(KeyCode.Escape))
            {
                EventHandler.CallCloseBasicBagOpen(SlotType.Box, boxBag);
                isOpen = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                canOpen = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                canOpen = false;
        }
        public void InitBox(int id)
        {
            boxID = id;
            string key = this.name + this.boxID;
            if (InventoryManager.Instance.GetBoxItem(key) != null)
            {
                boxBag.itemBagList = InventoryManager.Instance.GetBoxItem(key);
            }
            else//新建箱子
            {
                InventoryManager.Instance.AddBoxItem(this);
            }
        }
    }
}
