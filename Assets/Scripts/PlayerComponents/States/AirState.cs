﻿using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.States
{
    public class AirState : IState
    {
        public override string ToString() => "Air";
        public AnimationState Animation  => AnimationState.Air;

        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody;
        private readonly InputReader _input;

        private Vector2 _targetVelocity;

        public bool CanTransitionToSelf => false;

        public AirState(Player player, Rigidbody2D rigidbody, InputReader input)
        {
            _player = player;
            _rigidbody = rigidbody;
            _input = input;
        }


        public void Tick()
        {
            _player.Move(ref _targetVelocity, _input.Movement.x);
            
            if (_player.HasBufferedJump) _player.Jump(ref _targetVelocity);
        }

        public void FixedTick()
        {
            _player.CheckCollisions(ref _targetVelocity);
            _player.CustomGravity(ref _targetVelocity);

            _player.ApplyVelocity(_targetVelocity);
        }

        public void OnEnter() => _targetVelocity = _rigidbody.velocity;

        public void OnExit()
        {
        }
    }
}