using StateMachineComponents;
using UnityEngine;

namespace PlayerComponents.States
{
    public class AttackState : IState
    {
        public override string ToString() => AnimationState.LightAttack.ToString();

        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody;
        private readonly InputReader _input;

        private float _timer;
        private float _comboTimer;
        private Vector2 _targetVelocity;

        public bool Ended => _timer <= 0f;
        public bool CanCombo => _comboTimer <= 0f;
        public bool CanTransitionToSelf => true;

        public AttackState(Player player, Rigidbody2D rigidbody, InputReader input)
        {
            _player = player;
            _rigidbody = rigidbody;
            _input = input;
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;
            _comboTimer -= Time.deltaTime;

            var moveMultiplier = _player.Grounded ? _player.Stats.AttackSpeedConservation : 1;
            _player.Move(ref _targetVelocity, _input.Movement.x * moveMultiplier);

            if (_player.HasBufferedJump)
            {
                _timer = 0f;
                _player.Jump(ref _targetVelocity);
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
            _timer = _player.Stats.LightAttackTime;
            _comboTimer = _player.Stats.LightComboTime;

            _targetVelocity = _rigidbody.velocity;
            _player.Attack(ref _targetVelocity);
        }

        public void OnExit()
        {
        }
    }
}