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
        public float gridSize = 1f;
        
        private void Start()
        {
            //TilemapPos2Position();
        }

        public Vector3 TilemapPos2Position(Vector2Int gridPosition)//��Tilemap����ת��Ϊ��������
        {
            Vector3 position = new Vector3(gridPosition.x * gridSize, 0, gridPosition.y * gridSize);//��Tilemap����ת��Ϊ������������X���Z��
            return position;
        }
        public Vector2Int Position2TilemapPos(Vector3 position)//����������ת��ΪTilemap����
        {
            int gridX = Mathf.FloorToInt(position.x / gridSize);
            int gridY = Mathf.FloorToInt(position.y / gridSize);
            return new Vector2Int(gridX, gridY);
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