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

        private Collider2D _collider;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();

            _collider.enabled = false;
            _rigidbody.isKinematic = true;
        }

        public void Setup()
        {
            _rigidbody.isKinematic = false;
            _collider.enabled = true;
        }

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