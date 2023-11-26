using UnityEngine;

namespace DarkHavoc.StateMachineComponents
{
    [RequireComponent(typeof(Rigidbody))]
    public class StateMachineExample : FiniteStateBehaviour
    {
        private Rigidbody _rigidbody;

        // This rigidbody is not required is just for show where should get the reference.
        protected override void References() => _rigidbody = GetComponent<Rigidbody>();

        protected override void StateMachine()
        {
            var stateA = new SampleState(5f);
            var stateB = new SampleState(10f);

            // Always set manually the initial state.
            stateMachine.SetState(stateA);

            // transitions here.
            stateMachine.AddTransition(stateA, stateB, () => stateA.TransitionEnded);
            stateMachine.AddTransition(stateB, stateA, () => stateB.TransitionEnded);
        }
    }
}