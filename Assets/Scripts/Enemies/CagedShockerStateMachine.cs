using Enemies.CagedShockerStates;
using StateMachineComponents;
using UnityEngine;

namespace Enemies
{
    public class CagedShockerStateMachine : FiniteStateBehaviour
    {
        private CagedShocker _cagedShocker;
        private Rigidbody2D _rigidbody;

        protected override void References()
        {
            _cagedShocker = GetComponent<CagedShocker>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        protected override void StateMachine()
        {
            var idle = new IdleState(_cagedShocker, _rigidbody);
            var patrol = new PatrolState(_cagedShocker, _rigidbody);
            var chase = new ChaseState(_cagedShocker, _rigidbody);

            // Initial State.
            stateMachine.SetState(idle);

            stateMachine.AddTransition(idle, patrol, () => idle.Ended && _cagedShocker.Grounded);
            stateMachine.AddTransition(patrol, idle, () => patrol.Ended || !_cagedShocker.Grounded);
        }
    }
}