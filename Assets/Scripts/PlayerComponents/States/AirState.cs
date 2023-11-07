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

        private WallResult _wallResult;

        public bool CanTransitionToSelf => false;
        public bool FacingLedge { get; private set; }
        public bool FacingWall { get; private set; }

        public AirState(Player player, Rigidbody2D rigidbody, InputReader input)
        {
            _player = player;
            _rigidbody = rigidbody;
            _input = input;
        }

        public void Tick()
        {
            if (_player.CanMove && _player.HasBufferedJump)
            {
                _player.Jump();
                _player.ApplyVelocity();
            }
        }

        public void FixedTick()
        {
            _player.Move(_player.CanMove ? _input.Movement.x : 0f);

            if (_player.GetNormalizedVertical() > 0f) return;

            _wallResult =
                EntityVision.CheckWallCollision(_player.Collider, _player.Stats.WallDetection, _player.FacingLeft);

            if (_wallResult.FacingWall) FacingWall = true;

            float horizontal = _player.FacingLeft ? _player.Collider.bounds.min.x : _player.Collider.bounds.max.x;
            var spherePosition = new Vector3(_player.Stats.WallDetection.LedgeDetectorOffset.x + horizontal,
                _player.Collider.bounds.max.y + _player.Stats.WallDetection.LedgeDetectorOffset.y);

            var dif = spherePosition - (_player.transform.position);

            if (_player.HasBufferedLedgeGrab && _wallResult is { MidCheck: true, TopCheck: false })
            {
                var ledgeResult =
                    EntityVision.CheckLedge(_player.Collider, _player.Stats.WallDetection, _player.FacingLeft);
                if (ledgeResult != Vector2.zero)
                {
                    _player.transform.position = (Vector3)ledgeResult - dif;
                    FacingLedge = true;
                }
            }
        }

        public void OnEnter()
        {
            FacingWall = false;
            FacingLedge = false;
        }

        public void OnExit()
        {
        }
    }
}