using System;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Enemies.SharedStates
{
    public class AnimationOnlyState : IState
    {
        public override string ToString() => "AnimationStateOnly";
        public AnimationState AnimationState { get; }
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;

        private readonly float _duration;

        private float _timer;
        private readonly Action _onExitCallback;
        private readonly Action _onEnterCallback;

        public AnimationOnlyState(float duration, AnimationState animationState, Action onExitCallback = null,
            Action onEnterCallback = null)
        {
            _duration = duration;
            AnimationState = animationState;
            _onExitCallback = onExitCallback;
            _onEnterCallback = onEnterCallback;
        }

        public void Tick() => _timer -= Time.deltaTime;

        public void FixedTick()
        {
        }

        public void OnEnter()
        {
            _timer = _duration;
            _onEnterCallback?.Invoke();
        }

        public void OnExit()
        {
            _timer = _duration;
            _onExitCallback?.Invoke();
        }
    }
}