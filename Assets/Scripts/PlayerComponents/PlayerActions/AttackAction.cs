using System;
using AttackComponents;
using UnityEngine;

namespace PlayerComponents.PlayerActions
{
    [Serializable]
    public class AttackAction : BufferedAction
    {
        private readonly Transform _attackOffset;

        private readonly Rigidbody2D _rigidbody;

        public AttackAction(Player player, Rigidbody2D rigidbody, Transform attackOffset, InputReader inputReader) :
            base(player, inputReader)
        {
            _rigidbody = rigidbody;
            _attackOffset = attackOffset;
        }

        protected override bool InputTrigger => InputReader.Attack;
        protected override float BufferTime => Player.Stats.LightAttackBuffer;

        protected override void UseBuffer(ref Vector2 targetVelocity)
        {
            var velocity = _rigidbody.velocity;
            // targetVelocity = new Vector2(velocity.x * Player.Stats.AttackMoveVelocity, velocity.y);

            var centerOffset = _attackOffset.localPosition;
            var direction = Player.FacingLeft ? -1 : 1;
            centerOffset.x *= 0.5f * direction;
            var boxSize = new Vector2(_attackOffset.localPosition.x, _attackOffset.localPosition.y * 1.9f);

            var result = Physics2D.OverlapBox(Player.transform.position + centerOffset,
                boxSize, 0f, ~Player.Stats.Layer);

            if (result && result.transform.TryGetComponent(out ITakeDamage takeDamage))
                takeDamage.TakeDamage(Player.Damage);
        }
    }
}