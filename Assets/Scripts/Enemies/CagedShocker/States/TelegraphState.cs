using DarkHavoc.Fx;
using DarkHavoc.ServiceLocatorComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.CagedShocker.States
{
    public class TelegraphState : IState
    {
        public override string ToString() => "Telegraph";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;

        private readonly Enemy _enemy;
        private readonly float _telegraphTime;
        private readonly float _heightOffset;

        private FxManager _fxManager;
        private float _timer;
        private bool _telegraphed;

        public TelegraphState(Enemy enemy, float heightOffset)
        {
            _enemy = enemy;
            _heightOffset = heightOffset;
            _telegraphTime = _enemy.Stats.TelegraphTime;
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;
            if (!_telegraphed && _timer <= _telegraphTime / 2)
            {
                _telegraphed = true;
                _fxManager.GetFx(FxType.Telegraph, _enemy.transform.position + Vector3.up * _heightOffset);
            }
        }

        public void FixedTick() => _enemy.Move(0);

        public void OnEnter()
        {
            _fxManager ??= ServiceLocator.GetService<FxManager>();
            _telegraphed = false;
            _timer = _telegraphTime;
        }

        public void OnExit() => _timer = 0f;
    }
}