using StateMachineComponents;
using UnityEngine;

namespace Enemies
{
    public class ChaseState : IState
    {
        private readonly CagedShocker _cagedShocker;
        private readonly Rigidbody2D _rigidbody;
        public bool CanTransitionToSelf => false;

        public ChaseState(CagedShocker cagedShocker, Rigidbody2D rigidbody)
        {
            _cagedShocker = cagedShocker;
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
        }

        public void OnExit()
        {
        }
    }
}