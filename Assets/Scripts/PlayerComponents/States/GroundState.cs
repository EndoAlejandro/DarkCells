using StateMachineComponents;
using UnityEngine;

namespace PlayerComponents.States
{
    public class GroundState : IState
    {
        public override string ToString() => AnimationState.Ground.ToString();

        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody;
        private readonly InputReader _input;

        private Vector2 _targetVelocity;

        public bool CanTransitionToSelf => false;

        public GroundState(Player player, Rigidbody2D rigidbody, InputReader input)
        {
            _player = player;
            _rigidbody = rigidbody;
            _input = input;
        }

        public void Tick()
        {
            if (_player.CheckCeilingCollision()) return;

            if (_player.HasBufferedJump)
                _player.Jump(ref _targetVelocity);

            _player.Move(ref _targetVelocity, _input.Movement.x);
        }

        public void FixedTick()
        {
            _player.CheckCollisions(ref _targetVelocity);
            _player.CustomGravity(ref _targetVelocity);

            _player.ApplyVelocity(_targetVelocity);
            _targetVelocity = _rigidbody.velocity;
        }

        public void OnEnter() => _targetVelocity = _rigidbody.velocity;

        public void OnExit()
        {
        }
    }
}