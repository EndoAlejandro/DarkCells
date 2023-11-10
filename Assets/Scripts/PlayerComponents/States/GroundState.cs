using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.States
{
    public class DeathState : IState
    {
        public override string ToString() => "Death";
        public AnimationState Animation => AnimationState.Death;
        public bool CanTransitionToSelf => false;

        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody;
        
        public DeathState(Player player ,Rigidbody2D rigidbody)
        {
            _player = player;
            _rigidbody = rigidbody;
        }

        public void Tick()
        {
        }

        public void FixedTick()
        {
        }

        public void OnEnter()
        {
            _player.ResetVelocity();
            _rigidbody.isKinematic = true;
        }

        public void OnExit() => _rigidbody.isKinematic = false;
    }
    
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

            if (_player.HasBufferedJump)
            {
                _player.Jump();
                _player.ApplyVelocity();
            }
        }

        public void FixedTick()
        {
            _player.Move(_input.Movement.x);
            // _targetVelocity = _rigidbody.velocity;
        }

        public void OnEnter() => _targetVelocity = _rigidbody.velocity;

        public void OnExit()
        {
        }
    }
}