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
        public Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();//地图块的坐标和对应的Tile对象
        
        private void Start()
        {
            TilemapPos2Position();
        }

        private void TilemapPos2Position()
        {
            throw new NotImplementedException();
        }
        //根据坐标获取Tile对象
        public Tile GetTile(Vector2Int gridPosition)
        {
            if (tiles.ContainsKey(gridPosition))
            {
                return tiles[gridPosition];
            }
            return null;
        }
    }
    //字典来存储Tile对象

    public class Tile
    {
        public Vector2Int gridPosition;
        public MapTileType type;
        public Tile(Vector2Int gridPosition, MapTileType type)
        {
            this.gridPosition = gridPosition;
            this.type = type;
        }
    }
    public enum MapTileType
    {
        empty,
        Wall,
        Road,
        Door
    }
}