using System;
using Level;
using Map.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Map.Logic
{
    [ExecuteInEditMode]
    public class GridMap : MonoBehaviour
    {
        public MapData_SO mapData;
        public MapTileType gridType;
        private Tilemap curTilemap;

        private void OnEnable()
        {
            if (!Application.IsPlaying(this))
            {
                curTilemap = GetComponent<Tilemap>();

                if (mapData != null)
                {
                    mapData.tileProperties.Clear();
                }
            }
        }

        private void OnDisable()
        {
            if (Application.IsPlaying(this)) return;
            
            curTilemap = GetComponent<Tilemap>();
            UpdateTileMap();
            
#if UNITY_EDITOR
            if (mapData != null)
            {
                EditorUtility.SetDirty(mapData);
            }            
#endif
        }
        
        private void UpdateTileMap()
        {
            curTilemap.CompressBounds();

            if (mapData == null) return;
            
            var startPos = curTilemap.cellBounds.min;
            var endPos = curTilemap.cellBounds.max;

            for (var x = startPos.x; x < endPos.x; x++) 
            {
                for (var y = startPos.y; y < endPos.y; y++)
                {
                    var tile = curTilemap.GetTile(new Vector3Int(x, y, 0));
                    if (tile == null) continue;
                    
                    var newTile = new TileProperty(new Vector2Int(x, y), gridType);
                    mapData.tileProperties.Add(newTile);
                }
            }

        }
    }
}