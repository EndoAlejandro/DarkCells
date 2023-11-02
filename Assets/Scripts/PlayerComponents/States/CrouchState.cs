using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.States
{
    public class CrouchState : IState
    {
        public override string ToString() => "Crouch";
        public AnimationState Animation => AnimationState.Crouch;

        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody;
        private Vector2 _targetVelocity;

        public bool CanTransitionToSelf => false;

        public CrouchState(Player player, Rigidbody2D rigidbody)
        {
            _player = player;
            _rigidbody = rigidbody;
        }

        public void Tick() => _player.Move(0);

        public void FixedTick()
        {
        }

        public void OnEnter() => _player.SetPlayerCollider(false);

        public void OnExit() => _player.SetPlayerCollider(true);
    }
}