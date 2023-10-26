using System;
using System.Collections;
using StateMachineComponents;
using UnityEngine;

namespace Enemies
{
    public class CagedShockerAnimation : MonoBehaviour
    {
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");

        [SerializeField] private float hitAnimationDuration = 1f;

        private SpriteRenderer _renderer;
        private Animator _animator;
        private CagedShockerStateMachine _cagedShockerStateMachine;
        private CagedShocker _cagedShocker;

        private MaterialPropertyBlock _materialPb;
        private IEnumerator _hitAnimation;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _cagedShocker = GetComponentInParent<CagedShocker>();
            _cagedShockerStateMachine = GetComponentInParent<CagedShockerStateMachine>();

            _materialPb = new MaterialPropertyBlock();
        }

        private void Update()
        {
            _renderer.flipX = _cagedShocker.FacingLeft;
            _animator.SetFloat(Horizontal, Mathf.Abs(_cagedShocker.GetNormalizedHorizontal()));
        }

        private void OnEnable()
        {
            _cagedShockerStateMachine.OnEntityStateChanged += CagedShockerStateMachineOnEntityStateChanged;
            _cagedShocker.OnTakeDamage += CagedShockerOnTakeDamage;
        }

        private void CagedShockerStateMachineOnEntityStateChanged(IState state) =>
            _animator.SetTrigger(state.ToString());

        private void CagedShockerOnTakeDamage()
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
                _materialPb.SetFloat("_HitValue", hitThreshold);
                _renderer.SetPropertyBlock(_materialPb);
                yield return null;
            }

            yield return null;
            _materialPb.SetFloat("_HitValue", 0f);
            _renderer.SetPropertyBlock(_materialPb);
        }

        private void OnDisable()
        {
            _cagedShockerStateMachine.OnEntityStateChanged -= CagedShockerStateMachineOnEntityStateChanged;
            _cagedShocker.OnTakeDamage -= CagedShockerOnTakeDamage;
        }
    }
}