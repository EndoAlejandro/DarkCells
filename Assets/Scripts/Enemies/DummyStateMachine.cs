using DarkHavoc.PlayerComponents;
using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Enemies
{
    public class DummyStateMachine : FiniteStateBehaviour
    {
        private Dummy _dummy;

        protected override void References()
        {
            _dummy = GetComponent<Dummy>();
        }

        protected override void StateMachine()
        {
            var idle = new EnemyIdle(_dummy);
        }
    }
}