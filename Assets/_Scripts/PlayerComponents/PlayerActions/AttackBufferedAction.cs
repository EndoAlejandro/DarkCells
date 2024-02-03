using System;
using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.ImpulseComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents.PlayerActions
{
    [Serializable]
    public class AttackBufferedAction : BufferedAction
    {
        private readonly Transform _attackOffset;
        private Collider2D[] _hitboxResults;
        private RaycastHit2D[] _obstacleCheckResults;

        private float _cooldown;
        private float _comboTimer;
        private int _attackCount;

        public AttackBufferedAction(Transform attackOffset, Player player, float bufferTime,
            Func<bool> inputTrigger) : base(player, bufferTime, inputTrigger)
        {
            _attackOffset = attackOffset;

            _hitboxResults = new Collider2D[50];
            _obstacleCheckResults = new RaycastHit2D[50];
        }

        protected override bool CanBuffer() => _cooldown <= 0f && base.CanBuffer();

        public override void Tick()
        {
            if (_cooldown > 0f) _cooldown -= Time.deltaTime;
            if (_comboTimer > 0f) _comboTimer -= Time.deltaTime;
            else _attackCount = 0;
            base.Tick();
        }

        public void UseAction(AttackImpulseAction attackImpulse)
        {
            base.UseAction();

            _attackCount++;

            _comboTimer = Player.Stats.ComboTime;
            _cooldown = attackImpulse.CoolDownTime;

            Vector3 centerOffset = _attackOffset.localPosition;
            int direction = Player.FacingLeft ? -1 : 1;
            centerOffset.x *= 0.5f * direction;

            Vector2 boxSize = new Vector2(_attackOffset.localPosition.x, _attackOffset.localPosition.y * 1.9f);

            int hitboxSize = Physics2D.OverlapBoxNonAlloc(Player.transform.position + centerOffset,
                boxSize, 0f, _hitboxResults, Player.Stats.AttackLayerMask);

            for (int i = 0; i < hitboxSize; i++)
            {
                if (_hitboxResults[i].transform.TryGetComponent(out ITakeDamage takeDamage))
                {
                    var lineCastSize = Physics2D.LinecastNonAlloc(Player.MidPoint.position,
                        takeDamage.MidPoint.position, _obstacleCheckResults, Player.Stats.WallDetection.WallLayer);

                    if (lineCastSize == 0) Player.DoDamage(takeDamage, attackImpulse.DamageMultiplier);
                }
            }
        }

        public bool CanPerformHeavyAttack()
        {
            if (_attackCount <= 1) return false;
            _attackCount = 0;
            return true;
        }
    }
}