using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.States
{
    public class SitState : IState
    {
        public override string ToString() => _sitDown ? " SitDown" : "SitUp";
        public AnimationState Animation => _sitDown ? AnimationState.SitDown : AnimationState.SitUp;
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;
        
        private readonly bool _sitDown;
        private float _timer;

        public SitState(bool sitDown) => _sitDown = sitDown;

        public void Tick() => _timer -= Time.deltaTime;

        public void FixedTick()
        {
        }

        public void OnEnter() => _timer = 1f;

        public void OnExit()
        {
        }
    }
}