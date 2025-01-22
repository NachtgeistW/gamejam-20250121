using Plutono.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

            Debug.Log($"Patrol points count: {patrolPoints.Count}");
            foreach (var point in patrolPoints)
            {
                Debug.Log($"Patrol point: {point}");
            }

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
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            float distanceToTarget = Vector2.Distance(transform.position, targetPosition);

            if (distanceToTarget < 0.1f)
            {
                // 到达目标点，更新索引
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
                // 确保物体停止
                rb.velocity = Vector2.zero;
            }
            else
            {
                // 使用 MovePosition 而不是直接设置 velocity
                Vector2 moveDirection = (targetPosition - transform.position).normalized;
                Vector2 newPosition = rb.position + (moveDirection * patrolSpeed * Time.fixedDeltaTime);
                rb.MovePosition(newPosition);
            }
        }

        private void ChaseBehavior()
        {
            Vector3 playerPosition = player.transform.position;
            Vector2 moveDirection = (playerPosition - transform.position).normalized;
            Vector2 newPosition = rb.position + (moveDirection * chaseSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (currentState == EnemyState.Chasing && other.CompareTag("Player"))
            {
                EventCenter.Broadcast(new GameEvent.GameOverEvent { IsWin = false });
            }
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