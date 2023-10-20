using UnityEngine;

namespace AttackComponents
{
    public interface IDoDamage
    {
        Transform transform { get; }
        int Damage { get; }
        void DoDamage();
    }
}