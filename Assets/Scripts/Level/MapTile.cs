using System;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Level
{
    public class MapTile : MonoBehaviour
    {
        [field: SerializeField] public Tilemap Tilemap { get; private set; }
        [field: SerializeField] public MapTileType Type { get; private set; }
        [field: SerializeField] public float Brightness { get; private set; }
        [field: SerializeField] public Vector2 Position { get; private set; }
        public Dictionary<Vector2Int, TileProperty> tiles = new Dictionary<Vector2Int, TileProperty>();//地图块的坐标和对应的Tile对象
        public float gridSize = 1f;
        

        public Vector3 TilemapPos2Position(Vector2Int gridPosition)//将Tilemap坐标转换为世界坐标
        {
            Vector3 position = new Vector3(gridPosition.x * gridSize, 0, gridPosition.y * gridSize);//将Tilemap坐标转换为世界坐标用了X轴和Z轴
            return position;
        }
        public Vector2Int Position2TilemapPos(Vector3 position)//将世界坐标转换为Tilemap坐标
        {
            int gridX = Mathf.FloorToInt(position.x / gridSize);
            int gridY = Mathf.FloorToInt(position.y / gridSize);
            return new Vector2Int(gridX, gridY);
        }
        //根据坐标获取Tile对象
        public TileProperty GetTile(Vector2Int gridPosition)
        {
            if (tiles.ContainsKey(gridPosition))
            {
                return tiles[gridPosition];
            }
            return null;
        }
    }
    //字典来存储Tile对象

    [Serializable]
    public class TileProperty
    {
        public Vector2Int gridPosition;
        public MapTileType type;
        
        public TileProperty(Vector2Int gridPosition, MapTileType type)
        {
            this.gridPosition = gridPosition;
            this.type = type;
        }
    }
}