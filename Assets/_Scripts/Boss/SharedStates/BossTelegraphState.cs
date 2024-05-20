using DarkHavoc.Fx;
using DarkHavoc.ServiceLocatorComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Boss.SharedStates
{
    public class BossTelegraphState : IState
    {
        public override string ToString() => "Telegraph";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;

        private readonly Boss _boss;
        private readonly FxType _fxType;
        private readonly float _duration;

        private float _timer;

        public BossTelegraphState(Boss boss, FxType fxType, float duration)
        {
            _boss = boss;
            _fxType = fxType;
            _duration = duration;
        }

        public void Tick() => _timer -= Time.deltaTime;

        public void FixedTick()
        {
        }

        public void OnEnter()
        {
            _timer = _duration;
            ServiceLocator.GetService<FxManager>()?.PlayFx(_fxType, _boss.transform.position);
        }

        public void OnExit() => _timer = _duration;
    }
}