using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyFarm.Save;

namespace MyFarm.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>, Isavealbe
    {

        [Header("物品数据")]
        public ItemDataList_SO itemDataList_SO;
        [Header("人物背包数据")]
        public InventoryBag_SO playerBag;
        public InventoryBag_SO playerBagTemp;
        [Header("建造物品")]
        public BluePrintList_SO bluePrint;
        public int playerMoney;
        private InventoryBag_SO currentBox;
        private Dictionary<string, List<InventoryItem>> boxItemDic = new Dictionary<string, List<InventoryItem>>();
        public int dicCnt => boxItemDic.Count;

        public string GUID => GetComponent<DataGUID>().guid;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            EventHandler.DropItemEvent += OnDropItemEvent;
            EventHandler.HarvestAtPlayerPosition += OnHarvestAtPlayerPosition;
            EventHandler.buildItem += OnbuildItem;
            EventHandler.BasicBagOpen += OnBasicBagOpen;
            EventHandler.StartNewGame += OnStartNewGame;
        }

        private void OnStartNewGame()
        {
            playerBag = Instantiate(playerBagTemp);
            playerMoney = 100;
            boxItemDic.Clear();
            //收集地图上的所有BoxItem类并添加进boxItemDic
            var boxItems = FindObjectsOfType<BoxItem>();
            foreach (var item in boxItems)
            {
                if (!boxItemDic.ContainsKey(item.boxName))
                {
                    boxItemDic.Add(item.boxName, new List<InventoryItem>());
                }
                boxItemDic[item.boxName].Add(item.inventoryItem);
            }
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemBagList);
        }

        private void Start()
        {
            //EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemBagList);
            Isavealbe saveData = this;
            saveData.RegisterSaveble();
        }
        private void OnDropItemEvent(int itemid, Vector3 pos, ItemType itemType)
        {
            RemoveItem(itemid, 1);
        }

        private void OnHarvestAtPlayerPosition(int itemID)
        {
            var index = GetItemIndexInBag(itemID);
            AddItemIndex(itemID, index, 1);
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemBagList);
        }

        private void OnDisable()
        {
            EventHandler.DropItemEvent -= OnDropItemEvent;
            EventHandler.HarvestAtPlayerPosition -= OnHarvestAtPlayerPosition;
            EventHandler.buildItem -= OnbuildItem;
            EventHandler.BasicBagOpen -= OnBasicBagOpen;
            EventHandler.StartNewGame -= OnStartNewGame;
        }

        private void OnBasicBagOpen(SlotType slotType, InventoryBag_SO bag_SO)
        {
            currentBox = bag_SO;

        }

        //移除建造消耗
        private void OnbuildItem(int id, Vector3 pos)
        {
            RemoveItem(id, 1);
            BluePrint bluePrint = InventoryManager.Instance.bluePrint.FindBluePrint(id);
            foreach (var item in bluePrint.resourceItem)
            {
                RemoveItem(item.itemID, item.itemAmount);
            }
        }

        //通过ID查找详细信息
        public ItemDetails GetItemDetails(int ID)
        {
            return itemDataList_SO.itemDetailsList.Find(i => i.itemID == ID);
        }
        //拾取物品
        public void AddItem(Item item, bool isDestory)
        {
            var index = GetItemIndexInBag(item.itemID);
            AddItemIndex(item.itemID, index, 1);
            if (isDestory)
            {
                Destroy(item.gameObject);
            }
            //更新UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemBagList);
        }
        //服务器拾取物品
        public void PickUpItem(int clientID, bool isDestory)
        {
            var item = GameManager.Instance.GetItem(clientID);
            var index = GetItemIndexInBag(item.itemID);
            AddItemIndex(item.itemID, index, 1);
            if (isDestory)
            {
                GameManager.Instance.RemoveItem(clientID);
                Destroy(item.gameObject);
            }
            //更新UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemBagList);
        }
        //背包是否有空位
        private bool CheckBagCapacity()
        {
            for (int i = 0; i < playerBag.itemBagList.Count; i++)
            {
                if (playerBag.itemBagList[i].itemID == 0)
                {
                    return true;
                }
            }
            return false;
        }
        //背包是否有相同物品
        private int GetItemIndexInBag(int ID)
        {
            for (int i = 0; i < playerBag.itemBagList.Count; i++)
            {
                if (playerBag.itemBagList[i].itemID == ID)
                {
                    return i;
                }
            }
            return -1;
        }
        //指定位置添加物品
        private void AddItemIndex(int ID, int index, int amount)
        {
            //背包没有相同物品同时有空位
            if (index == -1 && CheckBagCapacity())
            {
                var item = new InventoryItem { itemID = ID, itemAmount = amount };
                for (int i = 0; i < playerBag.itemBagList.Count; i++)
                {
                    if (playerBag.itemBagList[i].itemID == 0)
                    {
                        playerBag.itemBagList[i] = item;
                        break;
                    }
                }
            }
            else
            {
                int currentAmount = playerBag.itemBagList[index].itemAmount + amount;
                var item = new InventoryItem { itemID = ID, itemAmount = currentAmount };
                playerBag.itemBagList[index] = item;
            }
        }
        //交换背包数据
        public void SwapItem(int from, int to)
        {
            InventoryItem currentItem = playerBag.itemBagList[from];
            InventoryItem targetItem = playerBag.itemBagList[to];
            if (targetItem.itemID != 0)
            {
                playerBag.itemBagList[from] = targetItem;
                playerBag.itemBagList[to] = currentItem;
            }
            else
            {
                playerBag.itemBagList[to] = currentItem;
                playerBag.itemBagList[from] = new InventoryItem();
                //Debug.Log("playerBag.itemBagList[from].itemAmount"+playerBag.itemBagList[from].itemAmount);
            }
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemBagList);
        }
        //跨背包交换数据
        public void SwapItem(InventoryLocation locationfrom, int fromindex, InventoryLocation locationto, int toindex)
        {
            List<InventoryItem> fromItem = GetItemList(locationfrom);
            List<InventoryItem> toItem = GetItemList(locationto);
            InventoryItem currentItem = fromItem[fromindex];
            InventoryItem targetItem = toItem[toindex];
            //对方有物品且不相同
            if (targetItem.itemID != 0 && currentItem.itemID != targetItem.itemID)
            {
                fromItem[fromindex] = targetItem;
                toItem[toindex] = currentItem;
            }
            //两个相同物品
            else if (targetItem.itemID != 0 && currentItem.itemID == targetItem.itemID)
            {
                targetItem.itemAmount += currentItem.itemAmount;
                toItem[toindex] = targetItem;
                fromItem[fromindex] = new InventoryItem();
            }
            else
            {

                toItem[toindex] = currentItem;
                fromItem[fromindex] = new InventoryItem();
            }
            EventHandler.CallUpdateInventoryUI(locationfrom, fromItem);
            EventHandler.CallUpdateInventoryUI(locationto, toItem);
        }
        public List<InventoryItem> GetItemList(InventoryLocation location)
        {
            return location switch
            {
                InventoryLocation.Player => playerBag.itemBagList,
                InventoryLocation.Box => currentBox.itemBagList,
                _ => null
            };
        }
        //移除player背包物品数据
        private void RemoveItem(int itemID, int removeAmount)
        {
            var index = GetItemIndexInBag(itemID);
            if (playerBag.itemBagList[index].itemAmount > removeAmount)
            {
                var amount = playerBag.itemBagList[index].itemAmount - removeAmount;
                var item = new InventoryItem { itemID = itemID, itemAmount = amount };
                playerBag.itemBagList[index] = item;
            }
            else if (playerBag.itemBagList[index].itemAmount == removeAmount)
            {
                var item = new InventoryItem();
                playerBag.itemBagList[index] = item;
            }
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemBagList);
            //将背包数据上传给服务器
        }
        //交易物品
        public void TradeItem(ItemDetails itemDetails, int amount, bool isTrade)
        {
            int cost = itemDetails.itemPrice * amount;
            //获得物品背包位置
            int index = GetItemIndexInBag(itemDetails.itemID);
            if (isTrade)//卖
            {
                if (playerBag.itemBagList[index].itemAmount >= amount)
                {
                    RemoveItem(itemDetails.itemID, amount);
                    cost = (int)(cost * itemDetails.sellPercentage);
                    playerMoney += cost;
                }
            }
            else if (playerMoney - cost >= 0)//买
            {
                if (CheckBagCapacity())
                {
                    AddItemIndex(itemDetails.itemID, index, amount);
                }
                playerMoney -= cost;
            }
            //刷新UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemBagList);
        }
        //检测背包是否满足数量
        public bool CheckStock(int id)
        {
            var build = bluePrint.FindBluePrint(id);
            foreach (var detail in build.resourceItem)
            {
                InventoryItem item = playerBag.Find(detail.itemID);
                if (item.itemAmount >= detail.itemAmount)
                    continue;
                else
                {
                    return false;
                }
            }
            return true;
        }
        public List<InventoryItem> GetBoxItem(string key)
        {
            return boxItemDic.ContainsKey(key) ? boxItemDic[key] : null;
        }
        public void AddBoxItem(EmptyBox box)
        {
            string key = box.name + box.boxID;
            print(key);
            if (!boxItemDic.ContainsKey(key))
                boxItemDic.Add(key, box.boxBag.itemBagList);

        }

        public SaveGameC2SMsg SaveGame()
        {
            SaveGameC2SMsg saveData = new();
            saveData.Money = playerMoney;
            ListInventoryItemMsg listInventoryItemMsg = new ListInventoryItemMsg();
            foreach (var inventoryItem in playerBag.itemBagList)
            {
                listInventoryItemMsg.InventoryItems.Add(InventoryItem.InventoryItem2InventoryItemMsg(inventoryItem));
            }
            saveData.InventoryItems[playerBag.name] = listInventoryItemMsg;
            foreach (var item in boxItemDic)
            {
                ListInventoryItemMsg listMsg = new();
                foreach (var inventoryItem in item.Value)
                {
                    listMsg.InventoryItems.Add(InventoryItem.InventoryItem2InventoryItemMsg(inventoryItem));
                }
                saveData.InventoryItems[item.Key] = listMsg;
            }
            print(saveData.InventoryItems);
            return saveData;
        }

        public void LoadGame(SaveGameC2SMsg saveData)
        {
            //money
            this.playerMoney = saveData.Money;
            playerBag.itemBagList.Clear();
            boxItemDic.Clear();
            foreach (var inventoryItem in saveData.InventoryItems[playerBag.name].InventoryItems)
            {
                playerBag.itemBagList.Add(InventoryItem.InventoryItemMsg2InventoryItem(inventoryItem));
            }
            foreach (var item in saveData.InventoryItems)
            {
                if (boxItemDic.ContainsKey(item.Key))
                {
                    foreach (var inventoryItem in item.Value.InventoryItems)
                    {
                        boxItemDic[item.Key].Add(InventoryItem.InventoryItemMsg2InventoryItem(inventoryItem));
                    }
                }
            }
            print(playerBag.itemBagList);
            print(boxItemDic);
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemBagList);
        }
    }
}


