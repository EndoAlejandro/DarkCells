using DarkHavoc.PlayerComponents;
using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Enemies.Colossal
{
    public class ColossalIdle : IState
    {
        private readonly float _idleTime;
        public override string ToString() => "Idle";
        public AnimationState Animation => AnimationState.Ground;
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;

        private float _timer;

        public ColossalIdle(float idleTime) => _idleTime = idleTime;

        public void Tick() => _timer--;

        public void FixedTick()
        {
        }

        public void OnEnter() => _timer = _idleTime;

        public void OnExit()
        {
        }
    }
}