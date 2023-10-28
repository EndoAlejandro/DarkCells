using UnityEngine;

namespace DarkHavoc.AttackComponents
{
    public interface ITakeDamage
    {
        Transform transform { get; }
        int Health { get; }
        bool IsAlive { get; }
        void TakeDamage(IDoDamage damageDealer);
        void Death();
    }
}