using MyFarm.Save;
public class LoadHandle
{
    //单例模式
    private static LoadHandle instance = new();
    public static LoadHandle Instance => instance;
    public void Load(byte[] data)
    {
        var dataMsg = ProtoHelper.ToMessage<GameSaveDataMsg>(data);
        GameManager.Instance.player.playerId = dataMsg.Account;
        SaveLoadManager.Instance.LoadGame(dataMsg.GamesaveGame);
    }
    public void Generate(byte[] data)
    {

    }
    public void Save(byte[] data)
    {

    }
}