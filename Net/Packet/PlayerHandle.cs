public class PlayerHandle
{
    //单例模式
    private static PlayerHandle instance = new();
    public static PlayerHandle Instance => instance;
    public void Move(byte[] data)
    {
    }
    public void SwitchAnim(byte[] data)
    {
    }
}