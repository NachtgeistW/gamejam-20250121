using Plutono.Util;

namespace Level
{
    public class GameEvent
    {
        public struct OnWaveHitWall : IEvent
        {
        }

        public struct OnWaveHitEnemy : IEvent
        {
        }

        public struct OnPlayerReachDoor : IEvent
        {
        }
    }
}