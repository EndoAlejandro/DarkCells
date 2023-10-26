using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.States
{
    public class CrouchState : IState
    {
        public override string ToString() => "Crouch";
        public AnimationState Animation  => AnimationState.Crouch;

        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody;
        private Vector2 _targetVelocity;

        public bool CanTransitionToSelf => false;

        public CrouchState(Player player, Rigidbody2D rigidbody)
        {
            _player = player;
            _rigidbody = rigidbody;
        }

        public void Tick()
        {
        }

        public void FixedTick()
        {
            _player.ApplyVelocity(_targetVelocity);
        }

        public void OnEnter()
        {
            _targetVelocity = _rigidbody.velocity;
            _targetVelocity.x *= 0.25f;

            _player.SetPlayerCollider(false);
        }

        public void OnExit()
        {
            _player.SetPlayerCollider(true);
        }
    }
}