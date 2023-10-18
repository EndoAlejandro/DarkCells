using UnityEngine;

namespace PlayerComponents.PlayerActions
{
    public class JumpAction : BufferedAction
    {
        private bool _airJumpAvailable;
        private bool _canAirJump;
        protected override bool InputTrigger => InputReader.Jump;
        protected override float BufferTime => Player.Stats.JumpBuffer;

        public bool EndedJumpEarly { get; set; }
        public bool IsCoyoteAvailable { get; set; }

        public JumpAction(Player player, InputReader inputReader) : base(player, inputReader)
        {
            player.OnGroundedChanged += PlayerOnGroundedChanged;
        }

        private void PlayerOnGroundedChanged(bool grounded)
        {
            Debug.Log("Player Grounded.");
            _canAirJump = true;
        }

        protected override void UseBuffer(ref Vector2 targetVelocity)
        {
            if (!Player.Grounded)
            {
                if (_canAirJump) _canAirJump = false;
                else return;
            }

            targetVelocity.y = Player.Stats.JumpForce;
        }
    }
}