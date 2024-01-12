using DarkHavoc.PlayerComponents;
using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Enemies.Colossal
{
    public class ColossalStateMachine : FiniteStateBehaviour
    {
        private Colossal _colossal;
        private Player _player;

        protected override void References()
        {
            _colossal = GetComponent<Colossal>();
        }

        protected override void StateMachine()
        {
            var initialDelay = new AnimationOnlyState(5f, AnimationState.None);
            var awake = new AnimationOnlyState(1.1f, AnimationState.Awake);
            var idle = new ColossalIdle(_colossal, 2f);

            stateMachine.SetState(initialDelay);

            stateMachine.AddTransition(initialDelay, awake, () => initialDelay.Ended);
            stateMachine.AddTransition(awake, idle, () => awake.Ended);
        }
    }
}