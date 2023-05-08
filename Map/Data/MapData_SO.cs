using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MapData_SO", menuName = "Map/MapData")]
public class MapData_SO : ScriptableObject
{
    public string sceneName;
    public List<TileProperty> tileProperties;
    [Header("地图信息")]
    public int gridWidth;
    public int gridHeight;
    [Header("左下角原点")]
    public int originX;
    public int originY;

}
