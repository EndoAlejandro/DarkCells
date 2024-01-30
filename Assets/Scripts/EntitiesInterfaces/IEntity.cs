using System;

namespace DarkHavoc.EntitiesInterfaces
{
    public interface IEntity
    {
        event Action<bool> OnXFlipped;
    }
}