using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.PlayerComponents.States
{
    public class BlockState : IState
    {
        public override string ToString() => "Block";
        public AnimationState AnimationState => AnimationState.Block;
        public bool Ended => _timer <= 0f;
        public bool CanTransitionToSelf => false;
        public bool ParryAvailable { get; private set; }

        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody;

        private float _timer;

        public BlockState(Player player, Rigidbody2D rigidbody, InputReader input)
        {
            _player = player;
            _rigidbody = rigidbody;
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;

            if (_player.HasBufferedJump && Ended)
            {
                _player.Jump();
                _player.ApplyVelocity();
            }
        }

        public void FixedTick() => _player.Move(0);

        public void OnEnter()
        {
            _player.TryToBlockDamage += PlayerOnTryToBlockDamage;

            _timer = _player.Stats.BlockTime;
            ParryAvailable = false;
        }

        private bool PlayerOnTryToBlockDamage(Vector2 damageSource)
        {
            float difference = damageSource.x - _player.transform.position.x;
            var result = (difference < 0 && _player.FacingLeft) || (difference > 0 && !_player.FacingLeft);

            if (result) ParryAvailable = true;
            if (!ParryAvailable) _player.AddImpulse(_player.Stats.ParryAction);
            
            return result;
        }

        public void OnExit() => _player.TryToBlockDamage -= PlayerOnTryToBlockDamage;
    }
}