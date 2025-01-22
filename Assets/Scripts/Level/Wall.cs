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
            foreach (var wall in e.hittedWalls)
            {
                StartCoroutine(Delight());
            }
        }

        private IEnumerator Delight()
        {
            throw new System.NotImplementedException();
            yield return new WaitForSeconds(0.5f);
        }
    }
}