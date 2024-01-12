using System;
using System.Collections;
using DarkHavoc.CustomUtils;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimation : MonoBehaviour
    {
        private static readonly int HitValue = Shader.PropertyToID("_HitValue");

        public event Action OnAttackInterruptionAvailable;
        public event Action OnAttackPerformed;

        protected Animator animator;
        protected new SpriteRenderer renderer;

        private MaterialPropertyBlock _materialPb;
        private FiniteStateBehaviour _stateMachine;
        private IState _previousState;

        private IEnumerator _hitAnimation;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            renderer = GetComponent<SpriteRenderer>();

            _stateMachine = GetComponentInParent<FiniteStateBehaviour>();

            _materialPb = new MaterialPropertyBlock();
        }

        protected virtual void OnEnable() => _stateMachine.OnEntityStateChanged += StateMachineOnEntityStateChanged;
        protected virtual void OnDisable() => _stateMachine.OnEntityStateChanged -= StateMachineOnEntityStateChanged;

        private void StateMachineOnEntityStateChanged(IState state)
        {
            if (state.AnimationState == AnimationState.None) return;
            if (_previousState != null) animator.ResetTrigger(_previousState.AnimationState.ToString());

            animator.SetTrigger(state.AnimationState.ToString());
            _previousState = state;
        }

        protected void PlayHitAnimation()
        {
            if (_hitAnimation != null) StopCoroutine(_hitAnimation);
            _hitAnimation = HitAnimationAsync();
            StartCoroutine(_hitAnimation);
        }

        private IEnumerator HitAnimationAsync()
        {
            renderer.GetPropertyBlock(_materialPb);

            float timer = 0f;
            while (timer < Constants.HitAnimationDuration)
            {
                timer += Time.deltaTime;
                float hitThreshold = 1 - (timer / Constants.HitAnimationDuration);
                _materialPb.SetFloat(HitValue, hitThreshold);
                renderer.SetPropertyBlock(_materialPb);
                yield return null;
            }

            yield return null;
            _materialPb.SetFloat(HitValue, 0f);
            renderer.SetPropertyBlock(_materialPb);
        }


        #region Animation Calls

        private void PerformAttack() => OnAttackPerformed?.Invoke();
        private void AttackInterruptionAvailable() => OnAttackInterruptionAvailable?.Invoke();

        #endregion
    }
}