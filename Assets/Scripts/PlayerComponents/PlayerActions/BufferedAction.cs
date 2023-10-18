using UnityEngine;

namespace PlayerComponents.PlayerActions
{
    public abstract class BufferedAction
    {
        protected abstract bool InputTrigger { get; }
        protected abstract float BufferTime { get; }

        protected readonly Player Player;
        protected readonly InputReader InputReader;
        
        private float _timeSinceActionPressed;
        private float _timer;
        private bool _wasActionPressed;
        
        public bool IsAvailable => _wasActionPressed && _timeSinceActionPressed > _timer;

        protected BufferedAction(Player player, InputReader inputReader)
        {
            Player = player;
            InputReader = inputReader;
        }

        public void Tick()
        {
            _timer += Time.deltaTime;
            
            if(!InputTrigger) return;
            _timeSinceActionPressed = _timer + BufferTime;
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