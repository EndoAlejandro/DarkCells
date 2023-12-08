using System;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimation : MonoBehaviour
    {
        public event Action OnAttackInterruptionAvailable;
        public event Action OnAttackPerformed;

        protected Animator animator;

        protected virtual void Awake() => animator = GetComponent<Animator>();

        #region Animation Calls

        private void PerformAttack() => OnAttackPerformed?.Invoke();
        private void AttackInterruptionAvailable() => OnAttackInterruptionAvailable?.Invoke();

        #endregion
    }
}