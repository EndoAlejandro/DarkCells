using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.PlayerComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.PlayerComponents.AnimationState;

namespace DarkHavoc.Enemies.CagedShockerStates
{
    public class AttackState : IState
    {
        public override string ToString() => "Attack";
        public AnimationState Animation => AnimationState.LightAttack;

        private readonly CagedShocker _cagedShocker;
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

        public AttackState(CagedShocker cagedShocker, CagedShockerAnimation animation, bool canCombo,
            float attackDuration)
        {
            _cagedShocker = cagedShocker;
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

        private void AnimationOnAttackPerformed()
        {
            /*var centerOffset = _cagedShocker.AttackOffset.localPosition;
            var direction = _cagedShocker.FacingLeft ? -1 : 1;
            centerOffset.x *= 0.5f * direction;
            var boxSize = new Vector2(_cagedShocker.AttackOffset.localPosition.x,
                _cagedShocker.AttackOffset.localPosition.y * 1.9f);*/
            var result = Physics2D.OverlapBox(_cagedShocker.HitBox.bounds.center, _cagedShocker.HitBox.bounds.size, 0f,
                _cagedShocker.Stats.AttackLayer);

            if (result && result.transform.TryGetComponent(out ITakeDamage takeDamage))
                _cagedShocker.DoDamage(takeDamage);
        }

        public void OnExit()
        {
            _player = null;
            _cagedShocker.OnStunned -= CagedShockerOnStunned;
            _animation.OnAttackPerformed -= AnimationOnAttackPerformed;
            _animation.OnAttackInterruptionAvailable -= AnimationOnAttackInterruptionAvailable;
        }
    }
}