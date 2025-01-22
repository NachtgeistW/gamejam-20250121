using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Level
{
    public enum MapTileType
    {
        Empty,
        Start,
        Wall,
        Road,
        Door,
        EnemyObstacle,
    }
}