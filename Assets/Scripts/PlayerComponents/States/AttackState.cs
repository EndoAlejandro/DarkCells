﻿using DarkHavoc.CustomUtils;
using DarkHavoc.ImpulseComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.States
{
    public class AttackState : IState
    {
        public override string ToString() => "Attack";
        public AnimationState Animation { get; }

        private readonly Player _player;
        private readonly InputReader _input;
        private readonly PlayerAnimation _animation;
        private readonly AttackImpulseAction _attackAction;

        private float _timer;

        public bool Ended => _timer <= 0f;

        public bool CanCombo { get; private set; }

        public bool CanTransitionToSelf => true;

        public AttackState(Player player, Rigidbody2D rigidbody, InputReader input, PlayerAnimation animation,
            AnimationState animationState, AttackImpulseAction attackAction)
        {
            _player = player;
            _input = input;
            _animation = animation;
            _attackAction = attackAction;
            Animation = animationState;
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;

            if (_player.HasBufferedJump)
            {
                _timer = 0f;
                _player.Jump();
                _player.ApplyVelocity();
            }
        }

        public void FixedTick()
        {
            var reduction = _player.Grounded ? _player.Stats.MovementReduction : 1;
            _player.Move(_input.Movement.x * reduction);
        }

        public void OnEnter()
        {
            _animation.OnAttackPerformed += AnimationOnAttackPerformed;
            _animation.OnComboAvailable += AnimationOnComboAvailable;

            _timer = _attackAction.Time;
            if (_player.Grounded)
                _player.AddImpulse(_attackAction);
        }

        private void AnimationOnAttackPerformed() => _player.Attack(_attackAction);
        private void AnimationOnComboAvailable() => CanCombo = true;

        public void OnExit()
        {
            CanCombo = false;
            _animation.OnAttackPerformed -= AnimationOnAttackPerformed;
            _animation.OnComboAvailable -= AnimationOnComboAvailable;
        }
    }
}