using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using DarkHavoc.CustomUtils;

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

        private void A(ref bool b){}
        
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