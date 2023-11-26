using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.States
{
    public class BlockState : IState
    {
        public override string ToString() => "Block";
        public AnimationState Animation => AnimationState.Block;

        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody;
        private readonly ImputReader _input;

        private Vector2 _targetVelocity;
        private float _timer;

        public bool Ended => _timer <= 0f;
        public bool CanTransitionToSelf => false;
        public bool ParryAvailable { get; private set; }

        public BlockState(Player player, Rigidbody2D rigidbody, ImputReader input)
        {
            _player = player;
            _rigidbody = rigidbody;
            _input = input;
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
            _targetVelocity = _rigidbody.velocity;
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