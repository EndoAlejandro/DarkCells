using System;
using DarkHavoc.PlayerComponents;

namespace DarkHavoc.EntitiesInterfaces
{
    public interface IEntity
    {
        event Action<bool> OnXFlipped;
    }

    public interface IEnemy : IEntity
    {
        Player Player { get; }
        void ActivateBuff(float duration);
    }
}