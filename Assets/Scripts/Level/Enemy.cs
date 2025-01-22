using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Level
{
    public enum State
    {
        Stay,
        Move,
        CatchPlayer
    }

    [RequireComponent(typeof(SpriteRenderer))]
    public class Enemy : MonoBehaviour
    {
        [field: SerializeField] public Vector2Int Position { get; private set; }// Position of the enemy on the map
        [field: SerializeField] public State State { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }

        [field: SerializeField] public Sprite Sprite { get; private set; }
        public Animator animator { get; private set; }
        public MapTile maptile { get; private set; }
        public Player targetPlayer { get; private set; }

        private void Update()
        {
            switch (State)
            {
                case State.Move:
                    OnMove();
                    break;
                case State.CatchPlayer:
                    OnCatchPlayer(targetPlayer);
                    break;
                case State.Stay:
                default:
                    OnStay();
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnStay()
        {
            animator.Play("Stay");
            //throw new System.NotImplementedException();
        }

        private void OnMove()
        {
            animator.Play("Move");
            //throw new System.NotImplementedException();
        }

        private void OnCatchPlayer(Player player)
        {
            animator.Play("CatchPlayer");
            FindPath(Position,player.getPosition());//寻路
            

            //throw new System.NotImplementedException();
        }

        //A*算法寻路
        public List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
        {
            List<Node> openList = new List<Node>();//列表存放待搜索的节点
            HashSet<Vector2Int> closedList = new HashSet<Vector2Int>();

            Node startNode = new Node(start, null, 0, Heuristic(start, target));//起始节点
            openList.Add(startNode);
            while (openList.Count > 0)
            {
                Node currentNode = openList[0];
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].F < currentNode.F)
                    {
                        currentNode = openList[i];
                    }
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode.position);

                if (currentNode.position == target)
                {
                    return BuildPath(currentNode);//逐个构建节点
                }//如果找到目标节点，则返回路径

                foreach (var neighbor in GetNeighbors(currentNode.position))//bfs遍历周围节点
                {
                    if (closedList.Contains(neighbor) || maptile.GetTile(neighbor).type == MapTileType.Wall)
                    {
                        continue;
                    }
                    float gCost = currentNode.gCost + 1;
                    Node neighborNode = openList.Find(n => n.position == neighbor);//查找节点是否已经在openList中
                    if (neighborNode == null || gCost < neighborNode.gCost)//如果节点不存在或g值更小
                    {
                        if (neighborNode != null)
                        {
                            neighborNode = new Node(neighbor, currentNode, gCost, Heuristic(neighbor, target));
                            openList.Add(neighborNode);
                        }
                        else
                        {
                            neighborNode.parent = currentNode;
                            neighborNode.gCost = gCost;
                        }
                    }
                }
            }
            return null;
        }
        private List<Vector2Int> GetNeighbors(Vector2Int position)
        {
            List<Vector2Int> neighbors = new List<Vector2Int>
            {
                new Vector2Int(position.x+1, position.y),
                new Vector2Int(position.x-1, position.y),
                new Vector2Int(position.x, position.y+1),
                new Vector2Int(position.x, position.y-1)
            };
            return neighbors;

        }
        private float Heuristic(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }
        private List<Vector2Int> BuildPath(Node node)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            while (node != null)
            {
                path.Add(node.position);
                node = node.parent;
            }
            path.Reverse();
            return path;
        }

    }

    class Node
    {
        public Vector2Int position;//节点位置
        public Node parent;
        public float gCost;
        public float hCost;

        public float F => gCost + hCost;

        public Node(Vector2Int position, Node parent, float gCost, float hCost)
        {
            this.position = position;
            this.parent = parent;
            this.gCost = gCost;
            this.hCost = hCost;
        }

    }
}