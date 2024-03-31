using System;

namespace DarkHavoc.Enemies
{
    public class EnemyAnimation : EntityAnimation
    {
        protected override float NormalizedHorizontal => enemy != null ? enemy.GetNormalizedHorizontal() : 0f;
        protected override float NormalizedVertical => enemy != null ? enemy.GetNormalizedVertical() : 0f;
        protected Enemy enemy;

        protected override void Awake()
        {
            base.Awake();
            enemy = GetComponentInParent<Enemy>();
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