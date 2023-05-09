using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace MyFarm.Inventory
{
    public class SlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("组件获取")]
        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] public Image highLight;
        [SerializeField] private Button button;
        [Header("格子类型")]
        public SlotType slotType;
        public bool isSelected;
        //物品信息
        public ItemDetails itemDetails;
        private InventoryLocation location
        {
            get
            {
                return slotType switch
                {
                    SlotType.Bag => InventoryLocation.Player,
                    SlotType.Box => InventoryLocation.Box,
                    _ => InventoryLocation.Player
                };
            }
        }
        public int itemAmount;
        //当前第几个格子
        public int slotIndex;

        public InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            isSelected = false;
            if (itemDetails == null)
            {
                UpdateEmptySlot();
            }
        }
        //更新空的格
        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;

                inventoryUI.UpdateSlotHighLight(-1);
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            }
            itemDetails = null;
            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
            itemAmount = 0;//TODO:自己改
        }
        //更新格子
        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            slotImage.sprite = item.itemIcon;
            itemAmount = amount;
            amountText.text = amount.ToString();
            slotImage.enabled = true;
            button.interactable = true;
        }
        //点击格子
        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemDetails == null)
            {
                return;
            }
            isSelected = !isSelected;
            inventoryUI.UpdateSlotHighLight(slotIndex);
            //点击触发物品举起
            if (slotType == SlotType.Bag)
            {

                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            }
        }
        //开始拖拽
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemAmount != 0)
            {
                inventoryUI.dragImage.enabled = true;
                inventoryUI.dragImage.sprite = slotImage.sprite;
                //inventoryUI.dragImage.SetNativeSize();
                isSelected = true;
                inventoryUI.UpdateSlotHighLight(slotIndex);
            }
        }
        //正在拖拽
        public void OnDrag(PointerEventData eventData)
        {
            inventoryUI.dragImage.transform.position = Input.mousePosition;
        }
        //结束拖拽
        public void OnEndDrag(PointerEventData eventData)
        {
            inventoryUI.dragImage.enabled = false;
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null)
                {
                    return;
                }
                var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();
                int targetIndex = targetSlot.slotIndex;
                //自身背包范围内
                if (slotType == SlotType.Bag && targetSlot.slotType == SlotType.Bag)
                {
                    InventoryManager.Instance.SwapItem(slotIndex, targetIndex);
                }
                else if (slotType == SlotType.Shop && targetSlot.slotType == SlotType.Bag)//buy
                {
                    EventHandler.CallisSell(itemDetails, false);
                }
                else if (slotType == SlotType.Bag && targetSlot.slotType == SlotType.Shop)//sell
                {
                    EventHandler.CallisSell(itemDetails, true);
                }
                else if (slotType != SlotType.Shop && targetSlot.slotType != SlotType.Shop && slotType != targetSlot.slotType)//switch box
                {
                    //交换背包与箱子
                    InventoryManager.Instance.SwapItem(location, slotIndex, targetSlot.location, targetIndex);
                }
                inventoryUI.UpdateSlotHighLight(-1);

            }
            //世界地图生成物品
            // else
            // {
            //     if(itemDetails.canDrop)
            //     {
            //         var pos=Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,-Camera.main.transform.position.z));
            //         EventHandler.CallInstantiateItemInScene(itemDetails.itemID,pos);
            //     }

            // }
        }
    }
}
