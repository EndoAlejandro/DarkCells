using DarkHavoc.PlayerComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.AnimationState;

namespace DarkHavoc.Enemies.CagedShocker.States
{
    public class IdleState : IState
    {
        public override string ToString() => "Grounded";
        public AnimationState AnimationState => AnimationState.Ground;

        private readonly CagedShocker _cagedShocker;
        private float _timer;

        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;

        public IdleState(CagedShocker cagedShocker) => _cagedShocker = cagedShocker;

        public void Tick()
        {
            _timer -= Time.deltaTime;
            _cagedShocker.SeekPlayer();
        }

        public void FixedTick() => _cagedShocker.Move(0);
        public void OnEnter() => _timer = _cagedShocker.IdleTime;
        public void OnExit() => _timer = 0f;
    }
}