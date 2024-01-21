using DarkHavoc.PlayerComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.AnimationState;

namespace DarkHavoc.Enemies.CagedShocker.States
{
    public class AttackState : IState
    {
        public override string ToString() => "Attack";
        public AnimationState AnimationState => AnimationState.LightAttack;

        private readonly CagedShocker _cagedShocker;
        private readonly EnemyHitBox _hitBox;
        private readonly CagedShockerAnimation _animation;
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

        public AttackState(CagedShocker cagedShocker, EnemyHitBox hitBox, CagedShockerAnimation animation,
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
        private void AnimationOnAttackPerformed() => _hitBox.Attack();

        public void OnExit()
        {
            _cagedShocker.OnStunned -= CagedShockerOnStunned;
            _animation.OnAttackPerformed -= AnimationOnAttackPerformed;
            _animation.OnAttackInterruptionAvailable -= AnimationOnAttackInterruptionAvailable;
        }
    }
}