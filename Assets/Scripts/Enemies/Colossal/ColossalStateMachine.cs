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
            var idle = new ColossalIdle(2f);
            // var chase = new ChaseState(1f);

            stateMachine.SetState(idle);
        }
    }
}