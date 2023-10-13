using StateMachineComponents;
using UnityEngine;

namespace PlayerComponents.States
{
    public class JumpState : IState
    {
        public override string ToString() => "Jump";

        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody;
        private readonly InputReader _input;

        private Vector2 _targetVelocity;

        public JumpState(Player player, Rigidbody2D rigidbody, InputReader input)
        {
            _player = player;
            _rigidbody = rigidbody;
            _input = input;
        }

        public void Tick()
        {
        }

        public void FixedTick()
        {
            _targetVelocity = _rigidbody.velocity;
            _player.Move(ref _targetVelocity, _input.Movement.x);
            _player.CustomGravity(ref _targetVelocity);
            _player.ApplyVelocity(_targetVelocity);
        }

        public void OnEnter()
        {
            _targetVelocity = _rigidbody.velocity;
            _player.Jump(ref _targetVelocity);
            _player.ApplyVelocity(_targetVelocity);
        }

        public void OnExit()
        {
        }
    }
}