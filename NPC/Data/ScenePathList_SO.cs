using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ScenePathList_SO", menuName = "Map/scenePath")]
public class ScenePathList_SO : ScriptableObject
{
    public List<SceneRoute> sceneRoutes;
}
