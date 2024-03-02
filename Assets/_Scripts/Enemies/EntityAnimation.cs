using System.Collections;
using DarkHavoc.CustomUtils;
using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Enemies
{
    [RequireComponent(typeof(Animator))]
    public abstract class EntityAnimation : MonoBehaviour
    {
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int HitValue = Shader.PropertyToID("_HitValue");

        protected abstract float NormalizedHorizontal { get; }
        protected virtual float NormalizedVertical => 0f;

        protected Animator animator;
        protected new SpriteRenderer renderer;

        protected MaterialPropertyBlock materialPb;
        private FiniteStateBehaviour _stateMachine;
        private IState _previousState;

        private ITakeDamage _takeDamage;
        private IEntity _entity;

        private IEnumerator _hitAnimation;
        private float _hitThreshold;
        private float _timer;

        [SerializeField] private bool useVerticalHash;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            renderer = GetComponent<SpriteRenderer>();

            _takeDamage = GetComponentInParent<ITakeDamage>();
            _entity = GetComponentInParent<IEntity>();
            _stateMachine = GetComponentInParent<FiniteStateBehaviour>();

            materialPb = new MaterialPropertyBlock();
        }

        protected virtual void Update()
        {
            animator.SetFloat(Horizontal, Mathf.Abs(NormalizedHorizontal));
            if (useVerticalHash) animator.SetFloat(Vertical, Mathf.Abs(NormalizedVertical));
        }

        protected virtual void OnEnable()
        {
            if (_takeDamage != null) _takeDamage.OnDamageTaken += EnemyOnDamageTaken;
            if (_entity != null) _entity.OnXFlipped += EnemyOnXFlipped;
            if (_stateMachine != null) _stateMachine.OnEntityStateChanged += StateMachineOnEntityStateChanged;
        }

        protected virtual void OnDisable()
        {
            if (_takeDamage != null) _takeDamage.OnDamageTaken -= EnemyOnDamageTaken;
            if (_entity != null) _entity.OnXFlipped -= EnemyOnXFlipped;
            if (_stateMachine != null) _stateMachine.OnEntityStateChanged -= StateMachineOnEntityStateChanged;
        }

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
            _timer = 0f;
        }

        private void LateUpdate()
        {
            renderer.GetPropertyBlock(materialPb);
            if (_timer < Constants.HitAnimationDuration)
            {
                _timer += Time.deltaTime;
                _hitThreshold = 1 - (_timer / Constants.HitAnimationDuration);
                materialPb.SetFloat(HitValue, _hitThreshold);
            }
            else
                materialPb.SetFloat(HitValue, 0f);

            renderer.SetPropertyBlock(materialPb);
        }
    }
}