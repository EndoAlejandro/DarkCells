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

        private readonly CagedShocker _cagedShocker;
        private readonly float _telegraphTime;

        private FxProvider _fxProvider;
        private float _timer;
        private bool _telegraphed;

        public TelegraphState(CagedShocker cagedShocker, float telegraphTime)
        {
            _cagedShocker = cagedShocker;
            _telegraphTime = telegraphTime;
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;
            if (!_telegraphed && _timer <= _telegraphTime / 2)
            {
                _telegraphed = true;
                _fxProvider.GetFx(FxType.Telegraph, _cagedShocker.transform.position + Vector3.up * 1.25f);
            }

        }

        public void FixedTick() => _cagedShocker.Move(0);

        public void OnEnter()
        {
            _fxProvider ??= ServiceLocator.GetService<FxProvider>();
            
            _telegraphed = false;
            _timer = _telegraphTime;
            
            _cagedShocker.TelegraphAttack(_telegraphTime);
        }

        public void OnExit() => _timer = 0f;
    }
}