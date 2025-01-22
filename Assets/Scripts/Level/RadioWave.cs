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

        [SerializeField] private float radius = 5f; // 扫描半径
        [SerializeField] private float angle = 120f; // 扇形角度
        [SerializeField] private int rayCount = 8; // 射线数量
        [SerializeField] private float wallVisibleTime = 3f; // 墙壁显示时间

        private void ScanArea()
        {
            float startAngle = transform.eulerAngles.z - angle / 2;
            float angleStep = angle / (rayCount - 1);

            for (int i = 0; i < rayCount; i++)
            {
                float currentAngle = startAngle + angleStep * i;
                Vector2 direction = GetVectorFromAngle(currentAngle);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, radius, LayerMask.GetMask("Wall"));

                if (hit.collider != null)
                {
                    // 检查是否击中TileMap
                    var tilemap = hit.collider.GetComponent<TilemapRenderer>();
                    if (tilemap != null)
                    {
                        Debug.Log(hit.transform.position);
                        // 使TileMap可见
                        StartCoroutine(ShowWallTemporarily(tilemap));
                    }
                }
            }
        }

        private IEnumerator ShowWallTemporarily(TilemapRenderer tilemap)
        {
            // 存储原始颜色
            Color originalColor = tilemap.material.color;

            // 设置为可见
            Color visibleColor = originalColor;
            visibleColor.a = 1f;
            tilemap.material.color = visibleColor;

            // 等待指定时间
            yield return new WaitForSeconds(wallVisibleTime);

            // 恢复为不可见
            tilemap.material.color = originalColor;
        }

        private Vector2 GetVectorFromAngle(float angle)
        {
            float angleRad = angle * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        // 在Update或者其他适当的时机调用ScanArea()
        private void Update()
        {
            // 可以根据需要添加触发条件
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("发射电波");
                ScanArea();

            }
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