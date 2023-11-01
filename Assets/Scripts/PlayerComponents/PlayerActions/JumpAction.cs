using System;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.PlayerActions
{
    [Serializable]
    public class JumpAction : BufferedAction
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly InputReader _inputReader;

        private float _timeLeftGrounded;
        private bool _canAirJump;
        private bool _coyoteTimeAvailable;

        private bool CanUseCoyote => _coyoteTimeAvailable && !Player.Grounded &&
                                     Timer < _timeLeftGrounded + Player.Stats.CoyoteTime;

        public bool EndedJumpEarly { get; private set; }

        public JumpAction(Player player, Rigidbody2D rigidbody, InputReader inputReader, float bufferTime,
            Func<bool> inputTrigger) : base(
            player, bufferTime, inputTrigger)
        {
            _rigidbody = rigidbody;
            _inputReader = inputReader;
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

        public void UseAction(ref Vector2 targetVelocity)
        {
            base.UseAction();
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
            if (!EndedJumpEarly && !Player.Grounded && !_inputReader.JumpHold && _rigidbody.velocity.y > 0f)
                EndedJumpEarly = true;
        }
    }
}