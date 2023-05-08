using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyFarm.CropPlant;
using MyFarm.Map;
using MyFarm.Inventory;

public class CursorManager : MonoBehaviour
{
    public Sprite normal;
    private Image cursorImage;
    //建造图标
    private Image buildImage;
    private RectTransform cursorCanvas;
    //鼠标检测
    private Camera mainCamera;
    private Grid currentGrid;
    //鼠标坐标
    private Vector3 mouseWorldPos;
    //瓦片坐标
    private Vector3Int mouseGridPos;
    //鼠标是否可用
    private bool cursorEnable;
    //鼠标是否可以点按
    private bool CursorPositonValid;
    //当前鼠标选中的物品
    private ItemDetails currentItem;

    //人物坐标
    private Transform playerPos => FindObjectOfType<Player>().transform;
    private void Start()
    {
        cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<RectTransform>();
        cursorImage = cursorCanvas.GetChild(0).GetComponent<Image>();
        SetCursorImage(normal);
        mainCamera = Camera.main;
        buildImage = cursorCanvas.GetChild(1).GetComponent<Image>();
        buildImage.gameObject.SetActive(false);

    }

    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.AfterLoadSceneEvent += OnAfterLoadSceneEvent;
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
    }
    private void Update()
    {
        if (cursorCanvas == null) return;
        cursorImage.transform.position = Input.mousePosition;

        if (cursorEnable)
        {
            CheckCursorValid();
            CheckPlayerInput();
        }

    }
    //获取地图信息
    private void OnAfterLoadSceneEvent()
    {
        currentGrid = FindObjectOfType<Grid>();

    }
    //物品选中事件
    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        if (!isSelected)
        {
            SetCursorvalid();
            currentItem = null;
            cursorEnable = false;
            buildImage.gameObject.SetActive(false);
            Debug.Log("取消选中");
        }
        else
        {

            currentItem = itemDetails;

            cursorEnable = true;
            //显示建造图片
            if (itemDetails.itemType == ItemType.Furniture)
            {
                buildImage.gameObject.SetActive(true);
                buildImage.sprite = itemDetails.itemOnWorldSprite;

            }
            else
            {
                buildImage.gameObject.SetActive(false);
            }
        }

    }
    private void OnBeforeSceneUnloadEvent()
    {
        cursorEnable = false;

    }


    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.AfterLoadSceneEvent -= OnAfterLoadSceneEvent;
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
    }
    //设置鼠标图片
    private void SetCursorImage(Sprite sprite)
    {
        cursorImage.sprite = sprite;
        cursorImage.color = new Color(1, 1, 1, 1);
    }
    //检测鼠标指针
    private void CheckCursorValid()
    {
        mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
        //Debug.Log(" mouseWorldPos:" + mouseWorldPos);
        mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
        var player = currentGrid.WorldToCell(playerPos.position);
        buildImage.rectTransform.position = Input.mousePosition;
        TileDetails currentTile = GridMapManager.Instance.GetMapTileDetailsOnMousePosition(mouseGridPos);
        //  判断物品在可扔的范围内
        if (Mathf.Abs(mouseGridPos.x - player.x) > currentItem.itemUseRadius || Mathf.Abs(mouseGridPos.y - player.y) > currentItem.itemUseRadius)
        {
            SetCursorInvalid();
            return;
        }
        //判断物品是否可
        if (currentTile != null)
        {
            CropDetails currentCropDetails = CropManager.Instance.GetCropDetails(currentTile.seedItemID);
            Crop crop = GridMapManager.Instance.GetCropObject(mouseWorldPos);
            switch (currentItem.itemType)
            {
                case ItemType.Commodity:
                    if (currentTile.canDrop && currentItem.canDrop) SetCursorvalid(); else SetCursorInvalid();
                    break;
                case ItemType.HoeTool:
                    if (currentTile.canDig) SetCursorvalid(); else SetCursorInvalid();
                    break;
                case ItemType.WaterTool:
                    if (currentTile.daySinceDig > -1 && currentTile.daySinceWater == -1) SetCursorvalid(); else SetCursorInvalid();
                    break;
                case ItemType.Seed:
                    if (currentTile.daySinceDig > -1 && currentTile.seedItemID == -1) SetCursorvalid(); else SetCursorInvalid();
                    break;

                case ItemType.CollectTool:
                    if (currentCropDetails != null)
                    {
                        if (currentCropDetails.IsCanUse(currentItem.itemID))
                            if (currentTile.growthDays >= currentCropDetails.TotalGrowthDays) SetCursorvalid(); else SetCursorInvalid();
                    }
                    else
                        SetCursorInvalid();
                    break;
                case ItemType.BreakTool:
                case ItemType.ChopTool:
                    if (crop != null)
                    {
                        if (crop.isHarvest && crop.cropDetails.IsCanUse(currentItem.itemID)) SetCursorvalid(); else SetCursorInvalid();

                    }
                    else SetCursorInvalid();
                    break;
                case ItemType.ReapTool:

                    if (GridMapManager.Instance.HaveReapItemInRadius(mouseWorldPos, currentItem))
                    {

                        SetCursorvalid();
                        Debug.Log("SetCursorvalid();");
                    }

                    else SetCursorInvalid();
                    break;
                case ItemType.Furniture:
                    var bluePrint = InventoryManager.Instance.bluePrint.FindBluePrint(currentItem.itemID);
                    if (currentTile.canPlaceFurniture && InventoryManager.Instance.CheckStock(currentItem.itemID) && !HaveFurnitureInRadius(bluePrint))
                        SetCursorvalid();
                    else SetCursorInvalid();
                    break;
            }
        }
        else
        {

            SetCursorInvalid();
        }

    }
    //鼠标可用不可用
    private void SetCursorInvalid()
    {
        CursorPositonValid = false;
        cursorImage.color = new Color(1, 0, 0, 0.5f);
        buildImage.color = new Color(1, 0, 0, 0.5f);
    }
    private void SetCursorvalid()
    {
        CursorPositonValid = true;
        cursorImage.color = new Color(1, 1, 1, 1);
        buildImage.color = new Color(1, 1, 1, 0.5f);
    }
    //鼠标点击
    private void CheckPlayerInput()
    {
        if (Input.GetMouseButtonDown(0) && CursorPositonValid)
        {
            EventHandler.CallMouseClickEvent(mouseWorldPos, currentItem);
        }
    }
    private bool HaveFurnitureInRadius(BluePrint bluePrint)
    {
        var buildItem = bluePrint.bulidPrefab;
        Vector2 point = mouseWorldPos;
        var size = buildItem.GetComponent<BoxCollider2D>().size;
        var otherColl = Physics2D.OverlapBox(point, size, 0);
        if (otherColl != null)
            return otherColl.GetComponent<Furniture>();
        else
            return false;
    }

}
