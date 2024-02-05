using DarkHavoc.PlayerComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.AnimationState;

namespace DarkHavoc.Enemies.CagedShocker.States
{
    public class StunState : IState
    {
        public override string ToString() => "Stun";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;

        private readonly Enemy _enemy;
        private readonly float _stunTime;
        private float _timer;

        public StunState(Enemy enemy, float stunTime)
        {
            _enemy = enemy;
            _stunTime = stunTime;
        }

        public void Tick() => _timer -= Time.deltaTime;
        public void FixedTick() => _enemy.Move(0);
        public void OnEnter()
        {
            _timer = _stunTime;
            // TODO: Stun feedback.
        }

        public void OnExit() => _timer = 0f;
    }
}