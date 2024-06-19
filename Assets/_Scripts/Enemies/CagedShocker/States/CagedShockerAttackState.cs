using System;
using UnityEngine;
using DarkHavoc.StateMachineComponents;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Enemies.CagedShocker.States
{
    [Obsolete("Not longer supported state, use EnemyAttackState instead", true)]
    public class CagedShockerAttackState : IState
    {
        public override string ToString() => "Attack";
        public AnimationState AnimationState => AnimationState.LightAttack;

        private readonly CagedShocker _cagedShocker;
        private readonly EnemyHitBox _hitBox;
        private readonly EnemyAnimation _animation;
        private readonly bool _canCombo;
        private readonly float _attackDuration;

        private bool _attackInterruptionAvailable;
        private bool _stunned;
        private float _timer;
        private float _comboTimer;

        public bool CanTransitionToSelf => _canCombo;
        public bool Ended => _timer <= 0f;
        public bool CanCombo => _canCombo && _comboTimer <= 0f;
        public bool Stunned => _stunned && _attackInterruptionAvailable;

        public CagedShockerAttackState(CagedShocker cagedShocker, EnemyHitBox hitBox, EnemyAnimation animation,
            bool canCombo, float attackDuration)
        {
            _cagedShocker = cagedShocker;
            _hitBox = hitBox;
            _animation = animation;
            _canCombo = canCombo;

            _attackDuration = attackDuration;
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;
            _comboTimer -= Time.deltaTime;
        }

        public void FixedTick()
        {
        }

        public void OnEnter()
        {
            _stunned = false;
            _attackInterruptionAvailable = false;

            _timer = _attackDuration;
            _comboTimer = _cagedShocker.Stats.ComboTime;

            _cagedShocker.OnStunned += CagedShockerOnStunned;
            _animation.OnAttackPerformed += AnimationOnAttackPerformed;
            _animation.OnAttackInterruptionAvailable += AnimationOnAttackInterruptionAvailable;
        }

        private void AnimationOnAttackInterruptionAvailable() => _attackInterruptionAvailable = true;
        private void CagedShockerOnStunned() => _stunned = true;
        private void AnimationOnAttackPerformed() => _hitBox.TryToAttack();

        public void OnExit()
        {
            _cagedShocker.OnStunned -= CagedShockerOnStunned;
            _animation.OnAttackPerformed -= AnimationOnAttackPerformed;
            _animation.OnAttackInterruptionAvailable -= AnimationOnAttackInterruptionAvailable;
        }
    }
}