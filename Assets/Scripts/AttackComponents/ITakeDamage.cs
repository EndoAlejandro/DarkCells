using System;
using UnityEngine;

namespace DarkHavoc.AttackComponents
{
    public interface ITakeDamage
    {
        event Action OnDamageTaken;
        Transform transform { get; }
        int Health { get; }
        bool IsAlive { get; }
        void TakeDamage(IDoDamage damageDealer);
        void Death();
    }
}