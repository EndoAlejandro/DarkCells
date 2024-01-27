using System;
using DarkHavoc.PlayerComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.Colossal
{
    public class ColossalBoomerangArms : MonoBehaviour
    {
        public event Action<bool> OnDestroyed;
        public float Cooldown => cooldown;

        [SerializeField] private float speed = 2f;
        [SerializeField] private float cooldown = 5f;

        private SpriteRenderer _renderer;
        private Rigidbody2D _rigidbody;
        private Colossal _colossal;

        private Vector3 _direction;
        private Vector3 _initialPosition;
        private Vector3 _targetPosition;

        private bool _returning;
        private float _travelDistance;

        public void Setup(Colossal colossal, float travelDistance)
        {
            _colossal = colossal;
            _renderer ??= GetComponentInChildren<SpriteRenderer>();
            _rigidbody ??= GetComponent<Rigidbody2D>();

            _initialPosition = transform.position;

            _renderer.flipX = _colossal.FacingLeft;

            _direction = _colossal.FacingLeft ? Vector3.left : Vector3.right;
            _travelDistance = travelDistance;
            _targetPosition = transform.position + _direction * travelDistance;
            _returning = false;
        }

        private void FixedUpdate()
        {
            var distance = Vector3.Distance(transform.position, _returning ? _initialPosition : _targetPosition);
            var normalizedDistance = distance / _travelDistance;
            _rigidbody.velocity = _direction * (speed + normalizedDistance);

            if (distance > 1f) return;
            if (_returning) DestroyArms();
            else Return();
        }

        private void Return()
        {
            _direction *= -1;
            _returning = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.transform.root.TryGetComponent(out Player player)) return;
            _colossal.DoDamage(player);
            player.TakeDamage(_colossal);
            if (!_returning) Return();
        }

        private void DestroyArms(bool hitPlayer = false)
        {
            OnDestroyed?.Invoke(hitPlayer);
            Destroy(gameObject);
        }
    }
}