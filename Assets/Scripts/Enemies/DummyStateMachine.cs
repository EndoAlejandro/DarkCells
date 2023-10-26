using DarkHavoc.AttackComponents;
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

    public class EnemyIdle : IState
    {
        private readonly Dummy _dummy;
        public EnemyIdle(Dummy dummy)
        {
            _dummy = dummy;
            _dummy.OnTakeDamage += DummyOnTakeDamage;
        }

        private void DummyOnTakeDamage(IDoDamage damageDealer)
        {
            
        }

        public AnimationState Animation  => AnimationState.Ground;
        public bool CanTransitionToSelf => false;

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
            throw new System.NotImplementedException();
        }
    }
}