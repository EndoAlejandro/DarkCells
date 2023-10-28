using DarkHavoc.CustomUtils;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.States
{
    public class RollState : IState
    {
        public override string ToString() => "Roll";
        public AnimationState Animation => AnimationState.Roll;

        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody;
        private readonly InputReader _input;
        private readonly ImpulseAction _rollAction;

        private Vector2 _targetVelocity;
        private float _timer;

        public bool Ended { get; private set; }
        public bool CanTransitionToSelf => false;

        public RollState(Player player, Rigidbody2D rigidbody, InputReader input, ImpulseAction rollAction)
        {
            _player = player;
            _rigidbody = rigidbody;
            _input = input;
            _rollAction = rollAction;
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;

            /*switch (_input.Movement.x)
            {
                case > 0 when _player.FacingLeft:
                case < 0 when !_player.FacingLeft:
                    _targetVelocity.x = _rigidbody.velocity.x;
                    Ended = true;
                    break;
            }*/

            _targetVelocity.x = _rollAction.Decelerate(_targetVelocity.x, Time.deltaTime);

            if (_player.HasBufferedJump)
            {
                _player.Jump(ref _targetVelocity);
                Ended = true;
            }

            if (_timer <= 0f) Ended = true;
        }

        public void FixedTick()
        {
            _player.CheckCollisions(ref _targetVelocity);
            // _player.Roll(ref _targetVelocity);
            _player.CustomGravity(ref _targetVelocity);

            _player.ApplyVelocity(_targetVelocity);
        }

        public void OnEnter()
        {
            Ended = false;
            
            float y = _rigidbody.velocity.y > 0 ? _rigidbody.velocity.y : 0f;
            float x = _rollAction.GetTargetVelocity(_player.Direction);
            _targetVelocity = new Vector2(x, y);
            
            _timer = _rollAction.Time;
            _player.SetPlayerCollider(false);
        }

        public void OnExit()
        {
            Ended = false;
            _player.SetPlayerCollider(true);
        }
    }
}