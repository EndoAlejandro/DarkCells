using System;
using DarkHavoc.AttackComponents;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    public class Dummy : MonoBehaviour, ITakeDamage
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private float knockBackForce = 2f;

        public event Action<IDoDamage> OnTakeDamage;

        private Rigidbody2D _rigidbody;

        public int Health { get; private set; }
        public bool IsAlive => true;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            Health = maxHealth;
        }

        public void TakeDamage(IDoDamage damageDealer)
        {
            if (Health < 0) return;

            Health -= damageDealer.Damage;
            var dif = Mathf.Sign(transform.position.x - damageDealer.transform.position.x);
            _rigidbody.AddForce(new Vector2(dif * knockBackForce, 1f), ForceMode2D.Force);
            Debug.Log($"Damage:{damageDealer.Damage} || Health:{Health}");
            if (Health <= 0f) Death();
        }

        public void Death() => Health = maxHealth;
    }
}