using CustomUtils;
using StateMachineComponents;
using UnityEngine;
using AnimationState = PlayerComponents.AnimationState;

namespace Enemies.CagedShockerStates
{
    public class IdleState : IState
    {
        public override string ToString() => AnimationState.Ground.ToString();

        private readonly CagedShocker _cagedShocker;
        private readonly Rigidbody2D _rigidbody;
        private float _timer;
        private Vector2 _targetVelocity;

        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;

        public IdleState(CagedShocker cagedShocker, Rigidbody2D rigidbody)
        {
            _cagedShocker = cagedShocker;
            _rigidbody = rigidbody;
        }

        public void Tick() => _timer -= Time.deltaTime;

        public void FixedTick()
        {
            _cagedShocker.Move(ref _targetVelocity, 0);
            _cagedShocker.CheckGrounded(out bool leftFoot, out bool rightFoot);

            _cagedShocker.CustomGravity(ref _targetVelocity);
            _cagedShocker.ApplyVelocity(_targetVelocity);
        }

        public void OnEnter()
        {
            _targetVelocity = _rigidbody.velocity.With(x: _rigidbody.velocity.x * 0.2f);
            _timer = _cagedShocker.IdleTime;
        }

        public void OnExit() => _timer = 0f;
    }
}