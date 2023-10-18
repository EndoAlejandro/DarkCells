using AttackComponents;
using UnityEngine;

namespace Enemies
{
    public class Dummy : MonoBehaviour, ITakeDamage
    {
        [SerializeField] private int maxHealth;
        public int Health { get; private set; }

        private void Awake() => Health = maxHealth;

        public void TakeDamage(int damage)
        {
            if (Health < 0) return;

            Health -= damage;
            Debug.Log($"Damage:{damage} || Health:{Health}");
            if (Health <= 0f) Death();
        }

        public void Death() => Health = maxHealth;
    }
}