using System;
using DarkHavoc.PlayerComponents;
using UnityEngine;

namespace DarkHavoc.EntitiesInterfaces
{
    public interface IEntity
    {
        bool FacingLeft { get; }
        Transform MidPoint { get; }
        event Action<bool> OnXFlipped;
    }

    public interface IEnemy : IEntity
    {
        Player Player { get; }
        void ActivateBuff(float duration);
    }
}