using System.Collections.Generic;
using Level;
using UnityEngine;

namespace Map.Data
{
    [CreateAssetMenu(fileName = "MapData_SO", menuName = "Map/MapData")]
    public class MapData_SO : ScriptableObject
    {
        public string mapName;
        public List<TileProperty> tileProperties;
    }
}