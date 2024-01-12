using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Enemies
{
    public class EnemyIdle : IState
    {
        private readonly Dummy _dummy;
        public EnemyIdle(Dummy dummy)
        {
            _dummy = dummy;
        }

        private void DummyOnTakeDamage(IDoDamage damageDealer)
        {
            
        }

        public AnimationState AnimationState => AnimationState.Ground;
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