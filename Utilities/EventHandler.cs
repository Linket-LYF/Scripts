using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyFarm.Dialog;
using UnityEngine.Events;
public static class EventHandler
{
    //更新背包ui事件
    public static event Action<InventoryLocation, List<InventoryItem>> UpdateInventoryUI;
    public static void CallUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
    {
        UpdateInventoryUI?.Invoke(location, list);
    }
    //生成世界物品事件
    public static event Action<int, Vector3> InstantiateItemInScene;
    public static void CallInstantiateItemInScene(int ID, Vector3 pos)
    {
        InstantiateItemInScene?.Invoke(ID, pos);
    }
    //人物扔出物品
    public static event Action<int, Vector3, ItemType> DropItemEvent;
    public static void CallDropItemEvent(int itemID, Vector3 pos, ItemType itemType)
    {
        DropItemEvent?.Invoke(itemID, pos, itemType);
    }
    //通知物品点按事件
    public static event Action<ItemDetails, bool> ItemSelectedEvent;
    public static void CallItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        ItemSelectedEvent?.Invoke(itemDetails, isSelected);
    }
    //分钟事件
    public static event Action<int, int, int> GameMinuteEvent;
    public static void CallGameMinuteEvent(int minute, int hour, int day)
    {
        GameMinuteEvent?.Invoke(minute, hour, day);
    }
    //小时事件日月年季节
    public static event Action<int, int, int, int, Season> GameHourEvent;
    public static void CallGameHourEvent(int hour, int day, int month, int year, Season season)
    {
        GameHourEvent?.Invoke(hour, day, month, year, season);
    }
    //每天事件
    public static event Action<int, Season> GameDayEvent;
    public static void CallGameDayEvent(int day, Season season)
    {
        GameDayEvent?.Invoke(day, season);
    }
    //切换场景事件
    public static event Action<string, Vector3> TransitionEvent;
    public static void CallTransitionEvent(string sceneName, Vector3 targetPositon)
    {

        TransitionEvent?.Invoke(sceneName, targetPositon);
    }
    //场景卸载之前的事件
    public static event Action BeforeSceneUnloadEvent;
    public static void CallBeforeSceneUnloadEven()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }
    //场景加载之后的事件
    public static event Action AfterLoadSceneEvent;
    public static void CallAfterLoadSceneEvent()
    {
        AfterLoadSceneEvent?.Invoke();
    }
    //角色坐标
    public static event Action<Vector3> MoveToPositon;
    public static void CallMoveToPositon(Vector3 pos)
    {
        MoveToPositon?.Invoke(pos);
    }
    //鼠标点击事件
    public static event Action<Vector3, ItemDetails> MouseClickEvent;
    public static void CallMouseClickEvent(Vector3 mousePos, ItemDetails itemDetails)
    {
        MouseClickEvent?.Invoke(mousePos, itemDetails);
    }
    //人物动画结束事件
    public static event Action<Vector3, ItemDetails> ExecuteActionAfteiAnimation;
    public static void CallExecuteActionAfteiAnimation(Vector3 pos, ItemDetails itemDetails)
    {
        ExecuteActionAfteiAnimation?.Invoke(pos, itemDetails);
    }
    //更新当前场景农作物
    public static event Action<int, TileDetails> PlantSeedEvent;
    public static void CallPlantSeedEvent(int cropID, TileDetails tileDetails)
    {
        PlantSeedEvent?.Invoke(cropID, tileDetails);
    }
    //  收割作物事件
    public static event Action<int> HarvestAtPlayerPosition;
    public static void CallHarvestAtPlayerPosition(int itemID)
    {
        HarvestAtPlayerPosition?.Invoke(itemID);
    }
    //  种子重复收割事件
    public static event Action RefreshMap;
    public static void CallRefreshMap()
    {
        RefreshMap?.Invoke();
    }
    //更新场景事先摆放好的东西
    public static event Action GenerterCropEvent;
    public static void CallGenerterCropEvent()
    {
        GenerterCropEvent?.Invoke();
    }
    //传输数据显示对话UI
    public static event Action<DialogPiece> ShowDialogEvent;
    public static void CallShowDialogEvent(DialogPiece dialogPiece)
    {
        ShowDialogEvent?.Invoke(dialogPiece);
    }
    //商店开启关闭
    public static event Action<SlotType, InventoryBag_SO> BasicBagOpen;
    public static void CallBasicBagOpen(SlotType slotType, InventoryBag_SO inventoryBag_SO)
    {
        BasicBagOpen?.Invoke(slotType, inventoryBag_SO);
    }
    public static event Action<SlotType, InventoryBag_SO> CloseBasicBagOpen;
    public static void CallCloseBasicBagOpen(SlotType slotType, InventoryBag_SO inventoryBag_SO)
    {
        CloseBasicBagOpen?.Invoke(slotType, inventoryBag_SO);
    }
    //暂停游戏
    public static event Action<GameState> PauseGame;
    public static void CallPauseGame(GameState gameState)
    {
        PauseGame?.Invoke(gameState);
    }
    //买卖
    public static event Action<ItemDetails, bool> isSell;
    public static void CallisSell(ItemDetails itemDetails, bool issell)
    {
        isSell?.Invoke(itemDetails, issell);
    }
    //建造
    public static event Action<int, Vector3> buildItem;
    public static void CallbuildItem(int id, Vector3 pos)
    {
        buildItem?.Invoke(id, pos);
    }
    //切换灯光
    public static event Action<Season, LightShift, float> ChangeLightEvent;
    public static void CallChangeLightEven(Season season, LightShift shift, float time)
    {
        ChangeLightEvent?.Invoke(season, shift, time);
    }
    //播放音效
    public static event Action<SoundDetails> InitSoundEffect;
    public static void CallInitSoundEffect(SoundDetails soundDetails)
    {
        InitSoundEffect?.Invoke(soundDetails);
    }
    //获得detail
    public static event Action<soundName> GetSoundDeatil;
    public static void CallGetSoundDeatil(soundName soundName)
    {
        GetSoundDeatil?.Invoke(soundName);
    }
    //开始新游戏
    public static event Action<int> StartNewGame;
    public static void CallStartNewGame(int index)
    {
        StartNewGame?.Invoke(index);
    }
    //结束游戏
    public static event Action EndGameEvent;
    public static void CallEndGameEvent()
    {
        EndGameEvent?.Invoke();
    }
        //其他玩家加入
    public static event Action<int> OtherPlayerJoin;
    public static void CallOtherPlayerJoin(int name)
    {
        OtherPlayerJoin?.Invoke(name);
    }
    //其他玩家加入完毕
    public static event Action OtherPlayerJoinFinish;
    public static void CallOtherPlayerJoinFinish()
    {
        OtherPlayerJoinFinish?.Invoke();
    }
    //其他玩家离开
    public static event Action<string> OtherPlayerLeave;
    public static void CallOtherPlayerLeave(string name)
    {
        OtherPlayerLeave?.Invoke(name);
    }
}
