using Plutono.Util;
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

        //[field: SerializeField] public Sprite Sprite { get; private set; }

        private void Update()
        {
            Move();
            SendOutRadioWaves();
        }

        //玩家移动
        private void Move()
        {
            InputX = Input.GetAxisRaw("Horizontal");
            InputY = Input.GetAxisRaw("Vertical");
            Vector2 movement = new Vector2(InputX, InputY);
            transform.LookAt((Vector2)transform.position + movement);

            transform.position += new Vector3((movement * (Speed * Time.deltaTime)).x,
                (movement * (Speed * Time.deltaTime)).y, 0f);
            //throw new System.NotImplementedException();
        }

        //发射电波
        private void SendOutRadioWaves()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                throw new System.NotImplementedException();
            }
        }

        //获取玩家在地图上面的位置
        public Vector2Int getPosition()
        {
            throw new System.NotImplementedException();
        }
    }
}