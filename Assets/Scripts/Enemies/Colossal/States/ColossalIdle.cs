using DarkHavoc.PlayerComponents;
using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Enemies.Colossal
{
    public class ColossalIdle : IState
    {
        private readonly Colossal _colossal;
        private readonly float _duration;
        public override string ToString() => "Idle";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;

        private float _timer;

        public ColossalIdle(Colossal colossal,float duration)
        {
            _colossal = colossal;
            _duration = duration;
        }

        public void Tick() => _timer--;

        public void FixedTick()
        {
        }

        public void OnEnter()
        {
            _colossal.Setup();
            _timer = _duration;
        }

        public void OnExit()
        {
        }
    }
}