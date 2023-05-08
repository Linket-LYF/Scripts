using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyFarm.Save
{
    public class DataSlot
    {
        //key is GUID
        public Dictionary<string, SaveData> dataDic = new Dictionary<string, SaveData>();
        public string DataTime
        {
            get
            {
                var key = TimeManager.Instance.GUID;
                if (dataDic.ContainsKey(key))
                {
                    var timeData = dataDic[key];
                    return timeData.timeDic["gameYear"] + "年/" + timeData.timeDic["gameMonth"] + "月/" + timeData.timeDic["gameDay"] + "日/";
                }
                else
                    return string.Empty;
            }
        }
    }
}
