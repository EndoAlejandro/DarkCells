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
        private readonly InputReader _input;

        private Vector2 _targetVelocity;

        private float _parryTime;
        private float _timer;

        public bool Ended => _timer <= 0f;
        public bool ParryAvailable { get; private set; }
        public bool CanTransitionToSelf => false;

        public BlockState(Player player, Rigidbody2D rigidbody, InputReader input)
        {
            _player = player;
            _rigidbody = rigidbody;
            _input = input;
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;
            _player.Move(0);
            
            if (_player.HasBufferedJump)
            {
                _player.Jump();
                _player.ApplyVelocity();
            }
        }

        public void FixedTick()
        {
        }

        public void OnEnter()
        {
            _player.TryToBlockDamage += PlayerOnTryToBlockDamage;

            ParryAvailable = false;
            _timer = _player.Stats.BlockTime;
            _targetVelocity = _rigidbody.velocity;
        }

        private bool PlayerOnTryToBlockDamage(Vector2 damageSource)
        {
            float difference = damageSource.x - _player.transform.position.x;
            ParryAvailable = (difference < 0 && _player.FacingLeft) || (difference > 0 && !_player.FacingLeft);
            return ParryAvailable;
        }

        public void OnExit()
        {
            _player.TryToBlockDamage -= PlayerOnTryToBlockDamage;
            ParryAvailable = false;
        }
    }
}