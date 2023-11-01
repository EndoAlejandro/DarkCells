using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.PlayerComponents.AnimationState;

namespace DarkHavoc.Enemies.CagedShockerStates
{
    public class TelegraphState : IState
    {
        public AnimationState Animation => AnimationState.Ground;
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;

        private readonly CagedShocker _cagedShocker;
        private readonly float _telegraphTime;
        private float _timer;

        public TelegraphState(CagedShocker cagedShocker, float telegraphTime)
        {
            _cagedShocker = cagedShocker;
            _telegraphTime = telegraphTime;
        }

        public void Tick() => _timer -= Time.deltaTime;

        public void FixedTick()
        {
        }

        public void OnEnter()
        {
            _timer = _telegraphTime;
            _cagedShocker.TelegraphAttack(_telegraphTime);
        }

        public void OnExit() => _timer = 0f;
    }
}