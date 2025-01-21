﻿using UnityEngine;

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
        [field: SerializeField] public Vector2 Position { get; private set; }
        [field: SerializeField] public State State { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        
        [field: SerializeField] public Sprite Sprite { get; private set; }

        private void OnStay()
        {
            throw new System.NotImplementedException();
        }
        
        private void OnMove()
        {
            throw new System.NotImplementedException();
        }
        
        private void OnCatchPlayer()
        {
            throw new System.NotImplementedException();
        }
    }
}