using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Enemies.CagedShocker.States
{
    public class RestState : IState
    {
        public override string ToString() => "Rest";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;

        private readonly CagedShocker _cagedShocker;
        private readonly float _restTime;
        private float _timer;

        public RestState(CagedShocker cagedShocker, float restTime)
        {
            _cagedShocker = cagedShocker;
            _restTime = restTime;
        }

        public void Tick() => _timer -= Time.deltaTime;
        public void FixedTick() => _cagedShocker.Move(0);
        public void OnEnter() => _timer = _restTime;
        public void OnExit() => _timer = 0f;
    }
}