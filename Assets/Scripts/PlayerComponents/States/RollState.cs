﻿using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.States
{
    public class RollState : IState
    {
        public override string ToString() => "Roll";
        public AnimationState Animation  => AnimationState.Roll;

        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody;
        private readonly InputReader _input;

        private Vector2 _targetVelocity;
        private float _timer;

        public bool Ended { get; private set; }
        public bool CanTransitionToSelf => false;

        public RollState(Player player, Rigidbody2D rigidbody, InputReader input)
        {
            _player = player;
            _rigidbody = rigidbody;
            _input = input;
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;

            switch (_input.Movement.x)
            {
                case > 0 when _player.FacingLeft:
                case < 0 when !_player.FacingLeft:
                    _targetVelocity.x = _rigidbody.velocity.x;
                    Ended = true;
                    break;
            }

            if (_player.HasBufferedJump)
            {
                _player.Jump(ref _targetVelocity);
                Ended = true;
            }

            if (_timer <= 0f) Ended = true;
        }

        public void FixedTick()
        {
            _targetVelocity = _rigidbody.velocity;
            _player.CheckCollisions(ref _targetVelocity);

            _player.Roll(ref _targetVelocity);
            _player.CustomGravity(ref _targetVelocity);

            _player.ApplyVelocity(_targetVelocity);
        }

        public void OnEnter()
        {
            Ended = false;
            _targetVelocity = _rigidbody.velocity;
            _timer = _player.Stats.RollTime;
            _player.SetPlayerCollider(false);
        }

        public void OnExit()
        {
            _targetVelocity.x *= _player.Stats.RollSpeedConservation;
            _player.ApplyVelocity(_targetVelocity);
            Ended = false;
            _player.SetPlayerCollider(true);
        }
    }
}