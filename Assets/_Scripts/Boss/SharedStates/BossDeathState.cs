using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Boss.SharedStates
{
    public class BossDeathState : IState
    {
        public override string ToString() => "Dead";
        public AnimationState AnimationState => AnimationState.Death;
        public bool CanTransitionToSelf => false;
        
        private readonly DarkHavoc.Boss.Boss _boss;

        public BossDeathState(DarkHavoc.Boss.Boss boss) => _boss = boss;

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