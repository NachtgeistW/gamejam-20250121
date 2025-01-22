using Level;
using Plutono.Util;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Level
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private TilemapCollider2D collider2d;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!GameManager.Instance.IsGameBegin) return;
            if (!col.CompareTag("Player")) return;

            Debug.Log("Door collided with " + col.name);
            EventCenter.Broadcast(new GameEvent.GameOverEvent { IsWin = true });
        }
    }
}