using System;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.PlayerActions
{
    [Serializable]
    public abstract class BufferedAction
    {
        private float _timeSinceActionPressed;
        private bool _wasActionPressed;
        
        protected float Timer;
        protected readonly Player Player;
        protected readonly InputReader InputReader;
        
        protected abstract bool InputTrigger { get; }
        protected abstract float BufferTime { get; }
        
        public bool IsAvailable => _wasActionPressed && _timeSinceActionPressed > Timer;

        protected BufferedAction(Player player, InputReader inputReader)
        {
            Player = player;
            InputReader = inputReader;
        }

        public virtual void Tick()
        {
            Timer += Time.deltaTime;
            
            if(!InputTrigger) return;
            _timeSinceActionPressed = Timer + BufferTime;
            _wasActionPressed = true;
        }

        public void UseAction(ref Vector2 targetVelocity)
        {
            _wasActionPressed = false;
            UseBuffer(ref targetVelocity);
        }

        protected abstract void UseBuffer(ref Vector2 targetVelocity);
    }
}