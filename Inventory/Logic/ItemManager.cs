using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyFarm.Save;
namespace MyFarm.Inventory
{
    public class ItemManager : MonoBehaviour, Isavealbe
    {
        public Item itemPrefab;
        public Item bounce;
        private Transform itemParent;
        private Transform playerPos => FindObjectOfType<Player>().transform;

        public string GUID => GetComponent<DataGUID>().guid;

        //记录场景item
        private Dictionary<string, List<SceneItem>> sceneItemDic = new Dictionary<string, List<SceneItem>>();
        //记录场景建造信息
        private Dictionary<string, List<SceneFurniture>> sceneFurnitureDic = new Dictionary<string, List<SceneFurniture>>();

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;
            EventHandler.AfterLoadSceneEvent += OnAfterLoadSceneEvent;
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
            EventHandler.DropItemEvent += OnDropItemEvent;
            EventHandler.buildItem += OnbuildItem;
            EventHandler.StartNewGame += OnStartNewGame;
        }

        private void OnStartNewGame()
        {
            sceneItemDic.Clear();
            sceneFurnitureDic.Clear();
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            Isavealbe save = this;
            save.RegisterSaveble();
        }


        private void OnBeforeSceneUnloadEvent()
        {
            GetAllSceneItems();
            GetSceneFurniture();
        }

        private void OnAfterLoadSceneEvent()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;
            RecreateAllItems();
            RebulidFurniture();
        }

        private void OnInstantiateItemInScene(int id, Vector3 pos)
        {
            var item = Instantiate(bounce, pos, Quaternion.identity, itemParent);
            item.itemID = id;
            item.GetComponent<ItemBounce>().InitBounceItemm(pos, Vector3.up);
        }
        private void OnDropItemEvent(int itemid, Vector3 mousepos, ItemType itemType)
        {
            if (itemType == ItemType.Seed) return;
            var item = Instantiate(bounce, playerPos.transform.position, Quaternion.identity, itemParent);
            item.itemID = itemid;
            var dir = (mousepos - playerPos.position).normalized;
            item.GetComponent<ItemBounce>().InitBounceItemm(mousepos, dir);
        }
        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
            EventHandler.AfterLoadSceneEvent -= OnAfterLoadSceneEvent;
            EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
            EventHandler.DropItemEvent -= OnDropItemEvent;
            EventHandler.buildItem -= OnbuildItem;
            EventHandler.StartNewGame -= OnStartNewGame;
        }
        //建造
        private void OnbuildItem(int id, Vector3 pos)
        {
            var buildItem = InventoryManager.Instance.bluePrint.FindBluePrint(id);
            var item = Instantiate(buildItem.bulidPrefab, pos, Quaternion.identity, itemParent);
            if (item.GetComponent<EmptyBox>())
            {
                item.GetComponent<EmptyBox>().boxID = InventoryManager.Instance.dicCnt;
                item.GetComponent<EmptyBox>().InitBox(item.GetComponent<EmptyBox>().boxID);
            }
        }

        //获得场景物品信息
        private void GetAllSceneItems()
        {
            List<SceneItem> currentSceneItem = new List<SceneItem>();
            foreach (var item in FindObjectsOfType<Item>())
            {
                SceneItem sceneItem = new SceneItem
                {
                    itemID = item.itemID,
                    position = new SerializableVector3(item.transform.position),


                };
                currentSceneItem.Add(sceneItem);
            }
            if (sceneItemDic.ContainsKey(SceneManager.GetActiveScene().name))
            {
                //找到数据就更新item数据列表
                sceneItemDic[SceneManager.GetActiveScene().name] = currentSceneItem;
            }
            else
            {
                //如果新场景就添加进去
                sceneItemDic.Add(SceneManager.GetActiveScene().name, currentSceneItem);
            }
        }
        //生成场景物品信息
        private void RecreateAllItems()
        {
            List<SceneItem> currentItems = new List<SceneItem>();
            if (sceneItemDic.TryGetValue(SceneManager.GetActiveScene().name, out currentItems))
            {
                if (currentItems != null)
                {
                    foreach (var item in FindObjectsOfType<Item>())
                    {
                        Destroy(item.gameObject);
                    }
                    foreach (var item in currentItems)
                    {
                        Item newItem = Instantiate(itemPrefab, item.position.ToVertor3(), Quaternion.identity, itemParent);
                        newItem.InitItem(item.itemID);
                    }
                }
            }
        }
        //获取场景建造信息
        private void GetSceneFurniture()
        {
            List<SceneFurniture> sceneFurnitures = new List<SceneFurniture>();
            foreach (var furniture in FindObjectsOfType<Furniture>())
            {
                SceneFurniture sceneFurniture = new SceneFurniture();
                {
                    sceneFurniture.itemID = furniture.furnitureID;
                    sceneFurniture.position = new SerializableVector3(furniture.transform.position);
                    sceneFurniture.clientID = furniture.clientID;
                };
                GameManager.Instance.sceneFurnitures.Add(sceneFurniture);
                if (furniture.GetComponent<EmptyBox>())
                    sceneFurniture.boxID = furniture.GetComponent<EmptyBox>().boxID;
                sceneFurnitures.Add(sceneFurniture);
            }
            if (sceneFurnitureDic.ContainsKey(SceneManager.GetActiveScene().name))
            {
                sceneFurnitureDic[SceneManager.GetActiveScene().name] = sceneFurnitures;
            }
            else
            {
                sceneFurnitureDic.Add(SceneManager.GetActiveScene().name, sceneFurnitures);
            }
        }
        //重新生成场景建造物品
        private void RebulidFurniture()
        {
            List<SceneFurniture> sceneFurnitures = new List<SceneFurniture>();
            sceneFurnitureDic.TryGetValue(SceneManager.GetActiveScene().name, out sceneFurnitures);
            if (sceneFurnitures != null)
            {
                foreach (SceneFurniture sceneFurniture in sceneFurnitures)
                {
                    var buildItem = InventoryManager.Instance.bluePrint.FindBluePrint(sceneFurniture.itemID);
                    var item = Instantiate(buildItem.bulidPrefab, sceneFurniture.position.ToVertor3(), Quaternion.identity, itemParent);
                    if (item.GetComponent<EmptyBox>())
                    {
                        item.GetComponent<EmptyBox>().InitBox(sceneFurniture.boxID);
                    }
                }
            }
        }

        public SaveGameC2SMsg SaveGame()
        {
            GetAllSceneItems();
            GetSceneFurniture();
            SaveGameC2SMsg saveData = new();
            foreach (var item in sceneItemDic)
            {
                ListSceneItemMsg listSceneItemMsg = new ListSceneItemMsg();
                foreach (var sceneItem in item.Value)
                {
                    listSceneItemMsg.SceneItems.Add(SceneItem.SceneItem2SceneItemMsg(sceneItem));

                }
                saveData.SceneItems.Add(item.Key, listSceneItemMsg);
            }
            foreach (var item in sceneFurnitureDic)
            {
                ListSceneFurnitureMsg listSceneFurnitureMsg = new ListSceneFurnitureMsg();
                foreach (var sceneFurniture in item.Value)
                {
                    listSceneFurnitureMsg.SceneFurnitures.Add(SceneFurniture.SceneFurniture2SceneFurnitureMsg(sceneFurniture));
                }
                saveData.SceneFurnitures.Add(item.Key, listSceneFurnitureMsg);
            }
            return saveData;
        }

        public void LoadGame(SaveGameC2SMsg saveData)
        {
            foreach (var item in saveData.SceneItems)
            {
                List<SceneItem> sceneItems = new();
                foreach (var sceneItem in item.Value.SceneItems)
                {
                    sceneItems.Add(SceneItem.SceneItemMsg2SceneItem(sceneItem));
                }
                sceneItemDic[item.Key] = sceneItems;
            }
            foreach (var item in saveData.SceneFurnitures)
            {
                List<SceneFurniture> sceneFurnitures = new();
                foreach (var sceneFurniture in item.Value.SceneFurnitures)
                {
                    sceneFurnitures.Add(SceneFurniture.SceneFurnitureMsg2SceneFurniture(sceneFurniture));
                }
                sceneFurnitureDic[item.Key] = sceneFurnitures;
            }
            RecreateAllItems();
            RebulidFurniture();
        }
    }
}

