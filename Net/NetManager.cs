using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class NetManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        MsgMap.Instance.Init();
    }
    void Start()
    {
        Client.Instance.Start();
    }

}
