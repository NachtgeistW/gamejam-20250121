using Plutono.Util;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Level.GameEvent;

namespace Assets.Scripts.Level
{
    public class Wall : MonoBehaviour
    {
        public Tilemap wallTilemap;           // 墙的Tilemap
        public TilemapRenderer wallRenderer;  // 墙的渲染器

        private void OnEnable()
        {
            EventCenter.AddListener<WaveHitWallEvent>(OnWaveHitWall);
        }

        private void OnDisable()
        {
            EventCenter.RemoveListener<WaveHitWallEvent>(OnWaveHitWall);
        }

        private void OnWaveHitWall(WaveHitWallEvent e)
        {
        }

        private void Start()
        {

            for (var x = wallTilemap.cellBounds.xMin; x < wallTilemap.cellBounds.xMax; x++)
            {
                for (var y = wallTilemap.cellBounds.yMin; y < wallTilemap.cellBounds.yMax; y++)
                {
                    wallTilemap.SetColor(new Vector3Int(x, y, 0), new Color(1, 1, 1, 0));
                }
            }
        }
    }
}