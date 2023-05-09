using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFarm.Map;

namespace MyFarm.CropPlant
{
    public class CropGenerator : MonoBehaviour
    {
        private Grid currentGrid;
        public int seedItemID;
        public int growthDay;
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            currentGrid = FindObjectOfType<Grid>();
            //GenerterCrop();
        }
        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            EventHandler.GenerterCropEvent += GenerterCrop;
        }
        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            EventHandler.GenerterCropEvent -= GenerterCrop;
        }
        private void GenerterCrop()
        {
            Vector3Int cropgridPos = currentGrid.WorldToCell(transform.position);
            if (seedItemID != 0)
            {
                TileDetails tileDetails = GridMapManager.Instance.GetMapTileDetailsOnMousePosition(cropgridPos);
                if (tileDetails == null)
                {
                    tileDetails = new TileDetails();
                    tileDetails.gridX = cropgridPos.x;
                    tileDetails.gridY = cropgridPos.y;
                }
                tileDetails.daySinceWater = -1;
                tileDetails.seedItemID = this.seedItemID;
                tileDetails.growthDays = this.growthDay;

                GridMapManager.Instance.UpdateTileDetails(tileDetails);
            }

        }

    }
}

