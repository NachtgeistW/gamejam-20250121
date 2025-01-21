using UnityEngine;

namespace Level
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class RadioWave : MonoBehaviour
    {
        [field: SerializeField] public Vector2 Position { get; set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }

        //显示电波
        public void Show()
        {
            throw new System.NotImplementedException();
        }
    }
}