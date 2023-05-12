using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFarm.Save;
namespace MyFarm.CropPlant
{
    public class CropManager : Singleton<CropManager>
    {
        public CropDataList_SO cropDataList_SO;
        private Transform cropParent;
        private Grid currentGrid;
        private Season currentSeason;
        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            EventHandler.PlantSeedEvent += OnPlantSeedEvent;
            EventHandler.AfterLoadSceneEvent += OnAfterLoadSceneEvent;
            EventHandler.GameDayEvent += OnGameDayEvent;
        }
        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            EventHandler.PlantSeedEvent -= OnPlantSeedEvent;
            EventHandler.AfterLoadSceneEvent -= OnAfterLoadSceneEvent;
            EventHandler.GameDayEvent -= OnGameDayEvent;
        }

        private void OnGameDayEvent(int day, Season season)
        {
            currentSeason = season;
        }

        //种植事件
        private void OnPlantSeedEvent(int itemID, TileDetails tileDetails)
        {

            CropDetails currentCrop = GetCropDetails(itemID);

            //用于第一次种植
            if (currentCrop != null && SeasonCanSeed(currentCrop) && tileDetails.seedItemID == -1)
            {

                tileDetails.seedItemID = itemID;
                tileDetails.growthDays = 0;
                //显示农作物
                DisplayCrop(tileDetails, currentCrop);

            }
            //用来切换场景刷新农作物
            else if (tileDetails.seedItemID != -1)
            {
                //刷新显示农作物
                DisplayCrop(tileDetails, currentCrop);
                //Debug.Log("xianshi");

            }
        }
        //场景加载之后事件
        private void OnAfterLoadSceneEvent()
        {
            currentGrid = FindObjectOfType<Grid>();
            cropParent = GameObject.FindWithTag("CropParent").transform;
        }
        //得到种子信息
        public CropDetails GetCropDetails(int itemID)
        {
            return cropDataList_SO.cropDetailsList.Find(c => c.seedItemID == itemID);
        }
        //检测季节是否能够播种
        private bool SeasonCanSeed(CropDetails cropDetails)
        {
            for (int i = 0; i < cropDetails.seasons.Length; i++)
            {
                if (cropDetails.seasons[i] == currentSeason)
                    return true;

            }

            return false;
        }
        //显示农作物
        private void DisplayCrop(TileDetails tileDetails, CropDetails cropDetails)
        {

            //成长阶段
            int growStage = cropDetails.growthDay.Length;
            int currentStage = 0;
            int dayCounter = cropDetails.TotalGrowthDays;
            for (int i = growStage - 1; i >= 0; i--)
            {
                if (tileDetails.growthDays >= dayCounter)
                {
                    currentStage = i;
                    break;
                }
                dayCounter -= cropDetails.growthDay[i];
            }
            // for (int i = 0; i < growStage; i++)
            // {
            //     if (tileDetails.growthDays == cropDetails.growthDay[i])
            //     {
            //         currentStage = i;

            //         break;
            //     }
            // }
            GameObject cropPrefab = cropDetails.growthPrefabs[currentStage];

            Sprite cropSprite = cropDetails.growthSprites[currentStage];
            Vector3 pos = new Vector3(tileDetails.gridX + 0.5f, tileDetails.gridY + 0.5f, 0);

            GameObject cropInstance = Instantiate(cropPrefab, pos, Quaternion.identity, cropParent);
            cropInstance.GetComponentInChildren<SpriteRenderer>().sprite = cropSprite;
            cropInstance.GetComponent<Crop>().cropDetails = cropDetails;
            cropInstance.GetComponent<Crop>().tileDetails = tileDetails;

        }
    }

}
