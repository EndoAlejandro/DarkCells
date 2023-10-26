using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.PlayerComponents.AnimationState;

namespace DarkHavoc.Enemies.CagedShockerStates
{
    public class RestState : IState
    {
        public override string ToString() => "Rest";
        public AnimationState Animation => AnimationState.Ground;

        private readonly float _restTime;
        private float _timer;

        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;

        public RestState(float restTime) => _restTime = restTime;
        public void Tick() => _timer -= Time.deltaTime;

        public void FixedTick()
        {
        }

        public void OnEnter() => _timer = _restTime;
        public void OnExit() => _timer = 0f;
    }
}