using System;
using DarkHavoc.EntitiesInterfaces;
using UnityEngine;

namespace DarkHavoc.Enemies.CagedShocker
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class CagedShocker : Enemy, IStunnable
    {
        public event Action OnStunned;
        public override float Damage => Stats != null ? Stats.Damage : 0;
        
        public float StunTime { get; private set; }

        public void Stun() => OnStunned?.Invoke();
    }
}