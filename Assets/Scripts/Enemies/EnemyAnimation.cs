using System;
using System.Collections;
using DarkHavoc.CustomUtils;
using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimation : MonoBehaviour
    {
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int HitValue = Shader.PropertyToID("_HitValue");

        public event Action OnAttackInterruptionAvailable;
        public event Action OnAttackPerformed;
        public event Action OnAttackEnded;

        protected Animator animator;
        protected new SpriteRenderer renderer;

        protected MaterialPropertyBlock materialPb;
        private FiniteStateBehaviour _stateMachine;
        private IState _previousState;

        private ITakeDamage _takeDamage;
        private IEntity _entity;

        private IEnumerator _hitAnimation;

        protected virtual float NormalizedHorizontal => 0f;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            renderer = GetComponent<SpriteRenderer>();

            _takeDamage = GetComponentInParent<ITakeDamage>();
            _entity = GetComponentInParent<IEntity>();
            _stateMachine = GetComponentInParent<FiniteStateBehaviour>();

            materialPb = new MaterialPropertyBlock();
        }

        protected virtual void OnEnable()
        {
            _takeDamage.OnDamageTaken += EnemyOnDamageTaken;
            _entity.OnXFlipped += EnemyOnXFlipped;
            _stateMachine.OnEntityStateChanged += StateMachineOnEntityStateChanged;
        }

        protected virtual void OnDisable()
        {
            _takeDamage.OnDamageTaken -= EnemyOnDamageTaken;
            _entity.OnXFlipped -= EnemyOnXFlipped;
            _stateMachine.OnEntityStateChanged -= StateMachineOnEntityStateChanged;
        }

        private void Update() =>
            animator.SetFloat(Horizontal, Mathf.Abs(NormalizedHorizontal));

        protected virtual void EnemyOnXFlipped(bool facingLeft) => renderer.flipX = facingLeft;

        private void StateMachineOnEntityStateChanged(IState state)
        {
            if (state.AnimationState == AnimationState.None) return;
            if (_previousState != null) animator.ResetTrigger(_previousState.AnimationState.ToString());

            animator.SetTrigger(state.AnimationState.ToString());
            _previousState = state;
        }

        private void EnemyOnDamageTaken()
        {
            if (_hitAnimation != null) StopCoroutine(_hitAnimation);
            _hitAnimation = HitAnimationAsync();
            StartCoroutine(_hitAnimation);
        }

        private IEnumerator HitAnimationAsync()
        {
            renderer.GetPropertyBlock(materialPb);

            float timer = 0f;
            while (timer < Constants.HitAnimationDuration)
            {
                timer += Time.deltaTime;
                float hitThreshold = 1 - (timer / Constants.HitAnimationDuration);
                materialPb.SetFloat(HitValue, hitThreshold);
                renderer.SetPropertyBlock(materialPb);
                yield return null;
            }

            yield return null;
            materialPb.SetFloat(HitValue, 0f);
            renderer.SetPropertyBlock(materialPb);
        }


        #region Animation Calls

        protected void PerformAttack() => OnAttackPerformed?.Invoke();
        protected void EndAttack() => OnAttackEnded?.Invoke();
        protected void AttackInterruptionAvailable() => OnAttackInterruptionAvailable?.Invoke();

        #endregion
    }
}