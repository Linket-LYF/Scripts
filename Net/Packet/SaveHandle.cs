using System;
using MyFarm.Save;
public class SaveHandle
{
    //单例模式
    private static SaveHandle instance = new();
    public static SaveHandle Instance => instance;

    public void LoadRequest(byte[] data)
    {
        var loadGameS2CMsg = ProtoHelper.ToMessage<LoadGameS2CMsg>(data);
        if (loadGameS2CMsg.Result == 2)
        {
            EventHandler.CallStartNewGame();
        }
        else if(loadGameS2CMsg.Result == 1)
        {
            SaveLoadManager.Instance.LoadGame(loadGameS2CMsg.GamesaveGame);
        }
    }
    internal void SaveRequest(byte[] obj)
    {
        throw new NotImplementedException();
    }
}