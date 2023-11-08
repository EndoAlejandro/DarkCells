using System;
using DarkHavoc.CustomUtils;
using DarkHavoc.ImpulseComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.States
{
    public class ParryState : IState
    {
        public AnimationState Animation => AnimationState.Parry;
        public override string ToString() => "Parry";
        public bool CanTransitionToSelf => true;
        public bool Ended => _timer <= 0f;
        public bool ParryAvailable { get; private set; }

        private readonly Player _player;
        private readonly ImpulseAction _parryAction;

        private float _timer;

        public ParryState(Player player, ImpulseAction parryAction)
        {
            _player = player;
            _parryAction = parryAction;
        }

        public void Tick() => _timer -= Time.deltaTime;

        public void FixedTick()
        {
        }

        public void OnEnter()
        {
            ParryAvailable = false;
            _timer = _parryAction.Time;
            // _player.AddImpulse(_parryAction);

            _player.TryToBlockDamage += PlayerOnTryToBlockDamage;
        }

        private bool PlayerOnTryToBlockDamage(Vector2 damageSource)
        {
            float difference = damageSource.x - _player.transform.position.x;
            bool result = (difference < 0 && _player.FacingLeft) || (difference > 0 && !_player.FacingLeft);

            // if (result) ParryAvailable = true;
            
            return result;
        }

        public void OnExit()
        {
            ParryAvailable = false;
            _player.TryToBlockDamage -= PlayerOnTryToBlockDamage;
        }
    }
}