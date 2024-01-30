using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Enemies.CagedShocker.States
{
    public class EnemyDeathState : IState
    {
        public override string ToString() => "Dead";
        public AnimationState AnimationState => AnimationState.Death;
        public bool CanTransitionToSelf => false;
        
        private readonly Enemy _enemy;

        public EnemyDeathState(Enemy enemy) => _enemy = enemy;

        public void Tick()
        {
        }

        public void FixedTick() => _enemy.Move(0);
        public void OnEnter() => _enemy.Death();
        public void OnExit()
        {
        }
    }
}