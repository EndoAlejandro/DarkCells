using UnityEngine;

namespace DarkHavoc.EntitiesInterfaces
{
    public interface IDoDamage
    {
        Transform transform { get; }
        float Damage { get; }
        void DoDamage(ITakeDamage takeDamage, float damageMultiplier = 1f, bool unstoppable = false);
    }
}