using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Enemies.Colossal.States
{
    public class BossDeathState : IState
    {
        public override string ToString() => "Dead";
        public AnimationState AnimationState => AnimationState.Death;
        public bool CanTransitionToSelf => false;
        
        private readonly Colossal _colossal;

        public BossDeathState(Colossal colossal) => _colossal = colossal;

        public void Tick()
        {
        }

        public void FixedTick() => _colossal.Move(0);
        public void OnEnter() => _colossal.Death();
        public void OnExit()
        {
        }
    }
}