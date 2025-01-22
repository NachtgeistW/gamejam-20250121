using Assets.Scripts.Level;
using Plutono.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Level.GameEvent;

namespace Level
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class RadioWave : MonoBehaviour
    {
        [field: SerializeField] public Vector2 Position { get; set; }
        //[field: SerializeField] public Sprite Sprite { get; private set; }

        public Animator animator { get; private set; }

        private Grid curGrid;

        [SerializeField] private float angle = 90f;        // 扫描角度
        [SerializeField] private float distance = 5f;     // 扫描距离
        [SerializeField] private float rotatePerSecond = 90f;  // 每秒旋转角度
        [SerializeField] private Sprite sprite;            // 扫描效果的Sprite

        public Tilemap wallTilemap;           // 墙的Tilemap
        public TilemapRenderer wallRenderer;  // 墙的渲染器

        void Start()
        {
            // 获取墙的Tilemap组件
            wallTilemap = GameObject.FindGameObjectWithTag("Map").GetComponent<Tilemap>();
            wallRenderer = wallTilemap.GetComponent<TilemapRenderer>();
        }

        void Update()
        {

            if (!Input.GetKeyDown(KeyCode.Space))
            {
                //旋转扫描效果
                transform.Rotate(0, 0, rotatePerSecond * Time.deltaTime);

                // 在扇形区域内检测墙壁
                CheckWallsInSector();
            }

            for (var x = wallTilemap.cellBounds.xMin; x < wallTilemap.cellBounds.xMax; x++)
            {
                for (var y = wallTilemap.cellBounds.yMin; y < wallTilemap.cellBounds.yMax; y++)
                {
                    wallTilemap.SetColor(new Vector3Int(x, y, 0), new Color(1, 1, 1, 0));
                }
            }

        }

        void CheckWallsInSector()
        {
            // 获取扇形区域内的所有瓦片位置
            Vector3 origin = transform.position;
            Vector3 direction = transform.right;

            // 将世界坐标转换为瓦片坐标
            Vector3Int centerCell = wallTilemap.WorldToCell(origin);

            // 计算检测范围（以瓦片为单位）
            int tileDistance = Mathf.CeilToInt(distance);

            // 遍历可能范围内的所有瓦片
            for (int x = -tileDistance; x <= tileDistance; x++)
            {
                for (int y = -tileDistance; y <= tileDistance; y++)
                {
                    Vector3Int checkPos = centerCell + new Vector3Int(x, y, 0);

                    // 如果该位置有墙
                    if (wallTilemap.HasTile(checkPos))
                    {
                        // 获取瓦片的世界坐标（使用中心点）
                        Vector3 tileWorldPos = wallTilemap.GetCellCenterWorld(checkPos);
                        Vector2 directionToTile = tileWorldPos - origin;

                        // 检查距离
                        if (directionToTile.magnitude <= distance)
                        {
                            // 检查角度
                            float angleToDot = Vector2.Angle(direction, directionToTile);
                            if (Mathf.Abs(angleToDot) <= angle / 2)
                            {
                                // 在扇形范围内，显示这个瓦片
                                // 可以通过修改瓦片的颜色或者替换瓦片来实现显示效果
                                wallTilemap.SetTileFlags(checkPos, TileFlags.None);
                                wallTilemap.SetColor(checkPos, Color.white); // 显示瓦片

                                // 可以启动协程来处理瓦片的淡出效果
                                StartCoroutine(FadeOutTile(checkPos));
                            }
                        }
                    }
                }
            }
        }

        private System.Collections.IEnumerator FadeOutTile(Vector3Int tilePosition)
        {
            float duration = 2.0f; // 淡入持续时间
            float elapsed = 0;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = 1 - (elapsed / duration);

                // 设置瓦片的透明度
                wallTilemap.SetColor(tilePosition, new Color(1, 1, 1, alpha));

                yield return null;
            }

            // 完全隐藏瓦片
            wallTilemap.SetColor(tilePosition, new Color(1, 1, 1, 0));
        }
    }

    //显示电波
    //public void Show()
    //{
    //    //animator.SetTrigger("Show");

    //    //throw new System.NotImplementedException();
    //}

    ////[Range(0, 360)] public float Angle = 90f;
    ////[Range(0, 100)] public float distance = 25f;
    ////public float rotatePerSecond = 90f;

    //private void CheckCollision()
    //{
    //    Vector2 face = transform.forward.normalized;
    //    var colliders = Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("wall"));
    //    var walls = new List<GameObject>();
    //    foreach (var col in colliders)
    //    {
    //        Vector2 pos = (col.transform.position - transform.position).normalized; //获取碰撞到的墙壁的位置
    //        var angleBetween = Vector2.Angle(face, pos);
    //        if (angleBetween < Angle / 2)
    //        {
    //            if (Physics.Raycast(transform.position, pos, out var hit, 1f))
    //            {
    //                Debug.DrawRay(transform.position, transform.forward.normalized * distance, Color.green);
    //                if (hit.collider == col)
    //                {
    //                    walls.Add(col.gameObject);
    //                }
    //            }
    //        }
    //    }

    //    // if (LookAround(Quaternion.identity, Color.green))
    //    // {
    //    // }


    //    //cast put walls list to delight
    //    // EventCenter.Broadcast(new WaveHitWallEvent
    //    // {
    //    //     hittedWalls = walls
    //    // });
    //    //throw new System.NotImplementedException();
    //}

    //private bool LookAround(Quaternion eulerAnger, Color DebugColor)
    //{
    //    Debug.DrawRay(transform.position, eulerAnger * transform.forward.normalized * distance, DebugColor);

    //    var hit = Physics2D.Raycast(transform.position, eulerAnger * transform.forward, distance);
    //    if (!hit) return false;
    //    Debug.Log(hit.collider.gameObject.name);
    //    return true;
    //}
}