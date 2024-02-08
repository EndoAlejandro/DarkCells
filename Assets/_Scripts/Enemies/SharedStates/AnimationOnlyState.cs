using System;
using DarkHavoc.ServiceLocatorComponents;
using DarkHavoc.StateMachineComponents;
using DarkHavoc.UI;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

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
        private readonly Action _callback;

        public AnimationOnlyState(float duration, AnimationState animationState, Action callback = null)
        {
            _duration = duration;
            AnimationState = animationState;
            _callback = callback;
        }

        public void Tick() => _timer -= Time.deltaTime;

        public void FixedTick()
        {
        }

        public void OnEnter() => _timer = _duration;

        public void OnExit()
        {
            _timer = _duration;
            _callback?.Invoke();
        }
    }
}