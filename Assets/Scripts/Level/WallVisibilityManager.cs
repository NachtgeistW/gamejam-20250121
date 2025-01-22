using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Level
{
    public class WallVisibilityManager : MonoBehaviour
    {
        private Tilemap wallTilemap;

        private void Awake()
        {
            // 获取墙的Tilemap
            wallTilemap = GameObject.FindGameObjectWithTag("Map").GetComponent<Tilemap>();

            // 获取地图边界
            wallTilemap.CompressBounds();
            var bounds = wallTilemap.cellBounds;

            // 遍历所有瓦片
            for (var x = bounds.min.x; x < bounds.max.x; x++)
            {
                for (var y = bounds.min.y; y < bounds.max.y; y++)
                {
                    var tilePosition = new Vector3Int(x, y, 0);

                    if (!wallTilemap.HasTile(tilePosition)) continue;
                    // 移除瓦片的颜色锁定
                    wallTilemap.SetTileFlags(tilePosition, TileFlags.None);
                    // 设置瓦片为完全透明
                    wallTilemap.SetColor(tilePosition, new Color(1, 1, 1, 0));
                }
            }
        }
    }
}