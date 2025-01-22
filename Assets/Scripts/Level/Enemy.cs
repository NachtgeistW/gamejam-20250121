using Plutono.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Level
{
    public enum EnemyState
    {
        Hidden,     // 初始状态：隐藏且静止
        Patrolling, // 被发现后开始巡逻
        Chasing     // 第二次被发现后追击玩家
    }

    [RequireComponent(typeof(SpriteRenderer))]
    public class Enemy : MonoBehaviour
    {
        public Grid Grid;

        [SerializeField] private float patrolSpeed = 1f;    // 巡逻速度
        [SerializeField] private float chaseSpeed = 2f;     // 追击速度
        [SerializeField] private float catchDistance = 0.5f; // 捕获玩家的距离

        [SerializeField] private EnemyState currentState = EnemyState.Hidden;
        [SerializeField] SpriteRenderer spriteRenderer;
        private List<Vector2Int> patrolPoints;
        private int currentPatrolIndex = 0;
        private Grid grid;
        private Player player;
        private Vector2Int currentGridPosition;

        [SerializeField] private Rigidbody2D rb;

        private void Start()
        {
            grid = FindObjectOfType<Grid>();
            player = FindObjectOfType<Player>();

            // 获取所有巡逻点
            patrolPoints = GridMapManager.Instance.GetTileDetailsList(MapTileType.EnemyObstacle)
                .Select(tile => new Vector2Int(tile.girdX, tile.girdY))
                .ToList();

            // 设置初始位置为第一个巡逻点
            currentGridPosition = patrolPoints[0];
            transform.position = GridToWorld(currentGridPosition);

            // 初始状态：隐藏
            SetVisibility(false);
        }

        private void SetVisibility(bool visible)
        {
            spriteRenderer.color = visible ? Color.white : Color.clear;
        }

        private Vector3 GridToWorld(Vector2Int gridPosition)
        {
            var cellSize = grid.cellSize.x;
            var worldPos = grid.CellToWorld(new Vector3Int(gridPosition.x, gridPosition.y, 0));
            return new Vector3(worldPos.x + cellSize / 2, worldPos.y + cellSize / 2, 0);
        }

        private void Update()
        {
            switch (currentState)
            {
                case EnemyState.Hidden:
                    // 不做任何事
                    break;

                case EnemyState.Patrolling:
                    PatrolBehavior();
                    break;

                case EnemyState.Chasing:
                    ChaseBehavior();
                    break;
            }
        }

        private void PatrolBehavior()
        {
            Vector3 targetPosition = GridToWorld(patrolPoints[currentPatrolIndex]);
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            // 移动
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                patrolSpeed * Time.deltaTime
            );

            // 检查是否到达目标点
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                // 移动到下一个巡逻点
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
            }
        }

        private void ChaseBehavior()
        {
            // 使用 OverlapCircle 检测是否接触到玩家，而不是用距离判断
            var colliders = Physics2D.OverlapCircleAll(transform.position, catchDistance);
            if (colliders.Any(collider => collider.CompareTag("Player")))
            {
                EventCenter.Broadcast(new GameEvent.GameOverEvent { IsWin = false });
                return; // 确保在触发游戏结束后不再移动
            }

            // 获取到玩家的方向
            var directionToPlayer = (player.transform.position - transform.position).normalized;

            // 使用 Rigidbody2D 来移动，而不是直接设置位置
            rb.velocity = directionToPlayer * chaseSpeed;
        }

        // 被雷达波检测到时调用
        public void OnDetected()
        {
            SetVisibility(true);

            currentState = currentState switch
            {
                EnemyState.Hidden => EnemyState.Patrolling,
                EnemyState.Patrolling => EnemyState.Chasing,
                _ => currentState
            };
        }

        // 用于在 Scene 视图中显示巡逻路径
        private void OnDrawGizmos()
        {
            if (patrolPoints is { Count: > 0 })
            {
                Gizmos.color = Color.yellow;
                for (int i = 0; i < patrolPoints.Count; i++)
                {
                    Vector3 pos = GridToWorld(patrolPoints[i]);
                    Gizmos.DrawWireSphere(pos, 0.3f);
                    if (i < patrolPoints.Count - 1)
                    {
                        Vector3 nextPos = GridToWorld(patrolPoints[i + 1]);
                        Gizmos.DrawLine(pos, nextPos);
                    }
                }
            }
        }

        /// <summary>
        /// 将敌人的位置移动到指定瓦片地图的正中心
        /// </summary>
        /// <param name="position">要移动的位置</param>
        public void MoveTo(Vector2Int position)
        {
            if (Grid == null)
            {
                Grid = FindObjectOfType<Grid>();
            }

            var worldPos = Grid.GetCellCenterWorld(new Vector3Int(position.x, position.y));
            transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);
        }
    }
}