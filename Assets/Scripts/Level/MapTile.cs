using System;
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

        private void Start()
        {
            TilemapPos2Position();
        }

        private void TilemapPos2Position()
        {
            throw new NotImplementedException();
        }
    }

    public enum MapTileType
    {
        Wall,
        Road,
        Door
    }
}