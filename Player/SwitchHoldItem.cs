using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFarm.Inventory;

public class SwitchHoldItem : MonoBehaviour
{
    public BluePrintList_SO bluePrintList_SO;
    public SpriteRenderer holdItem;
    private Animator ani;
    private Player player;
    private void Awake()
    {

        holdItem.enabled = false;
        ani = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        //订阅物品举起事件
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.HarvestAtPlayerPosition += OnHarvestAtPlayerPosition;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        holdItem.enabled = false;
        ani.SetBool("isHoding", false);

    }

    //切换举物品动画及图片
    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {

        if (itemDetails.canCarry && isSelected)
        {
            if (itemDetails.itemType == ItemType.Furniture)
            {
                holdItem.enabled = true;
                holdItem.sprite = bluePrintList_SO.FindBluePrint(itemDetails.itemID).bulidPrefab.gameObject.GetComponent<SpriteRenderer>().sprite;
                ani.SetBool("isHoding", isSelected);
            }
            else
            {
                holdItem.enabled = true;
                holdItem.sprite = itemDetails.itemIcon;
                ani.SetBool("isHoding", isSelected);
                //abc
            }
        }
        else
        {

            holdItem.enabled = false;
            ani.SetBool("isHoding", false);
        }
    }


    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.HarvestAtPlayerPosition -= OnHarvestAtPlayerPosition;
    }

    private void OnHarvestAtPlayerPosition(int itemID)
    {
        Sprite item = InventoryManager.Instance.GetItemDetails(itemID).itemIcon;
        if (holdItem.enabled == false)
            StartCoroutine(ShowItem(item));
    }
    private IEnumerator ShowItem(Sprite item)
    {
        holdItem.sprite = item;
        holdItem.enabled = true;
        yield return new WaitForSeconds(1.5f);
        holdItem.enabled = false;
    }
}
