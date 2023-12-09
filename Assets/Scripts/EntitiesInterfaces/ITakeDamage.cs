using System;
using UnityEngine;

namespace DarkHavoc.EntitiesInterfaces
{
    public interface ITakeDamage : IMidPoint
    {
        event Action OnDamageTaken;
        Transform transform { get; }
        float Health { get; }
        bool IsAlive { get; }
        void TakeDamage(IDoDamage damageDealer, float damageMultiplier);
        void Death();
    }
}