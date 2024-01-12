namespace DarkHavoc.StateMachineComponents
{
    public class BlankState : IState
    {
        public override string ToString() => "Idle";

        public AnimationState AnimationState  => AnimationState.Ground;
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
        }
    }
}