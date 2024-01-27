using System;
using DarkHavoc.PlayerComponents;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace DarkHavoc
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class TriggerInteractive<T> : MonoBehaviour
    {
        private static readonly int ShowOutline = Shader.PropertyToID("_ShowOutline");

        [SerializeField] private bool oneTimeTrigger;
        [SerializeField] private GameObject[] additionalVisuals;

        private bool _available;
        private InputReader _input;
        private SpriteRenderer _renderer;

        private MaterialPropertyBlock _propertyBlock;

        private void Awake() => _renderer = GetComponentInChildren<SpriteRenderer>();

        private void Start()
        {
            SetPropertyBlock(false);
            SetAdditionalVisuals(false);
        }

        protected void OnEnable() => _available = true;

        private void Update()
        {
            if (_available && _input is { Interact: true }) OnTriggerInteraction();
        }

        private void OnTriggerInteraction()
        {
            _available = false;
            TriggerInteraction();
        }

        protected abstract void TriggerInteraction();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out T t)) return;
            SetHoverState(true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out T t)) return;
            SetHoverState(false);
        }

        private void SetHoverState(bool state)
        {
            SetPropertyBlock(state);
            SetAdditionalVisuals(state);
            _input ??= ServiceLocator.GetService<InputReader>();
            if (!oneTimeTrigger) _available = state;
        }

        private void SetPropertyBlock(bool state)
        {
            _propertyBlock ??= new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat(ShowOutline, state ? 1f : 0f);
            _renderer.SetPropertyBlock(_propertyBlock);
        }

        private void SetAdditionalVisuals(bool state)
        {
            foreach (var visual in additionalVisuals) visual.SetActive(state);
        }
    }
}