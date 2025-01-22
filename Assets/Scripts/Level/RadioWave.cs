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
        [field: SerializeField] public Vector2 WPosition { get; set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }

        public Animator Animator { get; private set; }
        public float Angle { get; private set; }

        private void Update()
        {
            CheckCollision();
        }

        //显示电波
        public void Show()
        {
            //animator.SetTrigger("Show");

            //throw new System.NotImplementedException();
        }

        private void CheckCollision()
        {
            Vector2 face = transform.forward.normalized;
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1f, LayerMask.GetMask("wall"));
            List<GameObject> walls = new List<GameObject>();
            foreach (Collider col in colliders)
            {
                Vector2 pos = (col.transform.position - transform.position).normalized; //获取碰撞到的墙壁的位置
                float angleBetween = Vector2.Angle(face, pos);
                if (angleBetween < Angle / 2)
                {
                    if (Physics.Raycast(transform.position, pos, out RaycastHit hit, 1f))
                    {
                        if (hit.collider == col)
                        {
                            walls.Add(col.gameObject);
                        }
                    }
                }
            }

            //cast put walls list to delight
            EventCenter.Broadcast(new WaveHitWallEvent { hittedWalls = walls });
            //throw new System.NotImplementedException();
        }
    }
}