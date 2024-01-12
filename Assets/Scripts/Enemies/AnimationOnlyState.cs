using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    public class AnimationOnlyState : IState
    {
        public override string ToString() => "AnimationStateOnly";
        public AnimationState AnimationState { get; }
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;

        private readonly float _duration;

        private float _timer;

        public AnimationOnlyState(float duration, AnimationState animationState)
        {
            _duration = duration;
            AnimationState = animationState;
        }

        public void Tick() => _timer -= Time.deltaTime;

        public void FixedTick()
        {
        }

        public void OnEnter() => _timer = _duration;
        public void OnExit() => _timer = _duration;
    }
}