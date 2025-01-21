using System;
using System.Collections.Generic;
using Plutono.Util;

namespace Level
{
    //Use this way to display Map in Inspector for convince
    [Serializable] public class Map { public List<MapTileList> map; }
    [Serializable] public class MapTileList { public List<MapTile> tiles; }

    public class GameManager : Singleton<GameManager>
    {
        public Map map;
        public Player player;
        public List<Enemy> enemies;

        private void Start()
        {
            map = InitMap();
            player = InitPlayer();
            enemies = InitEnemies();
        }

        private Map InitMap()
        {
            throw new NotImplementedException();
        }

        private Player InitPlayer()
        {
            throw new NotImplementedException();
        }

        private List<Enemy> InitEnemies()
        {
            throw new NotImplementedException();
        }
    }
}