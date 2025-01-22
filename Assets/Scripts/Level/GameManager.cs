using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Plutono.Util;
using UnityEngine;
using UnityEngine.Tilemaps;

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

        public GameObject enemyPrefab;
        public Transform enemiesTransform;

        public bool IsGameBegin { get; private set; } = false;
        public bool IsGameOver { get; private set; } = false;

        private void OnEnable()
        {
            EventCenter.AddListener<GameEvent.GameOverEvent>(OnGameOver);
        }
        private void OnDisable()
        {
            EventCenter.RemoveListener<GameEvent.GameOverEvent>(OnGameOver);
        }

        private void OnGameOver(GameEvent.GameOverEvent evt)
        {
            IsGameOver = true;
            if (evt.IsWin)
            {

            }
        }

        private void Start()
        {
            //map = InitMap();
            InitPlayer();
            InitEnemies();

            IsGameBegin = true;
        }

        private Map InitMap()
        {
            var map = new Map();

            var tilemap = GetComponent<Tilemap>();
            return map;
        }

        private void InitPlayer()
        {
            player = FindObjectOfType<Player>();

            var start = GridMapManager.Instance.GetTileDetailsList(MapTileType.Start)[0];
            player.MovePlayerTo(new Vector2Int(start.girdX, start.girdY));
        }

        private void InitEnemies()
        {
            var enemyTileDetails = GridMapManager.Instance.GetTileDetailsList(MapTileType.Enemy);

            foreach (var enemyTileDetail in enemyTileDetails)
            {
                var enemy = Instantiate(enemyPrefab, enemiesTransform).GetComponent<Enemy>();
                enemy.MoveTo(new Vector2Int(enemyTileDetail.girdX, enemyTileDetail.girdY));
            }
        }
    }
}