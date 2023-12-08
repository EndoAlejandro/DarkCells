using System.Collections;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.CagedShocker
{
    public class CagedShockerAnimation : EnemyAnimation
    {
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int HitValue = Shader.PropertyToID("_HitValue");

        [SerializeField] private float hitAnimationDuration = 1f;

        private SpriteRenderer _renderer;
        private CagedShockerStateMachine _cagedShockerStateMachine;
        private CagedShocker _cagedShocker;

        private MaterialPropertyBlock _materialPb;
        private IEnumerator _hitAnimation;
        private IState _previousState;

        protected override void Awake()
        {
            base.Awake();
            _renderer = GetComponent<SpriteRenderer>();
            _cagedShocker = GetComponentInParent<CagedShocker>();
            _cagedShockerStateMachine = GetComponentInParent<CagedShockerStateMachine>();

            _materialPb = new MaterialPropertyBlock();
        }

        private void EnemyOnXFlipped(bool facingLeft) => _renderer.flipX = _cagedShocker.FacingLeft;

        private void Update()
        {
            animator.SetFloat(Horizontal, Mathf.Abs(_cagedShocker.GetNormalizedHorizontal()));
        }

        private void OnEnable()
        {
            _cagedShockerStateMachine.OnEntityStateChanged += CagedShockerStateMachineOnEntityStateChanged;
            _cagedShocker.OnXFlipped += EnemyOnXFlipped;
            _cagedShocker.OnDamageTaken += CagedShockerOnDamageTaken;
        }

        private void CagedShockerStateMachineOnEntityStateChanged(IState state)
        {
            if (_previousState != null) animator.ResetTrigger(_previousState.Animation.ToString());

            animator.SetTrigger(state.Animation.ToString());
            _previousState = state;
        }


        private void CagedShockerOnDamageTaken()
        {
            if (_hitAnimation != null) StopCoroutine(_hitAnimation);
            _hitAnimation = HitAnimation();
            StartCoroutine(_hitAnimation);
        }

        private IEnumerator HitAnimation()
        {
            _renderer.GetPropertyBlock(_materialPb);

            float timer = 0f;
            while (timer < hitAnimationDuration)
            {
                timer += Time.deltaTime;
                float hitThreshold = 1 - (timer / hitAnimationDuration);
                _materialPb.SetFloat(HitValue, hitThreshold);
                _renderer.SetPropertyBlock(_materialPb);
                yield return null;
            }

            yield return null;
            _materialPb.SetFloat(HitValue, 0f);
            _renderer.SetPropertyBlock(_materialPb);
        }

        private void OnDisable()
        {
            _cagedShocker.OnXFlipped -= EnemyOnXFlipped;
            _cagedShockerStateMachine.OnEntityStateChanged -= CagedShockerStateMachineOnEntityStateChanged;
            _cagedShocker.OnDamageTaken -= CagedShockerOnDamageTaken;
        }
    }
}