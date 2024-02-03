using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.CagedShocker.States
{
    public class IdleState : IState
    {
        public override string ToString() => "Grounded";
        public AnimationState AnimationState => AnimationState.Ground;

        private readonly Enemy _enemy;
        private float _timer;

        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;

        public IdleState(Enemy enemy) => _enemy = enemy;

        public void Tick()
        {
            _timer -= Time.deltaTime;
            _enemy.SeekPlayer();
        }

        public void FixedTick() => _enemy.Move(0);
        public void OnEnter() => _timer = _enemy.Stats.IdleTime;
        public void OnExit() => _timer = 0f;
    }
}