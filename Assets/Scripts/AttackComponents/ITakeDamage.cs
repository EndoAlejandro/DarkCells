using UnityEngine;

namespace DarkHavoc.AttackComponents
{
    public interface ITakeDamage
    {
        Transform transform { get; }
        int Health { get; }
        bool IsAlive => Health > 0f;
        void TakeDamage(int damage, Vector2 damageSource);
        void Death();
    }
}