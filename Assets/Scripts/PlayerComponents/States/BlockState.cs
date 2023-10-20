using StateMachineComponents;
using UnityEngine;

namespace PlayerComponents.States
{
    public class BlockState : IState
    {
        public override string ToString() => AnimationState.Block.ToString();

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

            _player.OnAttackBlocked += PlayerOnAttackBlocked;
        }

        private void PlayerOnAttackBlocked() => ParryAvailable = true;

        public void Tick()
        {
            _timer -= Time.deltaTime;

            var moveMultiplier = !_player.Grounded ? _player.Stats.AttackMoveVelocity : 0f;
            _player.Move(ref _targetVelocity, _input.Movement.x * moveMultiplier);
            
            if (_player.HasBufferedJump) _player.Jump(ref _targetVelocity);
        }

        public void FixedTick()
        {
            _player.CheckCollisions(ref _targetVelocity);
            _player.CustomGravity(ref _targetVelocity);

            _player.ApplyVelocity(_targetVelocity);
        }

        public void OnEnter()
        {
            ParryAvailable = false;
            _timer = _player.Stats.BlockTime;
            _targetVelocity = _rigidbody.velocity;
        }

        public void OnExit() => ParryAvailable = false;
    }
}