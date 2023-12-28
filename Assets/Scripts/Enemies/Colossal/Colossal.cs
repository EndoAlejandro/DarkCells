using System;
using DarkHavoc.EntitiesInterfaces;
using UnityEngine;

namespace DarkHavoc.Enemies.Colossal
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Colossal : MonoBehaviour, IDoDamage, ITakeDamage, IEntity, IStunnable
    {
        public event Action OnDamageTaken;

        [SerializeField] private Transform midPoint;

        public float Damage => 1;
        public float StunTime => 1;
        public bool IsAlive => Health > 0f;
        public float Health { get; private set; }
        public Transform MidPoint => midPoint;

        public void DoDamage(ITakeDamage takeDamage, float damageMultiplier = 1) =>
            takeDamage.TakeDamage(this, damageMultiplier);

        public void TakeDamage(IDoDamage damageDealer, float damageMultiplier) => Health--;

        public void Death()
        {
            // TODO: Death.
        }

        public void Stun()
        {
            // TODO: Stun.
        }
    }
}