using DarkHavoc.CustomUtils;
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
        private readonly Rigidbody2D _rigidbody;
        private readonly InputReader _input;
        private readonly PlayerAnimation _animation;
        private readonly AttackImpulseAction _attackAction;

        private float _timer;

        // private float _comboTimer;
        private Vector2 _targetVelocity;

        public bool Ended => _timer <= 0f;

        public bool CanCombo { get; private set; }

        // public bool CanCombo => _comboTimer <= 0f;
        public bool CanTransitionToSelf => true;
        public bool CanHeavyAttack { get; private set; }

        public AttackState(Player player, Rigidbody2D rigidbody, InputReader input, PlayerAnimation animation,
            AnimationState animationState, AttackImpulseAction attackAction)
        {
            _player = player;
            _rigidbody = rigidbody;
            _input = input;
            _animation = animation;
            _attackAction = attackAction;
            Animation = animationState;

            CanHeavyAttack = true;
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;
            // _comboTimer -= Time.deltaTime;

            _targetVelocity.x = _attackAction.Decelerate(_targetVelocity.x, Time.deltaTime);
            _player.Move(ref _targetVelocity, _input.Movement.x * _player.Stats.MovementReduction);
            // _targetVelocity.x

            if (_player.HasBufferedJump)
            {
                _timer = 0f;
                _player.Jump(ref _targetVelocity);
                _player.ApplyVelocity(_targetVelocity);
            }
        }

        public void FixedTick()
        {
            _player.CheckCollisions(ref _targetVelocity);
            _player.CustomGravity(ref _targetVelocity);

            _player.ApplyVelocity(_targetVelocity);
        }

        public void OnEnter()
        {
            CanHeavyAttack = !CanHeavyAttack;

            _animation.OnAttackPerformed += AnimationOnAttackPerformed;
            _animation.OnComboAvailable += AnimationOnComboAvailable;

            _targetVelocity.x = _attackAction.GetTargetVelocity(_player.Direction);

            _timer = _attackAction.Time;
            _targetVelocity.y = _rigidbody.velocity.y;
            // _comboTimer = _player.Stats.LightComboTime;
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