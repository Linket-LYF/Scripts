//单例,返回一个唯一的ClientID用于标识客户端
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class NetClientID
{
    private static NetClientID instance;
    private static readonly object padlock = new object();
    private int clientID = 0;
    private NetClientID()
    {
        EventHandler.StartNewGame += ResetClientID;
    }
    public static NetClientID Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new NetClientID();
                }
                return instance;
            }
        }
    }
    public int GetClientID()
    {
        return clientID++;
    }
    public void ResetClientID()
    {
        clientID = 0;
    }
}