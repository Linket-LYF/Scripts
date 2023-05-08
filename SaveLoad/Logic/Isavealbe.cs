using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFarm.Save
{
    public interface Isavealbe
    {
        string GUID { get; }
        void RegisterSaveble()
        {
            SaveLoadManager.Instance.RegisterSavable(this);
        }
        SaveData SaveGame();
        void LoadGame(SaveData saveData);
    }
}
