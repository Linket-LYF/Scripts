using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using MyFarm.Transition;
namespace MyFarm.Save
{
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        private List<Isavealbe> saveList = new();
        //key为GUID，value为SaveData
        public Dictionary<string, SaveData> dataDic = new();

        private string jsonFolder;
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            jsonFolder = Application.persistentDataPath + "/SAVE DATA/";//创建存储路径 
            ReadSaveData();
        }
        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            EventHandler.StartNewGame += OnStartNewGame;
            EventHandler.EndGameEvent += OnEndGameEvent;
        }
        private void OnEndGameEvent()
        {
            Save();
        }

        private void OnStartNewGame()
        {
            //Debug.Log("curindex" + curIndex);
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            EventHandler.StartNewGame -= OnStartNewGame;
            EventHandler.EndGameEvent -= OnEndGameEvent;
        }
        //注册接口实例添加到list
        public void RegisterSavable(Isavealbe savelbe)
        {
            if (!saveList.Contains(savelbe))
            {
                saveList.Add(savelbe);

            }

        }

        private void ReadSaveData()
        {
            if (Directory.Exists(jsonFolder))
            {
                var resultPath = jsonFolder + "data" + GameManager.Instance.Player.playerId + ".json";
                if (File.Exists(resultPath))
                {
                    var stringData = File.ReadAllText(resultPath);
                    var jsonData = JsonConvert.DeserializeObject<Dictionary<string, SaveData>>(stringData);
                    dataDic = jsonData;
                }
            }
        }
        //存档
        public void Save()
        {
            foreach (var save in saveList)
            {
                //遍历所有接口的实例，把guid当做key，返回的savedate当value，所有的合一起就是一个存档字典
                dataDic.Add(save.GUID, save.SaveGame());

            }
            var resultPath = jsonFolder + "data" + GameManager.Instance.Player.playerId + ".json";

            // Debug.Log(dataSlots[index]);
            var jsonData = JsonConvert.SerializeObject(dataDic, Formatting.Indented);//序列化
            if (!File.Exists(resultPath))
            {
                //创建路径
                Directory.CreateDirectory(jsonFolder);
            }
            //写入存档数据    
            File.WriteAllText(resultPath, jsonData);
            //将存档发送给服务器
            GameSaveDataMsg gameSaveDataMsg = new GameSaveDataMsg();
            foreach (var item in dataDic)
            {
                gameSaveDataMsg.GamesaveGame.Add(item.Key, SaveData2SaveMsg(item.Value));
            }

            var stram=ProtoHelper.ToBytes(gameSaveDataMsg);
            Client.Instance.SendToClient(MsgTypes.LoadRequest, stram);
        }
        //读档
        public void Load()
        {
            var resultPath = jsonFolder + "data" + GameManager.Instance.Player.playerId + ".json";
            var stringData = File.ReadAllText(resultPath);//根据路径读出来
            var jsonData = JsonConvert.DeserializeObject<Dictionary<string, SaveData>>(stringData);//读出来的东西反序列化成saveData
            foreach (var save in saveList)
            {
                save.LoadGame(dataDic[save.GUID]);
            }

            //向服务器发送读档请求
            //NetManager.Instance.SendMsg(OpCode.Account, AccountCode.LoadRequest, index);
        }

        public SaveGameC2SMsg SaveData2SaveMsg(SaveData saveData)
        {
            var saveDataMsg = new SaveGameC2SMsg();
            saveDataMsg.DataSceneName = saveData.dataSceneName;
            foreach (var item in saveData.characterPos)
            {
                saveDataMsg.CharacterPos.Add(item.Key, new Vector3Msg { X = item.Value.x, Y = item.Value.y, Z = item.Value.z });
            }
            foreach (var item in saveData.sceneItemDic)
            {
                ListSceneItemMsg listSceneItemMsg = new ListSceneItemMsg();
                foreach (var sceneItem in item.Value)
                {
                    listSceneItemMsg.SceneItems.Add(SceneItem.SceneItem2SceneItemMsg(sceneItem));

                }
                
                saveDataMsg.SceneItems.Add(item.Key, listSceneItemMsg);
            }
            foreach (var item in saveData.sceneFurnitureDic)
            {
                ListSceneFurnitureMsg listSceneFurnitureMsg = new ListSceneFurnitureMsg();
                foreach (var sceneFurniture in item.Value)
                {
                    listSceneFurnitureMsg.SceneFurnitures.Add(SceneFurniture.SceneFurniture2SceneFurnitureMsg(sceneFurniture));
                }
                saveDataMsg.SceneFurnitures.Add(item.Key, listSceneFurnitureMsg);
            }
            foreach (var item in saveData.tileDetailsDic)
            {
                saveDataMsg.TileDetails.Add(item.Key,TileDetails.TileDetails2TileDetailsMsg(item.Value));
            }
            foreach (var item in saveData.fristLoadDic)
            {
                saveDataMsg.FristLogin.Add(item.Key, item.Value);
            }
            foreach (var item in saveData.inventoryDic)
            {
                ListInventoryItemMsg listInventoryItemMsg = new ListInventoryItemMsg();
                foreach (var inventoryItem in item.Value)
                {
                    listInventoryItemMsg.InventoryItems.Add(InventoryItem.InventoryItem2InventoryItemMsg(inventoryItem));
                }
                saveDataMsg.InventoryItems.Add(item.Key, listInventoryItemMsg);
            }
            foreach (var item in saveData.timeDic)
            {
                saveDataMsg.Time.Add(item.Key, item.Value);
            }
            saveDataMsg.Money = saveData.playerMoney;
            saveDataMsg.AnimState = saveData.ani;

            return saveDataMsg;
        }
        public SaveData SaveMsg2SaveData(SaveGameC2SMsg saveGameC2SMsg)
        {
            SaveData saveData = new SaveData();
            saveData.dataSceneName = saveGameC2SMsg.DataSceneName;
            foreach (var item in saveGameC2SMsg.CharacterPos)
            {
                saveData.characterPos.Add(item.Key, new Vector3(item.Value.X, item.Value.Y, item.Value.Z));
            }
            foreach (var item in saveGameC2SMsg.SceneItems)
            {
                List<SceneItem> sceneItems = new List<SceneItem>();
                foreach (var sceneItem in item.Value.SceneItems)
                {
                    sceneItems.Add(SceneItem.SceneItemMsg2SceneItem(sceneItem));
                }
                saveData.sceneItemDic.Add(item.Key, sceneItems);
            }
            foreach (var item in saveGameC2SMsg.SceneFurnitures)
            {
                List<SceneFurniture> sceneFurnitures = new List<SceneFurniture>();
                foreach (var sceneFurniture in item.Value.SceneFurnitures)
                {
                    sceneFurnitures.Add(SceneFurniture.SceneFurnitureMsg2SceneFurniture(sceneFurniture));
                }
                saveData.sceneFurnitureDic.Add(item.Key, sceneFurnitures);
            }
            foreach (var item in saveGameC2SMsg.TileDetails)
            {
                saveData.tileDetailsDic.Add(item.Key, TileDetails.TileDetailsMsg2TileDetails(item.Value));
            }
            foreach (var item in saveGameC2SMsg.FristLogin)
            {
                saveData.fristLoadDic.Add(item.Key, item.Value);
            }
            foreach (var item in saveGameC2SMsg.InventoryItems)
            {
                List<InventoryItem> inventoryItems = new List<InventoryItem>();
                foreach (var inventoryItem in item.Value.InventoryItems)
                {
                    inventoryItems.Add(InventoryItem.InventoryItemMsg2InventoryItem(inventoryItem));
                }
                saveData.inventoryDic.Add(item.Key, inventoryItems);
            }
            foreach (var item in saveGameC2SMsg.Time)
            {
                saveData.timeDic.Add(item.Key, item.Value);
            }
            saveData.playerMoney = saveGameC2SMsg.Money;
            saveData.ani = saveGameC2SMsg.AnimState;
            return saveData;
        }
        public void LoadGame(GameSaveDataMsg gameSaveDataMsg)
        {
            //将gameSaveDataMsg转换并赋值给dataDic
            foreach (var item in gameSaveDataMsg.GamesaveGame)
            {
                dataDic.Add(item.Key, SaveMsg2SaveData(item.Value));
            }
            foreach (var save in saveList)
            {
                save.LoadGame(dataDic[save.GUID]);
            }
        }
    }
}
