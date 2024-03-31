using System;

namespace DarkHavoc.Enemies
{
    public class BossAnimation : EntityAnimation
    {
        protected override float NormalizedHorizontal => boss != null ? boss.GetNormalizedHorizontal() : 0f;
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