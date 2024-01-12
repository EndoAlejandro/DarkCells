using DarkHavoc.CustomUtils;
using DarkHavoc.Senses;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.States
{
    public class AirState : IState
    {
        public override string ToString() => "Air";
        public AnimationState AnimationState => AnimationState.Air;

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
            if (_player.HasBufferedJump)
            {
                _player.Jump();
                _player.ApplyVelocity();
            }
        }

        public void FixedTick()
        {
            _player.Move(_input.Movement.x);

            if (_player.GetNormalizedVertical() > 0f) return;

            _wallResult =
                EntityVision.CheckWallCollision(_player.Collider, _player.Stats.WallDetection, _player.FacingLeft);

            if (_wallResult.FacingWall) FacingWall = true;

            if (_player.HasBufferedLedgeGrab && _wallResult is { MidCheck: true, TopCheck: false })
            {
                var ledgeResult =
                    EntityVision.CheckLedge(_player.Collider, _player.Stats.WallDetection, _player.FacingLeft);
                if (!ledgeResult.IsDefault()) FacingLedge = true;
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