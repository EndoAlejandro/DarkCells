using UnityEngine;
using AnimationState = DarkHavoc.PlayerComponents.AnimationState;

namespace DarkHavoc.StateMachineComponents
{
    public class SampleState : IState
    {
        public AnimationState Animation => AnimationState.Ground;
        private readonly float _transitionTime;
        private float _timer;

        public bool TransitionEnded => _timer <= 0f;
        public bool CanTransitionToSelf => false;

        public SampleState(float transitionTime) => _transitionTime = transitionTime;

        public void Tick() => _timer -= Time.deltaTime;

        public void FixedTick()
        {
        }

        public void OnEnter() => _timer = _transitionTime;

        public void OnExit() => _timer = 0f;
    }
}