using DarkHavoc.ServiceLocatorComponents;
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

            if (!_ended && _toLobbyTimer <= 0f)
            {
                _ended = true;
                ServiceLocator.Instance.GetService<TransitionManager>().LoadLobbyScene();
            }
        }

        public void FixedTick()
        {
        }

        public void OnEnter()
        {
            _toLobbyTimer = 5f;
            _player.ResetVelocity();
            _rigidbody.isKinematic = true;
        }

        public void OnExit() => _rigidbody.isKinematic = false;
    }
}