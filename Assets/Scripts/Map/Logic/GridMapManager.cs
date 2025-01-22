using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Level;
using Map.Data;
using Plutono.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TileDetails
{
    public int girdX, girdY;

    public bool isEmpty;
    public bool isStart;
    public bool isWall;
    public bool isRoad;
    public bool isEnemyObstacle;
    public bool isEnemy;
    public bool isDoor;
}

public class GridMapManager : Singleton<GridMapManager>
{
    [Header("地图信息")] public List<MapData_SO> mapDataList;

    //场景名字、坐标与对应的瓦片信息
    private readonly Dictionary<string, TileDetails> tileDetailsDict = new();

    protected override void Awake()
    {
        base.Awake();
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
                case MapTileType.Enemy:
                    tileDetails.isEnemy = true;
                    break;
                case MapTileType.Start:
                    tileDetails.isStart = true;
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

    /// <summary>
    /// 根据坐标返回瓦片信息
    /// </summary>
    /// <param name="pos">当前瓦片的坐标</param>
    /// <returns></returns>
    public TileDetails GetTileDetails(Vector2Int pos)
    {
        var key = $"{pos.x}x{pos.y}y{SceneManager.GetActiveScene().name}";
        return tileDetailsDict[key];
    }

    public List<TileDetails> GetTileDetailsList(MapTileType type)
    {
        var res = new List<TileDetails>();
        foreach (var tileDetails in tileDetailsDict)
        {
            switch (type)
            {
                case MapTileType.Empty:
                    if (tileDetails.Value.isEmpty)
                        res.Add(tileDetails.Value);
                    break;
                case MapTileType.Start:
                    if (tileDetails.Value.isStart)
                        res.Add(tileDetails.Value);
                    break;
                case MapTileType.Wall:
                    if (tileDetails.Value.isWall)
                        res.Add(tileDetails.Value);
                    break;
                case MapTileType.Road:
                    if (tileDetails.Value.isRoad)
                        res.Add(tileDetails.Value);
                    break;
                case MapTileType.Door:
                    if (tileDetails.Value.isDoor)
                        res.Add(tileDetails.Value);
                    break;
                case MapTileType.EnemyObstacle:
                    if (tileDetails.Value.isEnemyObstacle)
                        res.Add(tileDetails.Value);
                    break;
                case MapTileType.Enemy:
                    if (tileDetails.Value.isEnemy)
                        res.Add(tileDetails.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        return res;
    }
}