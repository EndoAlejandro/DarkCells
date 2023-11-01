using System;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.PlayerActions
{
    [Serializable]
    public class BufferedAction
    {
        public bool IsAvailable => _wasActionPressed && _timeSinceActionPressed > Timer;

        protected readonly Player Player;
        private event Func<bool> InputTrigger;

        protected float Timer;
        private float _bufferTime;
        private float _timeSinceActionPressed;

        private bool _wasActionPressed;

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
            _wasActionPressed = true;
        }

        public virtual void UseAction() => _wasActionPressed = false;
    }
}