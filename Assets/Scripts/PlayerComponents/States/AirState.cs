using DarkHavoc.Senses;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.States
{
    public class AirState : IState
    {
        public override string ToString() => "Air";
        public AnimationState Animation => AnimationState.Air;

        private readonly Player _player;
        private readonly Rigidbody2D _rigidbody;
        private readonly InputReader _input;

        private Vector2 _targetVelocity;
        private WallResult _wallResult;

        public bool CanTransitionToSelf => false;
        public bool FacingLedge { get; private set; }

        public AirState(Player player, Rigidbody2D rigidbody, InputReader input)
        {
            _player = player;
            _rigidbody = rigidbody;
            _input = input;
        }


        public void Tick()
        {
            _player.Move(_input.Movement.x);

            _wallResult =
                EntityVision.CheckWallCollision(_player.Collider, _player.Stats.WallDetection, _player.FacingLeft);

            if (_wallResult is { MidCheck: true, TopCheck: false })
            {
                var ledgeResult =
                    EntityVision.CheckLedge(_player.Collider, _player.Stats.WallDetection, _player.FacingLeft);
                if (ledgeResult != Vector2.zero)
                {
                    var go = new GameObject($"{ledgeResult.x}:{ledgeResult.y}");
                    go.transform.position = ledgeResult;
                }
            }

            if (_player.HasBufferedJump)
            {
                _player.Jump();
                _player.ApplyVelocity();
            }
        }


        public void FixedTick()
        {
        }

        public void OnEnter()
        {
            FacingLedge = false;
            _targetVelocity = _rigidbody.velocity;
        }

        public void OnExit()
        {
        }
    }
}