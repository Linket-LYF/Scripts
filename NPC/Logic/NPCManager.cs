using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : Singleton<NPCManager>
{
    public ScenePathList_SO scenePath;
    public List<NPCPosition> NPCPosition;
    public Dictionary<string, SceneRoute> sceneRouteDic = new Dictionary<string, SceneRoute>();

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        InitDic();
    }
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        EventHandler.StartNewGame += OnStartNewGame;
    }

    private void OnStartNewGame()
    {
        foreach (var chara in NPCPosition)
        {
            chara.npcPos.position = chara.oriPos;
            chara.npcPos.GetComponent<NPCMovement>().startScene = chara.startSCene;
        }
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        EventHandler.StartNewGame -= OnStartNewGame;
    }
    void InitDic()
    {
        if (scenePath.sceneRoutes != null)
        {
            if (scenePath.sceneRoutes.Count > 0)
            {
                foreach (SceneRoute route in scenePath.sceneRoutes)
                {
                    string key = route.fromScene + route.toScene;
                    if (sceneRouteDic.ContainsKey(key))
                        continue;
                    else
                        sceneRouteDic.Add(key, route);

                }
            }
        }

    }
    public SceneRoute FindRoute(string fromScene, string toScene)
    {
        return sceneRouteDic[fromScene + toScene];
    }
}
