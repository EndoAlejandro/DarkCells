using System;
using DarkHavoc.EntitiesInterfaces;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    public abstract class Enemy : MonoBehaviour, IDoDamage, ITakeDamage, IEntity
    {
        public event Action OnDamageTaken;
        public event Action OnDeath;
        public event Action<bool> OnXFlipped;
        public float Health { get; protected set; } = 1;
        public float MaxHealth { get; protected set; }
        public bool IsAlive => Health > 0f;
        public Transform MidPoint => midPoint;
        public bool FacingLeft { get; private set; }
        public EnemyStats BaseStats => stats;
        public abstract float Damage { get; }

        [SerializeField] protected EnemyStats stats;
        [SerializeField] private Transform midPoint;

        public abstract float GetNormalizedHorizontal();

        public void SetFacingLeft(bool facingLeft)
        {
            FacingLeft = facingLeft;
            OnXFlipped?.Invoke(FacingLeft);
        }

        public void DoDamage(ITakeDamage takeDamage, float damageMultiplier = 1, bool unstoppable = false) =>
            takeDamage.TakeDamage(this, damageMultiplier, unstoppable);

        public virtual void TakeDamage(IDoDamage damageDealer, float damageMultiplier, bool isUnstoppable)
        {
            if (!IsAlive) return;
            Health = Mathf.Max(Health - damageDealer.Damage, 0f);
            OnDamageTaken?.Invoke();

            if (!IsAlive) Death();
        }

        public abstract void Move(int direction);

        public virtual void Death() => OnDeath?.Invoke();
    }
}