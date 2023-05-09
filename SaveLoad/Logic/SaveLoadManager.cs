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

            ProtoHelper.ToBytes(gameSaveDataMsg);
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

            return null;
        }
    }
}
