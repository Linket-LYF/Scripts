using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEvent : MonoBehaviour
{
    public InventoryBag_SO shopBag;
    private bool isOpen;
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            //关闭背包
            CloseBag();
        }
    }
    public void OpenBag()
    {
        isOpen = true;
        EventHandler.CallBasicBagOpen(SlotType.Shop, shopBag);
        EventHandler.CallPauseGame(GameState.Pause);
    }
    public void CloseBag()
    {
        isOpen = false;
        EventHandler.CallCloseBasicBagOpen(SlotType.Shop, shopBag);
        EventHandler.CallPauseGame(GameState.GamePlay);
    }
}
