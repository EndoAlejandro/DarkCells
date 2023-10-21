using StateMachineComponents;
using UnityEngine;

namespace Enemies.CagedShockerStates
{
    public class IdleState : IState
    {
        private readonly CagedShocker _cagedShocker;
        private readonly Rigidbody2D _rigidbody;
        public bool CanTransitionToSelf => false;

        private Vector2 _targetVelocity;

        public IdleState(CagedShocker cagedShocker, Rigidbody2D rigidbody)
        {
            _cagedShocker = cagedShocker;
            _rigidbody = rigidbody;
        }

        public void Tick()
        {
        }

        public void FixedTick()
        {
            _cagedShocker.CheckGrounded(out bool leftFoot, out bool rightFoot);

            _cagedShocker.CustomGravity(ref _targetVelocity);
            _cagedShocker.ApplyVelocity(_targetVelocity);
        }

        public void OnEnter()
        {
            _targetVelocity = Vector2.zero;
        }

        public void OnExit()
        {
        }
    }
}