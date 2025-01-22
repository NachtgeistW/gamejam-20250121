using Plutono.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Level.GameEvent;

namespace Level
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class RadioWave : MonoBehaviour
    {
        [field: SerializeField] public Vector2 Position { get; set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }

        public Animator animator { get; private set; }

        private Grid curGrid;

        private void Start()
        {
            curGrid = FindObjectOfType<Grid>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CheckCollision();
            }
        }

        //显示电波
        public void Show()
        {
            //animator.SetTrigger("Show");

            //throw new System.NotImplementedException();
        }

        [Range(0, 360)] public float Angle = 90f;
        [Range(0, 100)] public float distance = 25f;
        public float rotatePerSecond = 90f;

        private void CheckCollision()
        {
            Vector2 face = transform.forward.normalized;
            var colliders = Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("wall"));
            var walls = new List<GameObject>();
            foreach (var col in colliders)
            {
                Vector2 pos = (col.transform.position - transform.position).normalized; //获取碰撞到的墙壁的位置
                var angleBetween = Vector2.Angle(face, pos);
                if (angleBetween < Angle / 2)
                {
                    if (Physics.Raycast(transform.position, pos, out var hit, 1f))
                    {
                        Debug.DrawRay(transform.position, transform.forward.normalized * distance, Color.green);
                        if (hit.collider == col)
                        {
                            walls.Add(col.gameObject);
                        }
                    }
                }
            }

            // if (LookAround(Quaternion.identity, Color.green))
            // {
            // }


            //cast put walls list to delight
            // EventCenter.Broadcast(new WaveHitWallEvent
            // {
            //     hittedWalls = walls
            // });
            //throw new System.NotImplementedException();
        }

        private bool LookAround(Quaternion eulerAnger, Color DebugColor)
        {
            Debug.DrawRay(transform.position, eulerAnger * transform.forward.normalized * distance, DebugColor);

            var hit = Physics2D.Raycast(transform.position, eulerAnger * transform.forward, distance);
            if (!hit) return false;
            Debug.Log(hit.collider.gameObject.name);
            return true;
        }
    }
}