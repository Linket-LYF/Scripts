using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace MyFarm.Inventory
{
    public class TradeUI : MonoBehaviour
    {
        public Image itemImage;
        public Text itemText;
        public InputField inputField;
        public Button confirm;
        public Button cancel;
        private ItemDetails itemDetails;
        private bool isSellTrade;
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            cancel.onClick.AddListener(CancelUI);
            confirm.onClick.AddListener(TradeItem);
        }
        public void SetUpTradeUI(ItemDetails item, bool isSell)
        {
            this.itemDetails = item;
            itemImage.sprite = item.itemIcon;
            itemText.text = item.itemname;
            isSellTrade = isSell;
            inputField.text = string.Empty;

        }
        public void CancelUI()
        {
            this.gameObject.SetActive(false);
        }
        public void TradeItem()
        {
            int amount = Convert.ToInt32(inputField.text);
            InventoryManager.Instance.TradeItem(itemDetails, amount, isSellTrade);
            CancelUI();
        }
    }
}
