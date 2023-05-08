using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    private Animator ani;
    public CropDetails cropDetails;
    public TileDetails tileDetails;
    private int count;
    private Transform playerTransform => FindObjectOfType<Player>().transform;
    public bool isHarvest => tileDetails.growthDays >= cropDetails.TotalGrowthDays;

    public void ProcessToolAction(ItemDetails tool, TileDetails currentDetails)
    {
        //Debug.Log("ProcessToolAction cropDetailsID:" + cropDetails.seedItemID);
        tileDetails = currentDetails;
        int requireActionCount = cropDetails.ToolUseNumber(tool.itemID);
        if (requireActionCount == -1) return;
        //  执行动画
        ani = GetComponentInChildren<Animator>();
        //计数器
        if (count < requireActionCount)
        {
            count++;
            //  播放声音 粒子效果
            if (ani != null && cropDetails.hasAnimation)
            {
                if (playerTransform.position.x < transform.position.x)
                {
                    ani.SetTrigger("Right");

                }
                else
                    ani.SetTrigger("Left");

            }
            if (cropDetails.soundName != soundName.None)
            {
                var soundDetail = AudioManager.Instance.soundDataList_SO.FindSoundDetail(cropDetails.soundName);
                EventHandler.CallInitSoundEffect(soundDetail);
            }
        }
        if (count >= requireActionCount)
        {
            if (cropDetails.generateAtPlayerPositon)
            {
                //  生成农作物
                SpawnHarvest();
            }
            //  砍树世界掉落
            else if (cropDetails.hasAnimation)
            {


                if (playerTransform.position.x < transform.position.x)
                {
                    ani.SetTrigger("FallDownRight");

                }
                else
                    ani.SetTrigger("FallDownLeft");
                EventHandler.CallGetSoundDeatil(soundName.TreeFalling);
                StartCoroutine(HarvestAfterAnimation());
            }
            else
            {
                SpawnHarvest();
            }
        }
    }
    private IEnumerator HarvestAfterAnimation()
    {
        while (!ani.GetCurrentAnimatorStateInfo(0).IsName("End"))
        {
            yield return null;
        }
        SpawnHarvest();
    }
    //  生成农作物
    public void SpawnHarvest()
    {
        Debug.Log("执行了");
        for (int i = 0; i < cropDetails.producedItemID.Length; i++)
        {
            int amountToProduce;
            if (cropDetails.producedMinAmount[i] == cropDetails.producedMaxAmount[i])
            {
                amountToProduce = cropDetails.producedMinAmount[i];
            }
            else
            {
                amountToProduce = Random.Range(cropDetails.producedMinAmount[i], cropDetails.producedMaxAmount[i] + 1);
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
                    var dirx = transform.position.x > playerTransform.position.x ? 1 : -1;
                    //一定范围内随机
                    var spawnPos = new Vector3(transform.position.x + Random.Range(dirx, cropDetails.spawnRadius.x * dirx),
                    transform.position.y + Random.Range(-cropDetails.spawnRadius.y, cropDetails.spawnRadius.y), 0);
                    EventHandler.CallInstantiateItemInScene(cropDetails.producedItemID[i], spawnPos);
                }
            }
        }
        if (tileDetails != null)
        {
            tileDetails.daysSinceLastHarvest++;
            //  重生
            if (cropDetails.daysToRegrow > 0 && tileDetails.daysSinceLastHarvest < cropDetails.regrowTimes)
            {
                tileDetails.growthDays = cropDetails.TotalGrowthDays - cropDetails.daysToRegrow;
                //  刷新种子
                EventHandler.CallRefreshMap();
            }
            else
            {
                tileDetails.daysSinceLastHarvest = -1;
                tileDetails.seedItemID = -1;
                EventHandler.CallRefreshMap();
                Debug.Log(tileDetails.seedItemID);
            }
            Destroy(gameObject);
            Debug.Log("destory");
        }

    }
}
