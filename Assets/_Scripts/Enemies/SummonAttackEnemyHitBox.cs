using System;
using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.PlayerComponents;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    public class SummonAttackEnemyHitBox : EnemyHitBox
    {
        [SerializeField] private RangedSummonAttack attackPrefab;
        private Vector2 _target;

        protected override void OverlapHitBox()
        {
            var size = isCircle ? OverlapCircle() : OverlapBox();

            for (int i = 0; i < size; i++)
            {
                if (!results[i].TryGetComponent(out Player player) ||
                    !results[i].transform.root.TryGetComponent(out player)) continue;
                this.player = player;
                return;
            }

            player = null;
        }

        public void TryToAttack(Vector2 target, bool isUnstoppable = false)
        {
            _target = target;
            TryToAttack(isUnstoppable);
        }

        [Obsolete]
        public override DamageResult TryToAttack(bool isUnstoppable = false)
        {
            SetUnstoppable(isUnstoppable);
            /*OverlapHitBox();*/

            DamageResult result = DamageResult.Failed;
            if (entity.Player)
            {
                result = DamageResult.Success;
                var staticRangedAttack = Instantiate(attackPrefab, _target, Quaternion.identity);
                staticRangedAttack.Setup(doDamage);
            }
            else
            {
                Debug.Log("Failed");
            }

            StartCoroutine(CooldownAsync());
            return result;
        }
    }
}