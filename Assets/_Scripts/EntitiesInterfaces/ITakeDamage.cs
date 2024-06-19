using System;
using UnityEngine;

namespace DarkHavoc.EntitiesInterfaces
{
    public enum DamageResult
    {
        Success,
        Blocked,
        Killed,
        Failed
    }
    
    public interface ITakeDamage : IMidPoint
    {
        event Action OnDamageTaken;
        event Action<ITakeDamage> OnDeath;
        Transform transform { get; }
        float Health { get; }
        float MaxHealth { get; }
        bool IsAlive { get; }
        DamageResult TakeDamage(IDoDamage damageDealer, float damageMultiplier, bool isUnstoppable);
        void Death();
    }
}