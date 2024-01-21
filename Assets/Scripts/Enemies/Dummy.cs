using System;
using DarkHavoc.EntitiesInterfaces;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    public class Dummy : MonoBehaviour, ITakeDamage
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private float knockBackForce = 2f;

        public event Action OnDamageTaken;
        public event Action OnDeath;

        private Rigidbody2D _rigidbody;

        private float _health;
        public float Health => _health;
        public float MaxHealth { get; private set; }
        public bool IsAlive => true;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _health = maxHealth;
        }

        public void TakeDamage(IDoDamage damageDealer, float damageMultiplier)
        {
            if (_health < 0) return;

            _health -= damageDealer.Damage * damageMultiplier;
            var dif = Mathf.Sign(transform.position.x - damageDealer.transform.position.x);
            _rigidbody.AddForce(new Vector2(dif * knockBackForce, 1f), ForceMode2D.Force);
            Debug.Log($"Damage:{damageDealer.Damage} || Health:{_health}");
            if (_health <= 0f) Death();
        }

        public void Death() => _health = maxHealth;
        public Transform MidPoint { get; }
    }
}