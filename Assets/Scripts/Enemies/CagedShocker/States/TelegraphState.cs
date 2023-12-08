﻿using DarkHavoc.Fx;
using DarkHavoc.ServiceLocatorComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.PlayerComponents.AnimationState;

namespace DarkHavoc.Enemies.CagedShocker.States
{
    public class TelegraphState : IState
    {
        public override string ToString() => "Telegraph";
        public AnimationState Animation => AnimationState.Ground;
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;

        private readonly Enemies.CagedShocker.CagedShocker _cagedShocker;
        private readonly float _telegraphTime;
        private readonly FxProvider _fxProvider;
        
        private float _timer;
        private bool _telegraphed;
        private Vector2 _targetVelocity;

        public TelegraphState(Enemies.CagedShocker.CagedShocker cagedShocker, float telegraphTime)
        {
            _cagedShocker = cagedShocker;
            _telegraphTime = telegraphTime;

            _fxProvider = ServiceLocator.Instance.GetService<FxProvider>();
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;
            if (!_telegraphed && _timer <= _telegraphTime/2)
            {
                _telegraphed = true;
                _fxProvider.GetFx(FxType.Telegraph, _cagedShocker.transform.position + Vector3.up * 1.25f);
            }

            _cagedShocker.Move(ref _targetVelocity, 0);
        }

        public void FixedTick()
        {
            _cagedShocker.CheckGrounded(out bool _, out bool _);
            _cagedShocker.CustomGravity(ref _targetVelocity);
            _cagedShocker.ApplyVelocity(_targetVelocity);
        }

        public void OnEnter()
        {
            _telegraphed = false;
            _timer = _telegraphTime;
            _cagedShocker.TelegraphAttack(_telegraphTime);
        }

        public void OnExit() => _timer = 0f;
    }
}