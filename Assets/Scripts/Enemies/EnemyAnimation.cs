using System;

namespace DarkHavoc.Enemies
{
    public class EnemyAnimation : EntityAnimation
    {
        public event Action OnAttackInterruptionAvailable;
        public event Action OnAttackPerformed;
        public event Action OnAttackEnded;
        protected override float NormalizedHorizontal => _enemy != null ? _enemy.GetNormalizedHorizontal() : 0f;
        
        private Enemy _enemy;

        protected override void Awake()
        {
            base.Awake();
            _enemy = GetComponentInParent<Enemy>();
        }

        #region Animation Calls

        protected void PerformAttack() => OnAttackPerformed?.Invoke();
        protected void EndAttack() => OnAttackEnded?.Invoke();
        protected void AttackInterruptionAvailable() => OnAttackInterruptionAvailable?.Invoke();

        #endregion
    }
}