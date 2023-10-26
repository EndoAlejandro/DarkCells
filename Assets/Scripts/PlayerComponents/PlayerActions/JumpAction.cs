using System;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.PlayerActions
{
    [Serializable]
    public class JumpAction : BufferedAction
    {
        private readonly Rigidbody2D _rigidbody;

        private float _timeLeftGrounded;
        private bool _airJumpAvailable;
        private bool _canAirJump;
        private bool _coyoteTimeAvailable;

        private bool CanUseCoyote => _coyoteTimeAvailable && !Player.Grounded &&
                                     Timer < _timeLeftGrounded + Player.Stats.CoyoteTime;

        public bool EndedJumpEarly { get; private set; }
        protected override bool InputTrigger => InputReader.Jump;
        protected override float BufferTime => Player.Stats.JumpBuffer;

        public JumpAction(Player player, Rigidbody2D rigidbody, InputReader inputReader) : base(player, inputReader)
        {
            _rigidbody = rigidbody;
            player.OnGroundedChanged += PlayerOnGroundedChanged;
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

        protected override void UseBuffer(ref Vector2 targetVelocity)
        {
            if (!Player.Grounded && !CanUseCoyote)
            {
                if (_canAirJump) _canAirJump = false;
                else return;
            }

            _coyoteTimeAvailable = false;
            EndedJumpEarly = false;
            targetVelocity.y = Player.Stats.JumpForce;
        }

        private void EndedUpEarlyCheck()
        {
            if (!EndedJumpEarly && !Player.Grounded && !InputReader.JumpHold && _rigidbody.velocity.y > 0f)
                EndedJumpEarly = true;
        }
    }
}