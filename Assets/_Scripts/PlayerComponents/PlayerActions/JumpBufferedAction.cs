using System;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.PlayerActions
{
    [Serializable]
    public class JumpBufferedAction : BufferedAction
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly InputReader _inputReader;

        private float _timeLeftGrounded;
        private bool _canAirJump;
        private bool _coyoteTimeAvailable;
        private bool _canWallJump;

        private bool CanUseCoyote => _coyoteTimeAvailable && !Player.Grounded &&
                                     Timer < _timeLeftGrounded + Player.Stats.CoyoteTime;

        public bool EndedJumpEarly { get; private set; }

        public JumpBufferedAction(Player player, Rigidbody2D rigidbody, InputReader inputReader, float bufferTime,
            Func<bool> inputTrigger) : base(
            player, bufferTime, inputTrigger)
        {
            _rigidbody = rigidbody;
            _inputReader = inputReader;
            player.OnGroundedChanged += PlayerOnGroundedChanged;
            player.OnLedgeGrabChanged += PlayerOnLedgeGrabChanged;
            player.OnWallSlideChanged += PlayerOnWallSlideChanged;
        }

        private void PlayerOnWallSlideChanged(bool value)
        {
            _coyoteTimeAvailable = true;
            _canWallJump = true;
        }

        private void PlayerOnLedgeGrabChanged(bool value)
        {
            _coyoteTimeAvailable = true;
            _canAirJump = true;
        }

        private void PlayerOnGroundedChanged(bool grounded)
        {
            if (grounded)
            {
                _coyoteTimeAvailable = true;
                _canAirJump = true;
            }
            else
            {
                _timeLeftGrounded = Timer;
            }
        }

        public override void Tick()
        {
            base.Tick();
            EndedUpEarlyCheck();
        }

        public void UseAction(ref Vector2 targetVelocity)
        {
            if (!Player.Grounded && !CanUseCoyote)
            {
                if (_canAirJump) _canAirJump = false;
                else return;
            }

            base.UseAction();
            _coyoteTimeAvailable = false;
            EndedJumpEarly = false;
            targetVelocity.y = Player.Stats.JumpForce;
        }

        public void UseWallAction(ref Vector2 targetVelocity)
        {
            if (!_canWallJump) return;

            _canWallJump = false;
            Player.SetFacingLeft(!Player.FacingLeft);
            targetVelocity = Player.Stats.WallJumpForce;
            targetVelocity.x *= Player.FacingLeft ? -1 : 1;
            base.UseAction();
        }

        private void EndedUpEarlyCheck()
        {
            if (!EndedJumpEarly && !Player.Grounded && !_inputReader.JumpHold && _rigidbody.velocity.y > 0f)
                EndedJumpEarly = true;
        }
    }
}