using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFarm.Inventory
{
    [RequireComponent(typeof(SlotUI))]
    public class ActionBar : MonoBehaviour
    {
        public KeyCode key;
        private SlotUI slotUI;
        private bool canuse = true;
        private void Awake()
        {
            slotUI = GetComponent<SlotUI>();
        }
        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {

            EventHandler.PauseGame += OnPauseGame;
        }

        private void OnPauseGame(GameState gameState)
        {
            canuse = gameState == GameState.GamePlay;
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            EventHandler.PauseGame -= OnPauseGame;
        }
        private void Update()
        {
            if (Input.GetKeyDown(key) && canuse)
            {
                if (slotUI.itemDetails != null)
                {
                    slotUI.isSelected = !slotUI.isSelected;
                    if (slotUI.isSelected)
                        slotUI.inventoryUI.UpdateSlotHighLight(slotUI.slotIndex);
                    else
                        slotUI.inventoryUI.UpdateSlotHighLight(-1);

                    EventHandler.CallItemSelectedEvent(slotUI.itemDetails, slotUI.isSelected);
                }
            }
        }
    }
}
