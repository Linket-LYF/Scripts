using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFarm.CropPlant
{
    public class ReapItem : MonoBehaviour
    {
        private CropDetails cropDetails;
        private Transform playerPos => FindObjectOfType<Player>().transform;

        //根据id获得cropdetail初始化
        public void InitCorpData(int id)
        {
            cropDetails = CropManager.Instance.GetCropDetails(id);
        }

        public void SpawnHarvest()
        {
            //Debug.Log("执行了");
            for (int i = 0; i < cropDetails.producedItemID.Length; i++)
            {
                int amountToProduce;
                if (cropDetails.producedMinAmount[i] == cropDetails.producedMinAmount[i])
                {
                    amountToProduce = cropDetails.producedMinAmount[i];
                }
                else
                {
                    amountToProduce = Random.Range(cropDetails.producedMinAmount[i], cropDetails.producedMinAmount[i] + 1);
                }
                //执行生成指定物品的数量
                for (int j = 0; j < amountToProduce; j++)
                {
                    if (cropDetails.generateAtPlayerPositon)
                    {
                        EventHandler.CallHarvestAtPlayerPosition(cropDetails.producedItemID[i]);
                    }
                    else //世界生成
                    {
                        //生成方向
                        var dirx = transform.position.x > playerPos.position.x ? 1 : -1;
                        //一定范围内随机
                        var spawnPos = new Vector3(transform.position.x + Random.Range(dirx, cropDetails.spawnRadius.x * dirx),
                        transform.position.y + Random.Range(-cropDetails.spawnRadius.y, cropDetails.spawnRadius.y), 0);
                        EventHandler.CallInstantiateItemInScene(cropDetails.producedItemID[i], spawnPos);
                    }
                }
            }
        }
    }

}
