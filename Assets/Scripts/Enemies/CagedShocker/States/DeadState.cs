using DarkHavoc.PlayerComponents;
using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Enemies.CagedShocker.States
{
    public class DeadState : IState
    {
        public override string ToString() => "Dead";
        public AnimationState AnimationState => AnimationState.Death;
        public bool CanTransitionToSelf => false;
        private readonly CagedShocker _cagedShocker;

        public DeadState(CagedShocker cagedShocker) => _cagedShocker = cagedShocker;

        public void Tick()
        {
        }

        public void FixedTick() => _cagedShocker.Move(0);
        public void OnEnter() => _cagedShocker.Death();
        public void OnExit()
        {
        }
    }
}