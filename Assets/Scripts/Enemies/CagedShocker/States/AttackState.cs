using DarkHavoc.PlayerComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.PlayerComponents.AnimationState;

namespace DarkHavoc.Enemies.CagedShocker.States
{
    public class AttackState : IState
    {
        public override string ToString() => "Attack";
        public AnimationState Animation => AnimationState.LightAttack;

        private readonly Enemies.CagedShocker.CagedShocker _cagedShocker;
        private readonly EnemyAttack _attack;
        private readonly CagedShockerAnimation _animation;
        private readonly bool _canCombo;
        private readonly float _attackDuration;

        private Vector2 _targetVelocity;
        private bool _attackInterruptionAvailable;
        private bool _stunned;
        private float _timer;
        private float _comboTimer;
        private Player _player;

        public bool CanTransitionToSelf => _canCombo;
        public bool Ended => _timer <= 0f;
        public bool CanCombo => _canCombo && _comboTimer <= 0f;
        public bool TargetOnRange { get; private set; }
        public bool Stunned => _stunned && _attackInterruptionAvailable;

        public AttackState(Enemies.CagedShocker.CagedShocker cagedShocker, EnemyAttack attack, CagedShockerAnimation animation,
            bool canCombo,
            float attackDuration)
        {
            _cagedShocker = cagedShocker;
            _attack = attack;
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
            _cagedShocker.CheckGrounded(out bool leftFoot, out bool rightFoot);
            _cagedShocker.CustomGravity(ref _targetVelocity);
            _cagedShocker.ApplyVelocity(_targetVelocity);
        }

        public void OnEnter()
        {
            // TODO: Perform telegraph.
            _stunned = false;
            _attackInterruptionAvailable = false;

            _timer = _attackDuration;
            _comboTimer = _cagedShocker.Stats.ComboTime;
            _player = _cagedShocker.Player;

            _cagedShocker.OnStunned += CagedShockerOnStunned;
            _animation.OnAttackPerformed += AnimationOnAttackPerformed;
            _animation.OnAttackInterruptionAvailable += AnimationOnAttackInterruptionAvailable;
        }

        private void AnimationOnAttackInterruptionAvailable() => _attackInterruptionAvailable = true;
        private void CagedShockerOnStunned() => _stunned = true;
        private void AnimationOnAttackPerformed() => _attack.Attack();

        public void OnExit()
        {
            _player = null;
            _cagedShocker.OnStunned -= CagedShockerOnStunned;
            _animation.OnAttackPerformed -= AnimationOnAttackPerformed;
            _animation.OnAttackInterruptionAvailable -= AnimationOnAttackInterruptionAvailable;
        }
    }
}