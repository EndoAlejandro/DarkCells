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
        private readonly bool _isUnstoppable;

        private FxManager _fxManager;
        private float _timer;

        public TelegraphState(Enemy enemy, float heightOffset, bool isUnstoppable = false)
        {
            _enemy = enemy;
            _heightOffset = heightOffset;
            _isUnstoppable = isUnstoppable;

            _telegraphTime = _enemy.Stats.TelegraphTime;
        }

        public void Tick() => _timer -= Time.deltaTime;

        public void FixedTick() => _enemy.Move(0);

        public void OnEnter()
        {
            _fxManager ??= ServiceLocator.GetService<FxManager>();
            _fxManager.GetFx(_isUnstoppable ? FxType.DangerousTelegraph : FxType.Telegraph,
                _enemy.transform.position + Vector3.up * _heightOffset);
            _timer = _telegraphTime;
        }

        public void OnExit() => _timer = 0f;
    }
}