using StateMachineComponents;
using UnityEngine;

namespace PlayerComponents.States
{
    public class GroundState : IState
    {
        public override string ToString() => "Ground";

        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody;
        private readonly InputReader _input;

        private Vector2 _targetVelocity;

        public GroundState(Player player, Rigidbody2D rigidbody, InputReader input)
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
        }

        public void OnExit()
        {
        }
    }
}