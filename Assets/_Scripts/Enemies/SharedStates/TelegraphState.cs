using DarkHavoc.Fx;
using DarkHavoc.ServiceLocatorComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Enemies.SharedStates
{
    public class TelegraphState : IState
    {
        public override string ToString() => "Telegraph";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;

        private readonly Enemy _enemy;
        private readonly EnemyHitBox _hitbox;
        private readonly float _heightOffset;

        private FxManager _fxManager;
        private float _timer;

        public TelegraphState(Enemy enemy, EnemyHitBox hitbox, float heightOffset)
        {
            _enemy = enemy;
            _heightOffset = heightOffset;
            _hitbox = hitbox;
        }

        public void Tick() => _timer -= Time.deltaTime;

        public void FixedTick() => _enemy.Move(0);

        public void OnEnter()
        {
            _fxManager ??= ServiceLocator.GetService<FxManager>();
            _fxManager.PlayFx(_hitbox.IsUnstoppable ? EnemyFx.DangerousTelegraph : EnemyFx.Telegraph,
                _enemy.transform.position + Vector3.up * _heightOffset);
            _timer = _hitbox.TelegraphTime;
        }

        public void OnExit() => _timer = 0f;
    }
}