using MyFarm.Save;
public class LoadHandle
{
    //单例模式
    private static LoadHandle instance = new();
    public static LoadHandle Instance => instance;
    public void Load(byte[] data)
    {
        var dataMsg=ProtoHelper.ToMessage<GameSaveDataMsg>(data);
        SaveLoadManager.Instance.LoadGame(dataMsg);
    }
    public void Generate(byte[] data)
    {

    }
    public void Save(byte[] data)
    {

    }
}