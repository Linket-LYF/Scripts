using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyFarm.Save;
public class SaveButton : MonoBehaviour
{
    public Text text;
    private Button curBnt;
    private int index => transform.GetSiblingIndex();
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    // private void OnEnable()
    // {
    //     SetSlotUI();
    // }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        curBnt = GetComponent<Button>();
        curBnt.onClick.AddListener(LoadGame);
    }
    // private void SetSlotUI()
    // {
    //     curData = SaveLoadManager.Instance.dataDic;
    //     if (curData != null)
    //     {
    //         text.text = curData.DataTime;
    //     }
    // }
    private void LoadGame()
    {
        // if (curData != null)
        // {
        //     SaveLoadManager.Instance.Load();
        // }
        // else
        // {
        //     EventHandler.CallStartNewGame();
        //     //SaveLoadManager.Instance.Load();
        // }
    }
}
