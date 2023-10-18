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

        protected override bool InputTrigger => InputReader.LightAttack;
        protected override float BufferTime => Player.Stats.LightAttackBuffer;

        protected override void UseBuffer(ref Vector2 targetVelocity)
        {
            var velocity = _rigidbody.velocity;
            targetVelocity = new Vector2(velocity.x * Player.Stats.AttackSpeedConservation, velocity.y);

            var centerOffset = _attackOffset.localPosition;
            centerOffset.x *= 0.5f;
            var origin = Player.transform.position + centerOffset;
            var size = new Vector2(_attackOffset.position.x, _attackOffset.localPosition.y * 2);
            var result = Physics2D.BoxCast(origin, size, 0f, Player.FacingLeft ? Vector2.left : Vector2.right, 3,
                ~Player.Stats.Layer);

            if (result &&
                result.transform.TryGetComponent(out ITakeDamage takeDamage))
                takeDamage.TakeDamage(Player.Damage);
        }
    }
}