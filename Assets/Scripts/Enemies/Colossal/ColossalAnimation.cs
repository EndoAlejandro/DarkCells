using System;
using UnityEngine;

namespace DarkHavoc.Enemies.Colossal
{
    public class ColossalAnimation : EntityAnimation
    {
        private static readonly int TurnAround = Animator.StringToHash("TurnAround");
        private static readonly int ShowOutline = Shader.PropertyToID("_ShowOutline");
        private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");
        
        private Colossal _colossal;

        protected override void Awake()
        {
            base.Awake();
            _colossal = GetComponentInParent<Colossal>();
        }

        private void Start()
        {
            _colossal.OnBuffStateChanged += ColossalOnBuffStateChanged;
        }

        private void ColossalOnBuffStateChanged(bool state)
        {
            renderer.GetPropertyBlock(materialPb);
            materialPb.SetColor(OutlineColor,_colossal.Stats.BuffOutlineColor);
            materialPb.SetFloat(ShowOutline, state ? 1f : 0f);
            renderer.SetPropertyBlock(materialPb);
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