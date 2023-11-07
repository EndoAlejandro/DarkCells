using System;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.PlayerActions
{
    [Serializable]
    public class WallJumpBufferedAction : BufferedAction
    {
        private bool _canJump;

        public WallJumpBufferedAction(Player player, float bufferTime, Func<bool> inputTrigger) : base(player,
            bufferTime, inputTrigger) => player.OnWallSlideChanged += PlayerOnWallSlideChanged;

        private void PlayerOnWallSlideChanged(bool value) => _canJump = value;

        public void UseAction(ref Vector2 targetVelocity)
        {
            if (!_canJump) return;

            _canJump = false;
            Player.SetFacingLeft(!Player.FacingLeft);
            targetVelocity = new Vector2(Player.Direction * Player.Stats.WallJumpForce.x,
                Player.Stats.WallJumpForce.y);
            Player.StopMovementForSeconds(.15f);
            base.UseAction();
        }
    }
}