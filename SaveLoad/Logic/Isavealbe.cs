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
        SaveGameC2SMsg SaveGame();
        void LoadGame(SaveGameC2SMsg saveData);
    }
}
