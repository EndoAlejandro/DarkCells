using UnityEngine;

namespace DarkHavoc.AttackComponents
{
    public interface IDoDamage
    {
        Transform transform { get; }
        float Damage { get; }
        void DoDamage(ITakeDamage takeDamage, float damageMultiplier = 1f);
    }
}