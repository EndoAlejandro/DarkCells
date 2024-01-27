using System;
using UnityEngine;

namespace DarkHavoc.EntitiesInterfaces
{
    public interface ITakeDamage : IMidPoint
    {
        event Action OnDamageTaken;
        event Action OnDeath;
        Transform transform { get; }
        float Health { get; }
        float MaxHealth { get; }
        bool IsAlive { get; }
        void TakeDamage(IDoDamage damageDealer, float damageMultiplier, bool isUnstoppable);
        void Death();
    }
}