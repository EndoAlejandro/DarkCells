using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.PlayerComponents.AnimationState;

namespace DarkHavoc.Enemies.CagedShocker.States
{
    public class StunState : IState
    {
        public override string ToString() => "Stun";
        public AnimationState Animation => AnimationState.Ground;
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;

        private readonly float _stunTime;
        private float _timer;

        public StunState(float stunTime) => _stunTime = stunTime;
        public void Tick() => _timer -= Time.deltaTime;

        public void FixedTick()
        {
        }

        public void OnEnter() => _timer = _stunTime;
        public void OnExit() => _timer = 0f;
    }
    
    public class RestState : IState
    {
        public override string ToString() => "Rest";
        public AnimationState Animation => AnimationState.Ground;
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;

        private readonly float _restTime;
        private float _timer;

        public RestState(float restTime) => _restTime = restTime;
        public void Tick() => _timer -= Time.deltaTime;

        public void FixedTick()
        {
        }

        public void OnEnter() => _timer = _restTime;
        public void OnExit() => _timer = 0f;
    }
}