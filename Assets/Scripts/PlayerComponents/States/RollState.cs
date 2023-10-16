using CustomUtils;
using StateMachineComponents;
using UnityEngine;

namespace PlayerComponents.States
{
    public class RollState : IState
    {
        public override string ToString() => AnimationState.Roll.ToString();

        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody;
        private readonly InputReader _input;

        private Vector2 _targetVelocity;
        private float _timer;

        public bool Ended { get; private set; }

        public RollState(Player player, Rigidbody2D rigidbody, InputReader input)
        {
            _player = player;
            _rigidbody = rigidbody;
            _input = input;
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;

            switch (_input.Movement.x)
            {
                case > 0 when _player.FacingLeft:
                case < 0 when !_player.FacingLeft:
                    _targetVelocity.x = _rigidbody.velocity.x;
                    Ended = true;
                    break;
            }

            if (_timer <= 0f) Ended = true;
        }

        public void FixedTick()
        {
            _player.CheckCollisions(ref _targetVelocity);

            _player.Roll(ref _targetVelocity);
            _player.CustomGravity(ref _targetVelocity);

            _player.ApplyVelocity(_targetVelocity);
        }

        public void OnEnter()
        {
            Ended = false;
            _targetVelocity = _rigidbody.velocity;
            _timer = _player.Stats.RollTime;
        }

        public void OnExit()
        {
            _targetVelocity.x *= _player.Stats.RollSpeedConservation;
            _player.ApplyVelocity(_targetVelocity);
            Ended = false;
        }
    }
}