using UnityEngine;

namespace PlayerComponents.PlayerActions
{
    public class RollAction : BufferedAction
    {
        protected override bool InputTrigger => InputReader.Roll;
        protected override float BufferTime => Player.Stats.JumpBuffer;

        public RollAction(Player player, InputReader inputReader) : base(player, inputReader)
        {
        }

        protected override void UseBuffer(ref Vector2 targetVelocity)
        {
            var direction = Player.FacingLeft ? -1 : 1;
            targetVelocity.x = Mathf.MoveTowards(targetVelocity.x, direction * Player.Stats.RollMaxSpeed,
                Player.Stats.RollAcceleration * Time.fixedDeltaTime);
        }
    }

    public class BlockAction : BufferedAction
    {
        protected override bool InputTrigger => InputReader.Block;
        protected override float BufferTime => Player.Stats.LightAttackBuffer;
        
        public BlockAction(Player player, InputReader inputReader) : base(player, inputReader)
        {
        }

        protected override void UseBuffer(ref Vector2 targetVelocity)
        {
        }
    }
}