using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace MyFarm.Save
{
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        private List<Isavealbe> saveList = new List<Isavealbe>();
        public List<DataSlot> dataSlots = new List<DataSlot>(new DataSlot[3]);

        private string jsonFolder;
        private int curIndex;
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
                for (int i = 0; i < dataSlots.Count; i++)
                {
                    var resultPath = jsonFolder + "data" + i + ".json";
                    if (File.Exists(resultPath))
                    {
                        var stringData = File.ReadAllText(resultPath);
                        var jsonData = JsonConvert.DeserializeObject<DataSlot>(stringData);
                        dataSlots[i] = jsonData;
                    }
                }
            }
        }
        //存档
        private void Save(int index)
        {
            DataSlot data = new DataSlot();
            foreach (var save in saveList)
            {
                //遍历所有接口的实例，把guid当做key，返回的savedate当value，所有的合一起就是一个存档字典
                data.dataDic.Add(save.GUID, save.SaveGame());

            }

            dataSlots[index] = data;


            var resultPath = jsonFolder + "data" + index + ".json";

            // Debug.Log(dataSlots[index]);
            var jsonData = JsonConvert.SerializeObject(dataSlots[index], Formatting.Indented);//序列化
            if (!File.Exists(resultPath))
            {
                //创建路径
                Directory.CreateDirectory(jsonFolder);
            }
            //写入存档数据    
            File.WriteAllText(resultPath, jsonData);
        }
        //读档
        public void Load(int index)
        {
            curIndex = index;
            var resultPath = jsonFolder + "data" + index + ".json";
            var sringData = File.ReadAllText(resultPath);//根据路径读出来
            var jsonData = JsonConvert.DeserializeObject<DataSlot>(sringData);//读出来的东西反序列化成dataslot
            foreach (var save in saveList)
            {
                save.LoadGame(jsonData.dataDic[save.GUID]);
            }

        }
    }
}
