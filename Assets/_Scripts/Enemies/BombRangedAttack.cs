using System;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class BombRangedAttack : MonoBehaviour
    {
        [SerializeField] private float gravity = 5f;

        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private Action _onCollisionCallback;

        public void Setup(Action onCollisionCallback)
        {
            _collider ??= GetComponent<Collider2D>();
            _rigidbody ??= GetComponent<Rigidbody2D>();

            _onCollisionCallback = onCollisionCallback;
        }

        private void FixedUpdate() => _rigidbody.velocity = Vector2.down * gravity;

        private void OnTriggerEnter2D(Collider2D other)
        {
            _onCollisionCallback?.Invoke();
            Destroy(gameObject);
        }
    }
}