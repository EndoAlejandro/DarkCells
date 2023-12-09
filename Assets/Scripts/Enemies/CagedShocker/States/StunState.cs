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

        private readonly CagedShocker _cagedShocker;
        private readonly float _stunTime;
        private float _timer;

        public StunState(CagedShocker cagedShocker, float stunTime)
        {
            _cagedShocker = cagedShocker;
            _stunTime = stunTime;
        }

        public void Tick() => _timer -= Time.deltaTime;
        public void FixedTick() => _cagedShocker.Move(0);
        public void OnEnter() => _timer = _stunTime;
        public void OnExit() => _timer = 0f;
    }
}