using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyFarm.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        public TextMeshProUGUI moneyText;
        public ItemTooltip itemTooltip;
        [Header("拖拽图片")]
        public Image dragImage;
        [Header("玩家背包")]
        [SerializeField] private GameObject bagUI;
        private bool bagOpen;
        [Header("同用背包")]

        public GameObject baseBag;
        public GameObject shopPrefabs;
        public GameObject boxPrefabs;
        [SerializeField] private SlotUI[] playerSlots;
        [SerializeField] private List<SlotUI> bagBasicSlots;
        [Header("交易UI")]
        public TradeUI tradeUI;
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            //给每一个格子序号
            for (int i = 0; i < playerSlots.Length; i++)
            {
                playerSlots[i].slotIndex = i;
            }
            bagOpen = bagUI.activeInHierarchy;
            moneyText.text = InventoryManager.Instance.playerMoney.ToString();

        }
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            //TODO切换移动端

            if (Input.GetKeyDown(KeyCode.P))
            {
                OpenBagUI();
            }
        }
        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            EventHandler.UpdateInventoryUI += OnUpdateInventoryUI;
            EventHandler.BeforeSceneUnloadEvent += BeforeSceneUnloadEvent;
            EventHandler.BasicBagOpen += OnBasicBagOpen;
            EventHandler.CloseBasicBagOpen += OnCloseBasicBagOpen;
            EventHandler.isSell += OnisSell;
        }

        private void OnBasicBagOpen(SlotType slotType, InventoryBag_SO shopSO)
        {

            GameObject slotPrefab = slotType switch
            {

                SlotType.Shop => shopPrefabs,

                SlotType.Box => boxPrefabs,
                _ => null,

            };
            baseBag.GetComponent<RectTransform>().pivot = new Vector2(0.85f, 0.5f);
            baseBag.SetActive(true);
            bagBasicSlots = new List<SlotUI>();
            for (int i = 0; i < shopSO.itemBagList.Count; i++)
            {
                var slot = Instantiate(slotPrefab, baseBag.transform.GetChild(0)).GetComponent<SlotUI>();
                slot.slotIndex = i;
                bagBasicSlots.Add(slot);
            }
            if (slotType == SlotType.Shop)
            {
                bagUI.GetComponent<RectTransform>().pivot = new Vector2(0.15f, 0.5f);
                bagUI.SetActive(true);
                bagOpen = true;
            }
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Box, shopSO.itemBagList);
        }
        //关闭背包
        private void OnCloseBasicBagOpen(SlotType slotType, InventoryBag_SO shop)
        {
            baseBag.SetActive(false);
            itemTooltip.gameObject.SetActive(false);
            UpdateSlotHighLight(-1);
            for (int i = 0; i < bagBasicSlots.Count; i++)
            {
                Destroy(bagBasicSlots[i].gameObject);
            }
            bagBasicSlots.Clear();
            if (slotType == SlotType.Shop)
            {
                bagUI.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                bagUI.SetActive(false);
                bagOpen = false;
            }
        }
        private void BeforeSceneUnloadEvent()
        {
            UpdateSlotHighLight(-1);
        }
        //更新背包UI
        private void OnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
        {
            switch (location)
            {
                case InventoryLocation.Player:
                    for (int i = 0; i < playerSlots.Length; i++)
                    {
                        if (list[i].itemAmount > 0)
                        {
                            var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
                            playerSlots[i].UpdateSlot(item, list[i].itemAmount);

                        }
                        else
                        {
                            playerSlots[i].UpdateEmptySlot();
                        }
                    }
                    break;
                case InventoryLocation.Box:
                    for (int i = 0; i < bagBasicSlots.Count; i++)
                    {
                        if (list[i].itemAmount > 0)
                        {
                            var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
                            bagBasicSlots[i].UpdateSlot(item, list[i].itemAmount);

                        }
                        else
                        {
                            bagBasicSlots[i].UpdateEmptySlot();
                        }
                    }
                    break;
            }
            moneyText.text = InventoryManager.Instance.playerMoney.ToString();
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            EventHandler.UpdateInventoryUI -= OnUpdateInventoryUI;
            EventHandler.BeforeSceneUnloadEvent -= BeforeSceneUnloadEvent;
            EventHandler.BasicBagOpen -= OnBasicBagOpen;
            EventHandler.CloseBasicBagOpen -= OnCloseBasicBagOpen;
            EventHandler.isSell -= OnisSell;
        }

        private void OnisSell(ItemDetails itemDetails, bool issell)
        {
            tradeUI.gameObject.SetActive(true);
            tradeUI.SetUpTradeUI(itemDetails, issell);
        }



        //背包启用
        public void OpenBagUI()
        {
            bagOpen = !bagOpen;
            bagUI.SetActive(bagOpen);
        }
        //更新高亮显示
        public void UpdateSlotHighLight(int index)
        {
            foreach (var slot in playerSlots)
            {
                if (slot.isSelected && slot.slotIndex == index)
                {
                    slot.highLight.gameObject.SetActive(true);
                }
                else
                {
                    slot.isSelected = false;
                    slot.highLight.gameObject.SetActive(false);
                }
            }
        }

    }
}

