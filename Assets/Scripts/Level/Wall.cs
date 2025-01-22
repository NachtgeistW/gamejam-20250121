using Plutono.Util;
using System.Collections;
using UnityEngine;
using static Level.GameEvent;

namespace Assets.Scripts.Level
{
    public class Wall : MonoBehaviour
    {
        private void OnEnable()
        {
            EventCenter.AddListener<WaveHitWallEvent>(OnWaveHitWall);
        }

        private void OnDisable()
        {
            EventCenter.RemoveListener<WaveHitWallEvent>(OnWaveHitWall);
        }

        private void OnWaveHitWall(WaveHitWallEvent e)
        {
        }

        private void Start()
        {

        }
    }
}