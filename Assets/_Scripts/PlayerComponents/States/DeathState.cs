using DarkHavoc.ServiceLocatorComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.PlayerComponents.States
{
    public class DeathState : IState
    {
        public override string ToString() => "Death";
        public AnimationState AnimationState => AnimationState.Death;
        public bool CanTransitionToSelf => false;

        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody;

        private float _toLobbyTimer;
        private bool _ended;

        public DeathState(Player player, Rigidbody2D rigidbody)
        {
            _player = player;
            _rigidbody = rigidbody;
        }

        public void Tick()
        {
            _toLobbyTimer -= Time.deltaTime;

            if (_ended || _toLobbyTimer > 0f) return;

            _ended = true;
            ServiceLocator.GetService<GameManager>().GoToLobby();
        }

        public void FixedTick() => _player.Move(0);

        public void OnEnter()
        {
            _toLobbyTimer = 5f;
            _player.ResetVelocity();
            _player.Death();
            _rigidbody.simulated = false;
        }

        public void OnExit() => _rigidbody.simulated = true;
    }
}