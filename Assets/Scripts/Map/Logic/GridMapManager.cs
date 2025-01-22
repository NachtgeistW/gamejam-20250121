using System;
using System.Collections;
using System.Collections.Generic;
using Level;
using Map.Data;
using UnityEngine;

[System.Serializable]
public class TileDetails
{
    public int girdX, girdY;

    public bool isEmpty;
    public bool isWall;
    public bool isRoad;
    public bool isEnemyObstacle;
    public bool isDoor;
}

public class GridMapManager : MonoBehaviour
{
    [Header("地图信息")] public List<MapData_SO> mapDataList;

    //场景名字、坐标与对应的瓦片信息
    private readonly Dictionary<string, TileDetails> tileDetailsDict = new();

    private void Start()
    {
        foreach (var data in mapDataList)
        {
            InitTileDetailsDict(data);
        }
    }


    private void InitTileDetailsDict(MapData_SO mapData)
    {
        foreach (var tileProperty in mapData.tileProperties)
        {
            var tileDetails = new TileDetails
            {
                girdX = tileProperty.gridPosition.x,
                girdY = tileProperty.gridPosition.y,
            };

            var key = $"{tileDetails.girdX}x{tileDetails.girdY}y{mapData.mapName}";
            if (GetTileDetails(key) != null)
            {
                tileDetails = GetTileDetails(key);
            }

            switch (tileProperty.type)
            {
                case MapTileType.Empty:
                    tileDetails.isEmpty = true;
                    break;
                case MapTileType.Wall:
                    tileDetails.isWall = true;
                    break;
                case MapTileType.Road:
                    tileDetails.isRoad = true;
                    break;
                case MapTileType.Door:
                    tileDetails.isDoor = true;
                    break;
                case MapTileType.EnemyObstacle:
                    tileDetails.isEnemyObstacle = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (GetTileDetails(key) != null)
            {
                tileDetailsDict[key] = tileDetails;
            }
            else
            {
                tileDetailsDict.Add(key, tileDetails);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key">x+y+sceneName</param>
    /// <returns></returns>
    private TileDetails GetTileDetails(string key)
    {
        return tileDetailsDict.ContainsKey(key) ? tileDetailsDict[key] : null;
    }
}