using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using MyFarm.CropPlant;
using MyFarm.Save;
namespace MyFarm.Map
{
    public class GridMapManager : Singleton<GridMapManager>, Isavealbe
    {
        [Header("种地瓦片切换信息")]
        public RuleTile digTile;
        public RuleTile waterTile;
        private Tilemap digTileMap;
        private Tilemap waterTileMap;
        [Header("地图信息")]
        public List<MapData_SO> mapDataList;
        private Season currentSeason;

        //场景名字+坐标和对应瓦片信息
        private Dictionary<string, TileDetails> tileDetailsDic = new Dictionary<string, TileDetails>();
        private Dictionary<string, bool> fristLoadDic = new Dictionary<string, bool>();
        private Grid currentGrid;
        public List<ReapItem> ReapItemRadius;

        public string GUID => GetComponent<DataGUID>().guid;

        private void OnEnable()
        {
            EventHandler.ExecuteActionAfteiAnimation += OnExecuteActionAfteiAnimation;
            EventHandler.AfterLoadSceneEvent += OnAfterLoadSceneEvent;
            EventHandler.GameDayEvent += OnGameDayEvent;
            EventHandler.RefreshMap += Refresh;
        }



        private void Start()
        {
            foreach (var mapData in mapDataList)
            {
                fristLoadDic.Add(mapData.sceneName, true);
                InitTileDetailsDic(mapData);
            }
            Isavealbe save = this;
            save.RegisterSaveble();
        }
        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            EventHandler.ExecuteActionAfteiAnimation -= OnExecuteActionAfteiAnimation;
            EventHandler.AfterLoadSceneEvent -= OnAfterLoadSceneEvent;
            EventHandler.GameDayEvent -= OnGameDayEvent;
            EventHandler.RefreshMap -= Refresh;
        }





        //初始化字典信息
        private void InitTileDetailsDic(MapData_SO mapData)
        {
            foreach (TileProperty tileProperties in mapData.tileProperties)
            {
                TileDetails tileDetails = new TileDetails
                {
                    gridX = tileProperties.tileCoordinate.x,
                    gridY = tileProperties.tileCoordinate.y,

                };
                //字典的key
                string key = tileDetails.gridX + "x" + tileDetails.gridY + "y" + mapData.sceneName;
                if (GetTileDetails(key) != null)
                {
                    tileDetails = GetTileDetails(key);
                }
                switch (tileProperties.gridType)
                {
                    case GridType.Diggable:
                        tileDetails.canDig = tileProperties.boolTypeValue;
                        break;
                    case GridType.DropItem:
                        tileDetails.canDrop = tileProperties.boolTypeValue;
                        break;
                    case GridType.PlaceFurniture:
                        tileDetails.canPlaceFurniture = tileProperties.boolTypeValue;
                        break;
                    case GridType.NPCObstacle:
                        tileDetails.isNPCObstacle = tileProperties.boolTypeValue;
                        break;

                }
                if (GetTileDetails(key) != null)
                {
                    tileDetailsDic[key] = tileDetails;
                }
                else
                {
                    tileDetailsDic.Add(key, tileDetails);
                }


            }
        }
        //根据key返回瓦片信息
        public TileDetails GetTileDetails(string key)
        {
            if (tileDetailsDic.ContainsKey(key))
            {

                return tileDetailsDic[key];
            }
            else
            {
                return null;
            }
        }
        //根据网格坐标返回瓦片信息
        public TileDetails GetMapTileDetailsOnMousePosition(Vector3Int mouseGrid)
        {

            string key = mouseGrid.x + "x" + mouseGrid.y + "y" + SceneManager.GetActiveScene().name;

            return GetTileDetails(key);
        }
        private void OnExecuteActionAfteiAnimation(Vector3 mousePos, ItemDetails itemDetails)
        {
            var mouseGridPos = currentGrid.WorldToCell(mousePos);
            var currentTile = GetMapTileDetailsOnMousePosition(mouseGridPos);
            if (currentGrid != null)
            {
                //物品使用实际功能
                switch (itemDetails.itemType)
                {
                    case ItemType.Commodity:
                        EventHandler.CallDropItemEvent(itemDetails.itemID, mousePos, itemDetails.itemType);

                        break;
                    case ItemType.HoeTool:
                        SetDigGround(currentTile);
                        currentTile.daySinceDig = 0;
                        currentTile.canDig = false; ;
                        currentTile.canDrop = false;

                        //音效
                        EventHandler.CallGetSoundDeatil(soundName.Hoe);
                        break;
                    case ItemType.WaterTool:
                        SetWaterGround(currentTile);
                        currentTile.daySinceWater = 0;
                        //音效
                        EventHandler.CallGetSoundDeatil(soundName.Water);
                        break;
                    case ItemType.Seed:

                        EventHandler.CallPlantSeedEvent(itemDetails.itemID, currentTile);
                        Debug.Log("1");
                        EventHandler.CallDropItemEvent(itemDetails.itemID, mousePos, itemDetails.itemType);
                        Debug.Log("2");
                        EventHandler.CallGetSoundDeatil(soundName.Plant);
                        Debug.Log("3");
                        break;
                    case ItemType.BreakTool:
                    case ItemType.ChopTool:
                        Crop currentCrop = GetCropObject(mousePos);
                        if (currentCrop != null)
                            Debug.Log("使用了");
                        currentCrop.ProcessToolAction(itemDetails, currentCrop.tileDetails);
                        return;
                    case ItemType.CollectTool:
                        Debug.Log("CollectTool");
                        Crop cuurrentCrop = GetCropObject(mousePos);
                        //  执行收割方法
                        cuurrentCrop.ProcessToolAction(itemDetails, currentTile);
                        break;
                    case ItemType.ReapTool:
                        int count = 0;
                        for (int i = 0; i < ReapItemRadius.Count; i++)
                        {
                            ReapItemRadius[i].SpawnHarvest();
                            Destroy(ReapItemRadius[i].gameObject);
                            count++;
                            if (count >= 3)
                                break;
                        }
                        EventHandler.CallGetSoundDeatil(soundName.Reap);
                        break;
                    case ItemType.Furniture:
                        EventHandler.CallbuildItem(itemDetails.itemID, mousePos);
                        break;
                }
                UpdateTileDetails(currentTile);
            }
        }
        private void OnAfterLoadSceneEvent()
        {
            currentGrid = FindObjectOfType<Grid>();
            digTileMap = GameObject.FindWithTag("Dig").GetComponent<Tilemap>();
            waterTileMap = GameObject.FindWithTag("Water").GetComponent<Tilemap>();
            if (fristLoadDic[SceneManager.GetActiveScene().name])
            {
                //预先生成农作物改变gird的值
                EventHandler.CallGenerterCropEvent();
                fristLoadDic[SceneManager.GetActiveScene().name] = false;
            }

            Refresh();
        }
        //  通过鼠标点击判断是否点击到农作物
        public Crop GetCropObject(Vector3 mouseWorldPos)
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(mouseWorldPos);
            Crop currentCrop = null;
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].GetComponent<Crop>())
                {
                    currentCrop = colliders[i].GetComponent<Crop>();
                }
            }
            return currentCrop;
        }
        //通过鼠标检测碰撞体的身上是否有reapitem
        public bool HaveReapItemInRadius(Vector3 WorldPos, ItemDetails itemDetails)
        {

            ReapItemRadius = new List<ReapItem>();
            Collider2D[] coll = new Collider2D[20];
            Physics2D.OverlapCircleNonAlloc(WorldPos, itemDetails.itemUseRadius * 0.5f, coll);

            if (coll.Length > 0)
            {
                for (int i = 0; i < coll.Length; i++)
                {
                    if (coll[i] != null)
                    {

                        if (coll[i].GetComponent<ReapItem>())
                        {

                            ReapItemRadius.Add(coll[i].GetComponent<ReapItem>());
                            //coll[i].GetComponent<ReapItem>().SpawnHarvest();
                        }
                    }
                }
            }


            return ReapItemRadius.Count > 0;
        }

        //时间增加地面变化
        private void OnGameDayEvent(int day, Season season)
        {
            currentSeason = season;
            foreach (var tile in tileDetailsDic)
            {
                if (tile.Value.daySinceWater > -1)
                {
                    tile.Value.daySinceWater = -1;
                }
                if (tile.Value.daySinceDig > -1)
                {
                    tile.Value.daySinceDig++;
                }
                if (tile.Value.seedItemID != -1)
                {
                    tile.Value.growthDays++;
                    Debug.Log("growthDays" + tile.Value.growthDays);
                }
            }
            Refresh();
        }
        //设置挖地
        private void SetDigGround(TileDetails tileDetails)
        {
            Vector3Int pos = new Vector3Int(tileDetails.gridX, tileDetails.gridY, 0);
            if (digTileMap != null)
            {
                digTileMap.SetTile(pos, digTile);
            }
        }
        //设置浇水
        private void SetWaterGround(TileDetails tileDetails)
        {
            Vector3Int pos = new Vector3Int(tileDetails.gridX, tileDetails.gridY, 0);
            if (waterTileMap != null)
            {
                waterTileMap.SetTile(pos, waterTile);
            }
        }
        //更新地图信息到字典中
        private void Refresh()
        {
            if (digTile != null)
                digTileMap.ClearAllTiles();
            if (waterTile != null)
                waterTileMap.ClearAllTiles();
            foreach (var crop in FindObjectsOfType<Crop>())
            {
                Destroy(crop.gameObject);
            }
            DisplayMap(SceneManager.GetActiveScene().name);
        }
        public void UpdateTileDetails(TileDetails tileDetails)
        {
            string key = tileDetails.gridX + "x" + tileDetails.gridY + "y" + SceneManager.GetActiveScene().name;
            if (tileDetailsDic.ContainsKey(key))
            {
                tileDetailsDic[key] = tileDetails;
            }
            else
            {
                tileDetailsDic.Add(key, tileDetails);
            }

            DisplayMap(SceneManager.GetActiveScene().name);
        }
        //把字典中的地图信息更新到场景里
        private void DisplayMap(string sceneName)
        {
            foreach (var tile in tileDetailsDic)
            {
                var key = tile.Key;
                var tileDetails = tile.Value;
                if (key.Contains(sceneName))
                {
                    if (tileDetails.daySinceDig > -1)
                        SetDigGround(tileDetails);
                    if (tileDetails.daySinceWater > -1)
                        SetWaterGround(tileDetails);
                    //种子
                    if (tileDetails.seedItemID > -1)
                    {

                        EventHandler.CallPlantSeedEvent(tileDetails.seedItemID, tileDetails);
                    }
                }
            }
        }
        //获得当前场景的grid信息
        public bool GetGridNodeRange(string sceneName, out Vector2Int gridNodeRange, out Vector2Int gridOrigin)
        {
            gridNodeRange = Vector2Int.zero;
            gridOrigin = Vector2Int.zero;
            foreach (var mapdata in mapDataList)
            {
                if (mapdata.sceneName == sceneName)
                {
                    gridNodeRange.x = mapdata.gridWidth;
                    gridNodeRange.y = mapdata.gridHeight;

                    gridOrigin.x = mapdata.originX;
                    gridOrigin.y = mapdata.originY;
                    return true;
                }
            }
            return false;
        }

        public SaveData SaveGame()
        {
            SaveData saveData = new SaveData();
            saveData.tileDetailsDic = this.tileDetailsDic;
            saveData.fristLoadDic = this.fristLoadDic;
            return saveData;
        }

        public void LoadGame(SaveData saveData)
        {
            this.tileDetailsDic = saveData.tileDetailsDic;
            this.fristLoadDic = saveData.fristLoadDic;
        }
    }

}
