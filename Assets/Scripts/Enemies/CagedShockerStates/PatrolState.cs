﻿using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.PlayerComponents.AnimationState;

namespace DarkHavoc.Enemies.CagedShockerStates
{
    public class PatrolState : IState
    {
        public override string ToString() => "Patrol";
        public AnimationState Animation  => AnimationState.Ground;

        private readonly CagedShocker _cagedShocker;
        private readonly Rigidbody2D _rigidbody;

        private Vector2 _targetVelocity;
        private int _direction;

        private bool _facingWall;
        private bool _rightFoot;
        private bool _leftFoot;

        public bool CanTransitionToSelf => false;
        public bool Ended { get; private set; }

        public PatrolState(CagedShocker cagedShocker, Rigidbody2D rigidbody)
        {
            _cagedShocker = cagedShocker;
            _rigidbody = rigidbody;
        }

        public void Tick()
        {
            _cagedShocker.SeekPlayer();

            _direction = _cagedShocker.FacingLeft ? -1 : 1;

            if (!_leftFoot && _rightFoot) Ended = _cagedShocker.FacingLeft;
            else if (_leftFoot && !_rightFoot) Ended = !_cagedShocker.FacingLeft;

            if (_facingWall) Ended = true;
        }

        public void FixedTick()
        {
            _cagedShocker.CheckWallCollisions(out _facingWall);
            _cagedShocker.CheckGrounded(out _leftFoot, out _rightFoot);
            _cagedShocker.Move(ref _targetVelocity, _direction);

            _cagedShocker.CustomGravity(ref _targetVelocity);
            _cagedShocker.ApplyVelocity(_targetVelocity);
        }

        public void OnEnter()
        {
            _cagedShocker.CheckWallCollisions(out _facingWall);
            _cagedShocker.CheckGrounded(out _leftFoot, out _rightFoot);

            if (_facingWall) _cagedShocker.SetFacingLeft(!_cagedShocker.FacingLeft);
            else if (!_leftFoot && _rightFoot) _cagedShocker.SetFacingLeft(false);
            else if (_leftFoot && !_rightFoot) _cagedShocker.SetFacingLeft(true);

            _facingWall = false;

            Ended = false;
            _targetVelocity = Vector2.zero;
        }

        public void OnExit() => Ended = false;
    }
}