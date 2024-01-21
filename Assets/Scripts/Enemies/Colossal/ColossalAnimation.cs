using System;
using UnityEngine;

namespace DarkHavoc.Enemies.Colossal
{
    public class ColossalAnimation : EnemyAnimation
    {
        private static readonly int TurnAround = Animator.StringToHash("TurnAround");
        private Colossal _colossal;

        protected override void Awake()
        {
            base.Awake();
            _colossal = GetComponentInParent<Colossal>();
        }

        protected override void EnemyOnXFlipped(bool facingLeft)
        {
            base.EnemyOnXFlipped(facingLeft);
            animator.SetTrigger(TurnAround);
        }

        protected override float NormalizedHorizontal => _colossal.GetNormalizedHorizontal();

        #region Animation Methods

        public event Action OnRangedAttack;
        private void PerformRangedAttack() => OnRangedAttack?.Invoke();

        public event Action OnMeleeAttack;
        private void PerformMeleeAttack() => OnMeleeAttack?.Invoke();

        public event Action OnBoomerangAttack;
        private void PerformBoomerangAttack() => OnBoomerangAttack?.Invoke();

        public event Action OnBuffAttack;
        private void PerformBuffAttack() => OnBuffAttack?.Invoke();

        #endregion
    }
}