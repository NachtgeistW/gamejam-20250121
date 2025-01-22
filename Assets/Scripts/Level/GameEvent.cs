using Plutono.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class GameEvent
    {
        public struct WaveHitWallEvent : IEvent
        {
            public List<GameObject> hittedWalls;
        }

        public struct OnWaveHitEnemy : IEvent
        {
        }

        public struct OnPlayerReachDoor : IEvent
        {
        }

        public struct GameOverEvent : IEvent
        {
            public bool IsWin;
        }
    }
}