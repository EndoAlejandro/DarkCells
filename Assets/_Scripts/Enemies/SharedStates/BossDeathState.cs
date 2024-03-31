using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Enemies.SharedStates
{
    public class BossDeathState : IState
    {
        public override string ToString() => "Dead";
        public AnimationState AnimationState => AnimationState.Death;
        public bool CanTransitionToSelf => false;
        
        private readonly Boss _boss;

        public BossDeathState(Boss boss) => _boss = boss;

        public void Tick()
        {
        }

        public void FixedTick() => _boss.Move(0);
        public void OnEnter() => _boss.Death();
        public void OnExit()
        {
        }
    }
}