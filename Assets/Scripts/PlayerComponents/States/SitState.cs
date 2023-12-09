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

        private readonly Player _player;
        private readonly bool _sitDown;
        private float _timer;

        public SitState(Player player,bool sitDown)
        {
            _player = player;
            _sitDown = sitDown;
        }

        public void Tick() => _timer -= Time.deltaTime;
        public void FixedTick() => _player.ResetVelocity();
        public void OnEnter() => _timer = 0.25f;
        public void OnExit() => _player.ResetVelocity();
    }
}