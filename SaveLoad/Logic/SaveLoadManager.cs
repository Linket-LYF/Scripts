using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace MyFarm.Save
{
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        private List<Isavealbe> saveList = new List<Isavealbe>();
        public Dictionary<string, SaveData> dataDic = new Dictionary<string, SaveData>();
        private string jsonFolder;
        private int curIndex;
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            jsonFolder = Application.persistentDataPath + "/SAVE DATA/";//创建存储路径 
            curIndex=0;
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Save(curIndex);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                Load(curIndex);
            }
        }
        private void OnEndGameEvent()
        {
            Save(curIndex);
        }

        private void OnStartNewGame(int obj)
        {
            curIndex = obj;
            Debug.Log("curIndex" + curIndex);
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
                var resultPath = jsonFolder + "data" + curIndex + ".json";
                if (File.Exists(resultPath))
                {
                    var stringData = File.ReadAllText(resultPath);
                    var jsonData = JsonConvert.DeserializeObject<Dictionary<string, SaveData>>(stringData);
                    saveData = jsonData;
                }
            }
        }
        //存档
        private void Save()
        {
            DataSlot data = new DataSlot();
            foreach (var save in saveList)
            {
                //遍历所有接口的实例，把guid当做key，返回的savedate当value，所有的合一起就是一个存档字典
                dataDic.Add(save.GUID, save.SaveGame());

            }
            var resultPath = jsonFolder + "data" + curIndex + ".json";

            // Debug.Log(dataSlots[index]);
            var jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);//序列化
            if (!File.Exists(resultPath))
            {
                //创建路径
                Directory.CreateDirectory(jsonFolder);
            }
            //写入存档数据    
            File.WriteAllText(resultPath, jsonData);
        }
        //读档
        public void Load()
        {
            var resultPath = jsonFolder + "data" + curIndex + ".json";
            var stringData = File.ReadAllText(resultPath);//根据路径读出来
            var jsonData = JsonConvert.DeserializeObject<Dictionary<string, SaveData>>(stringData);//读出来的东西反序列化成saveData
            foreach (var save in saveList)
            {
                save.LoadGame(dataDic[save.GUID]);
            }

        }
    }
}
