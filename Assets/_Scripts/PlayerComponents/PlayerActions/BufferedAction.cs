using System;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.PlayerActions
{
    [Serializable]
    public class BufferedAction
    {
        public virtual bool IsAvailable => WasActionPressed && _timeSinceActionPressed > Timer;

        protected readonly Player Player;
        private event Func<bool> InputTrigger;

        protected float Timer;
        protected bool WasActionPressed;
        
        private float _bufferTime;
        private float _timeSinceActionPressed;

        public BufferedAction(Player player, float bufferTime, Func<bool> inputTrigger)
        {
            Player = player;
            _bufferTime = bufferTime;
            InputTrigger = inputTrigger;
        }

        protected virtual bool CanBuffer() => InputTrigger?.Invoke() == true;

        public virtual void Tick()
        {
            Timer += Time.deltaTime;

            if (!CanBuffer()) return;
            _timeSinceActionPressed = Timer + _bufferTime;
            WasActionPressed = true;
        }

        public virtual void UseAction() => WasActionPressed = false;
    }
}