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

        public void TakeDamage(int damage, Vector2 damageSource)
        {
            if (Health < 0) return;

            Health -= damage;
            var dif = Mathf.Sign(transform.position.x - damageSource.x);
            _rigidbody.AddForce(new Vector2(dif * knockBackForce, 1f), ForceMode2D.Force);
            Debug.Log($"Damage:{damage} || Health:{Health}");
            if (Health <= 0f) Death();
        }

        public void Death() => Health = maxHealth;
    }
}