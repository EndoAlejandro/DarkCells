using System;
using System.Collections.Generic;
using DarkHavoc.EntitiesInterfaces;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    public class CompositeHitBox : EnemyHitBox
    {
        [Serializable]
        private struct HitBoxStep
        {
            public EnemyHitBox[] hitBoxes;
        }

        [SerializeField] private List<HitBoxStep> hitBoxSteps;
        private int _index;


        private void OnValidate()
        {
            if (hitBoxSteps.Count != 0) return;
            HitBoxStep step = new HitBoxStep { hitBoxes = new EnemyHitBox[] { this } };
            hitBoxSteps = new List<HitBoxStep> { step };
        }

        public void ResetIndex() => _index = 0;

        public void TryToAttacks(bool isUnstoppable = false)
        {
            foreach (var hitBox in hitBoxSteps[_index].hitBoxes)
                hitBox.TryToAttack(isUnstoppable);

            if (!onCooldown) StartCoroutine(CooldownAsync());
            _index = (_index + 1) % hitBoxSteps.Count;
        }

        public override DamageResult TryToAttack(bool isUnstoppable = false)
        {
            SetUnstoppable(isUnstoppable);
            OverlapHitBox();

            DamageResult result = DamageResult.Failed;
            if (player) result = player.TakeDamage(doDamage, isUnstoppable: isUnstoppable);

            return result;
        }
    }
}