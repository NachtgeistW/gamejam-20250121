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
        private bool isMoving;
        private bool gameover = false;
        private const float RotationSpeed = 100f;
        private bool isCollision = false;
        private float templine = 0.3f;

        public Grid grid;
        public Camera cam;

        private Rigidbody2D rb2d;

        private bool isMoveEnable;

        private void Start()
        {
            //grid = FindObjectOfType<Grid>();
            cam = Camera.main;
        }

        private void Update()
        {
            if (GameManager.Instance.IsGameOver)
            {
                isMoveEnable = false;
                return;
            }

            isMoveEnable = true;

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
            //        var hit = Physics2D.Raycast(transform.position, Vector2.right, templine, LayerMask.GetMask("Wall"));
            //        if (hit.collider != null) isCollision = true;
            //    }
            //    else
            //    {
            //        Debug.Log("isCollision");
            //        var hit = Physics2D.Raycast(transform.position, Vector2.right, templine, LayerMask.GetMask("Wall"));
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
            var pos = grid.WorldToCell(transform.position);
            Position = new Vector2Int(pos.x, pos.y);
        }
        //玩家移动


        //玩家死亡
        public void GameOver()
        {
            gameover = true;
            Destroy(gameObject);
        }

        //发射电波
        private void SendOutRadioWaves()
        {
            if (Input.GetKeyDown(KeyCode.Space)) Debug.Log("发射电波");
        }

        //获取玩家在地图上面的位置
        public Vector2Int GetPosition()
        {
            var pos = grid.WorldToCell(transform.position);
            Position = new Vector2Int(pos.x, pos.y);
            return new Vector2Int(pos.x, pos.y);
        }

        /// <summary>
        /// 将玩家的位置移动到指定位置
        /// </summary>
        /// <param name="position">要移动的位置</param>
        public void MovePlayerTo(Vector2Int position)
        {
            var cellSize = grid.cellSize.x;
            var worldPos = grid.CellToWorld(new Vector3Int(position.x, position.y));
            transform.position = new Vector3(worldPos.x + cellSize / 2, worldPos.y + cellSize / 2, transform.position.z);
        }
    }
}