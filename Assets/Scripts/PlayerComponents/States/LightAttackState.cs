using StateMachineComponents;
using UnityEngine;

namespace PlayerComponents.States
{
    public class LightAttackState : IState
    {
        public override string ToString() => AnimationState.LightAttack.ToString();

        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody;
        private float _timer;
        private float _comboTimer;
        private Vector2 _targetVelocity;
        private bool _canJump;

        public bool Ended => _timer <= 0f;
        public bool CanCombo => _comboTimer <= 0f;
        public bool CanTransitionToSelf => true;

        public LightAttackState(Player player, Rigidbody2D rigidbody)
        {
            _player = player;
            _rigidbody = rigidbody;
        }


        public void Tick()
        {
            _timer -= Time.deltaTime;
            _comboTimer -= Time.deltaTime;

            if (_player.HasBufferedJump && _canJump && CanCombo)
            {
                _canJump = false;
                _timer = 0f;

                _targetVelocity = Vector2.zero;
                _player.Jump(ref _targetVelocity);
                _player.ApplyVelocity(_targetVelocity);
            }
        }

        public void FixedTick()
        {
        }

        public void OnEnter()
        {
            _canJump = true;
            _timer = _player.Stats.LightAttackTime;
            _comboTimer = _player.Stats.LightComboTime;

            _player.Attack();
        }

        public void OnExit()
        {
        }
    }
}