using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.States
{
    public class GroundState : IState
    {
        public override string ToString() => "Grounded";
        public AnimationState Animation => AnimationState.Ground;

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

            _player.Move(_input.Movement.x);
            if (_player.HasBufferedJump)
            {
                _player.Jump();
                _player.ApplyVelocity();
            }
        }

        public void FixedTick()
        {
            _targetVelocity = _rigidbody.velocity;
        }

        public void OnEnter() => _targetVelocity = _rigidbody.velocity;

        public void OnExit()
        {
        }
    }
}