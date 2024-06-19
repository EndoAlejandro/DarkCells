using DarkHavoc.Enemies;
using DarkHavoc.Fx;
using DarkHavoc.ServiceLocatorComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Boss.HeartHoarder.States
{
    public class HeartHoarderTelegraphState : IState
    {
        public override string ToString() => "Telegraph";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;

        private readonly HeartHoarder _heartHoarder;
        private readonly EnemyHitBox _hitBox;
        private readonly float _offset;
        private float _timer;

        public HeartHoarderTelegraphState(HeartHoarder heartHoarder, EnemyHitBox hitBox, float offset)
        {
            _heartHoarder = heartHoarder;
            _hitBox = hitBox;
            _offset = offset;
        }

        public void Tick() => _timer -= Time.deltaTime;

        public void FixedTick()
        {
        }

        public void OnEnter()
        {
            _timer = _heartHoarder.Stats.TelegraphTime;
            BossFx fxType = _hitBox.IsUnstoppable ? BossFx.DangerousTelegraph : BossFx.Telegraph;
            ServiceLocator.GetService<FxManager>()?
                .PlayFx(fxType, _heartHoarder.transform.position + Vector3.up * _offset, 1.25f);
        }

        public void OnExit()
        {
        }
    }
}