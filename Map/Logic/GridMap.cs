using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class GridMap : MonoBehaviour
{
    public MapData_SO mapData_SO;
    public GridType gridType;
    private Tilemap currentTilemap;
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        if (!Application.IsPlaying(this))
        {
            currentTilemap = GetComponent<Tilemap>();
            if (mapData_SO != null)
            {
                mapData_SO.tileProperties.Clear();
            }
        }
    }
    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        if (!Application.IsPlaying(this))
        {
            currentTilemap = GetComponent<Tilemap>();
            UpdateTileProperties();
#if UNITY_EDITOR
            if (mapData_SO != null)
            {
                EditorUtility.SetDirty(mapData_SO);
            }
#endif
        }
    }
    //更新瓦片信息
    private void UpdateTileProperties()
    {
        currentTilemap.CompressBounds();
        if (!Application.IsPlaying(this))
        {
            if (mapData_SO != null)
            {
                //绘制范围左下角坐标
                Vector3Int startPos = currentTilemap.cellBounds.min;
                //绘制范围右上角坐标
                Vector3Int endPos = currentTilemap.cellBounds.max;
                for (int x = startPos.x; x < endPos.x; x++)
                {
                    for (int y = startPos.y; y < endPos.y; y++)
                    {
                        //每个Grid
                        TileBase tile = currentTilemap.GetTile(new Vector3Int(x, y, 0));
                        if (tile != null)
                        {
                            TileProperty newTile = new TileProperty
                            {
                                tileCoordinate = new Vector2Int(x, y),
                                gridType = this.gridType,
                                boolTypeValue = true

                            };
                            mapData_SO.tileProperties.Add(newTile);
                        }
                    }
                }
            }
        }
    }
}
