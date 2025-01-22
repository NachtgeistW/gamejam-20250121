using Plutono.Util;
using System.Collections;
using UnityEngine;

namespace Level
{
    //[RequireComponent(typeof(SpriteRenderer))]
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public Vector2 Position { get; private set; }

        [field: SerializeField] public float InputX { get; private set; }
        [field: SerializeField] public float InputY { get; private set; }
        private Vector2 movementInput;
        private const float RotationSpeed = 100f;
        private const bool isCollision = false;
        private const float tempLine = 0.3f;

        public Grid Grid;
        public Camera Cam;

        private Rigidbody2D rb2d;

        private bool isMoveEnable = true;


        private void OnEnable()
        {
            EventCenter.AddListener<GameEvent.GameOverEvent>(OnGameOver);
        }
        private void OnDisable()
        {
            EventCenter.RemoveListener<GameEvent.GameOverEvent>(OnGameOver);
        }

        private void Start()
        {
            //grid = FindObjectOfType<Grid>();
            Cam = Camera.main;
        }

        private void Update()
        {
            if (GameManager.Instance.IsGameOver)
            {
                isMoveEnable = false;
                return;
            }

            Move();
            UpdatePosition();
            SendOutRadioWaves();
        }

        private void Move()
        {
            //while (gameover == false)
            //{
            //    InputX = Input.GetAxisRaw("Horizontal"); //rotation
            //    transform.Rotate(0, 0, -InputX * rotationspeed * Time.deltaTime);

            //    if (isCollision == false)
            //    {
            //        InputY = Input.GetAxisRaw("Vertical"); //movement
            //        transform.Translate(InputY * Speed * Time.deltaTime, 0, 0, Space.Self);
            //        var hit = Physics2D.Raycast(transform.position, Vector2.right, tempLine, LayerMask.GetMask("Wall"));
            //        if (hit.collider != null) isCollision = true;
            //    }
            //    else
            //    {
            //        Debug.Log("isCollision");
            //        var hit = Physics2D.Raycast(transform.position, Vector2.right, tempLine, LayerMask.GetMask("Wall"));
            //        if (hit.collider == null) isCollision = false;
            //    }

            //    UpdatePosition();

            //    yield return null;
            //}
            if (!isMoveEnable) return;

            InputX = Input.GetAxisRaw("Horizontal"); //rotation
            transform.Rotate(0, 0, -InputX * RotationSpeed * Time.deltaTime);

            if (isCollision) return;
            InputY = Input.GetAxisRaw("Vertical"); //movement
            transform.Translate(InputY * Speed * Time.deltaTime, 0, 0, Space.Self);
        }

        private void UpdatePosition()
        {
            var pos = Grid.WorldToCell(transform.position);
            Position = new Vector2Int(pos.x, pos.y);
        }

        //发射电波
        private void SendOutRadioWaves()
        {
            if (Input.GetKeyDown(KeyCode.Space)) Debug.Log("发射电波");
        }

        private void OnGameOver(GameEvent.GameOverEvent evt)
        {
            isMoveEnable = false;

            if (!evt.IsWin)
            {
                Destroy(gameObject);
            }
        }

        //获取玩家在地图上面的位置
        public Vector2Int GetPosition()
        {
            var pos = Grid.WorldToCell(transform.position);
            return new Vector2Int(pos.x, pos.y);
        }

        /// <summary>
        /// 将玩家的位置移动到指定瓦片地图的正中心
        /// </summary>
        /// <param name="position">要移动的位置</param>
        public void MovePlayerTo(Vector2Int position)
        {
            var cellSize = Grid.cellSize.x;
            var worldPos = Grid.CellToWorld(new Vector3Int(position.x, position.y));
            transform.position = new Vector3(worldPos.x + cellSize / 2, worldPos.y + cellSize / 2, transform.position.z);
        }
    }
}