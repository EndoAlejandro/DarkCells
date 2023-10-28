using UnityEngine;

namespace DarkHavoc.AttackComponents
{
    public interface ITakeDamage
    {
        Transform transform { get; }
        int Health { get; }
        bool IsAlive { get; }
        void TakeDamage(int damage, Vector2 damageSource);
        void Death();
    }
}