using System;
using DarkHavoc.CustomUtils;
using DarkHavoc.Enemies;
using UnityEngine;

namespace DarkHavoc.Boss
{
    public class BossAnimation : EntityAnimation
    {
        protected static readonly int OutlineColorID = Shader.PropertyToID("_OutlineColor");
        
        protected override float NormalizedHorizontal => boss != null ? boss.GetNormalizedHorizontal() : 0f;
        protected override Color OutlineColor => Constants.BossOutlineColor;
        protected Boss boss;

        protected override void Awake()
        {
            base.Awake();
            boss = GetComponentInParent<Boss>();
        }

        #region Animation Calls

        public event Action OnAttackFx;
        protected void AttackFx() => OnAttackFx?.Invoke();

        public event Action OnAttackPerformed;
        protected void PerformAttack() => OnAttackPerformed?.Invoke();

        public event Action OnAttackEnded;
        protected void EndAttack() => OnAttackEnded?.Invoke();

        public event Action OnAttackInterruptionAvailable;
        protected void AttackInterruptionAvailable() => OnAttackInterruptionAvailable?.Invoke();

        #endregion
    }
}