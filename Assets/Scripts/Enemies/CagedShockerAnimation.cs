using System;
using StateMachineComponents;
using UnityEngine;

namespace Enemies
{
    public class CagedShockerAnimation : MonoBehaviour
    {
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");

        private SpriteRenderer _renderer;
        private Animator _animator;
        private CagedShockerStateMachine _cagedShockerStateMachine;
        private CagedShocker _cagedShocker;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _cagedShocker = GetComponentInParent<CagedShocker>();
            _cagedShockerStateMachine = GetComponentInParent<CagedShockerStateMachine>();
        }

        private void Update()
        {
            _renderer.flipX = _cagedShocker.FacingLeft;
            _animator.SetFloat(Horizontal, Mathf.Abs(_cagedShocker.GetNormalizedHorizontal()));
        }

        private void OnEnable() => _cagedShockerStateMachine.OnEntityStateChanged +=
            CagedShockerStateMachineOnEntityStateChanged;

        private void CagedShockerStateMachineOnEntityStateChanged(IState state) =>
            _animator.SetTrigger(state.ToString());

        private void OnDisable() => _cagedShockerStateMachine.OnEntityStateChanged +=
            CagedShockerStateMachineOnEntityStateChanged;
    }
}