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
        private bool _canJump;

        public GroundState(Player player, Rigidbody2D rigidbody, InputReader input)
        {
            _player = player;
            _rigidbody = rigidbody;
            _input = input;
        }

        public void Tick()
        {
            if (_canJump && _input.Jump)
            {
                _canJump = false;
                _player.Jump(ref _targetVelocity);
            }

            _player.Move(ref _targetVelocity, _input.Movement.x);
        }

        public void FixedTick()
        {
            _player.CheckCollisions(ref _targetVelocity);
            _player.CustomGravity(ref _targetVelocity);
            
            _player.ApplyVelocity(_targetVelocity);
        }

        public void OnEnter()
        {
            _canJump = true;
            _targetVelocity = _rigidbody.velocity;
        }

        public void OnExit()
        {
        }
    }
}