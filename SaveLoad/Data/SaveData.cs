using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyFarm.Save
{
    [System.Serializable]
    public class SaveData
    {
        public string dataSceneName;//场景名称
        public Dictionary<string, SerializableVector3> characterPos;//角色位置
        public Dictionary<string, List<SceneItem>> sceneItemDic;//物品信息
        public Dictionary<string, List<SceneFurniture>> sceneFurnitureDic;//家具
        public Dictionary<string, TileDetails> tileDetailsDic;//瓦片地图
        public Dictionary<string, bool> fristLoadDic;//场景是否第一次生成   
        public Dictionary<string, List<InventoryItem>> inventoryDic;//背包
        public Dictionary<string, int> timeDic;//时间
        public int playerMoney;//金钱

        public string targetScene;
        public bool interactalbe;
        public int ani;

    }
}
