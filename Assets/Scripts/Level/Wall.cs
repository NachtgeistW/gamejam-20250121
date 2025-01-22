using Plutono.Util;
using System.Collections;
using UnityEngine;
using static Level.GameEvent;

namespace Assets.Scripts.Level
{
    public class Wall : MonoBehaviour
    {

        void OnEnable()
        {
            EventCenter.AddListener<WaveHitWallEvent>(OnWaveHitWall);
        }
        void OnDisable()
        {
            EventCenter.RemoveListener<WaveHitWallEvent>(OnWaveHitWall);
        }

        void OnWaveHitWall(WaveHitWallEvent e)
        {
            foreach(var wall in e.hittedWalls)
            {
                StartCoroutine(Delight());
            }
        }
        IEnumerator Delight()
        {
            throw new System.NotImplementedException();
            yield return new WaitForSeconds(0.5f);

        }
    }
}