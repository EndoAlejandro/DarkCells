using System;
using DarkHavoc.EntitiesInterfaces;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    public abstract class ProjectileAttack : MonoBehaviour
    {
        protected abstract Vector2 Direction { get; }

        [SerializeField] private float velocity = 5f;

        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private IEntity _entity;

        private Action _onCollisionCallback;

        public virtual void Setup(IEntity entity, Action onCollisionCallback)
        {
            _entity = entity;
            _onCollisionCallback = onCollisionCallback;
            
            _collider ??= GetComponent<Collider2D>();
            _rigidbody ??= GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate() => _rigidbody.velocity = Direction * velocity;

        private void OnTriggerEnter2D(Collider2D other)
        {
            _onCollisionCallback?.Invoke();
            Destroy(gameObject);
        }
    }
}