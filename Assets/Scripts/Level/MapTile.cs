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
        public Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();//��ͼ�������Ͷ�Ӧ��Tile����
        
        private void Start()
        {
            TilemapPos2Position();
        }

        private void TilemapPos2Position()
        {
            throw new NotImplementedException();
        }
        //���������ȡTile����
        public Tile GetTile(Vector2Int gridPosition)
        {
            if (tiles.ContainsKey(gridPosition))
            {
                return tiles[gridPosition];
            }
            return null;
        }
    }
    //�ֵ����洢Tile����

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