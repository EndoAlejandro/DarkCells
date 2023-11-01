using System;
using DarkHavoc.AttackComponents;
using DarkHavoc.ImpulseComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.PlayerActions
{
    [Serializable]
    public class AttackAction : BufferedAction
    {
        private readonly Transform _attackOffset;
        private Collider2D[] _results;

        private float _cooldown;

        public AttackAction(Transform attackOffset, Player player, float bufferTime,
            Func<bool> inputTrigger) : base(player, bufferTime, inputTrigger)
        {
            _attackOffset = attackOffset;

            _results = new Collider2D[50];
        }

        protected override bool CanBuffer() => _cooldown <= 0f && base.CanBuffer();

        public override void Tick()
        {
            if (_cooldown > 0f) _cooldown -= Time.deltaTime;
            base.Tick();
        }

        public void UseAction(AttackImpulseAction attackImpulse)
        {
            base.UseAction();

            _cooldown = attackImpulse.CoolDownTime;

            Vector3 centerOffset = _attackOffset.localPosition;
            int direction = Player.FacingLeft ? -1 : 1;
            centerOffset.x *= 0.5f * direction;
            
            Vector2 boxSize = new Vector2(_attackOffset.localPosition.x, _attackOffset.localPosition.y * 1.9f);

            int size = Physics2D.OverlapBoxNonAlloc(Player.transform.position + centerOffset,
                boxSize, 0f, _results, ~Player.Stats.AttackLayerMask);

            for (int i = 0; i < size; i++)
            {
                var result = _results[i];
                if (result.transform.TryGetComponent(out ITakeDamage takeDamage))
                    Player.DoDamage(takeDamage, attackImpulse.DamageMultiplier);
            }
        }
    }
}