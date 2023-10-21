using System;
using AttackComponents;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class CagedShocker : MonoBehaviour, IDoDamage, ITakeDamage
    {
        [SerializeField] private int damage = 1;
        [SerializeField] private int maxHealth = 10;
        [SerializeField] private float gravity;
        [SerializeField] private float acceleration;
        [SerializeField] private float maxFallSpeed;
        [SerializeField] private float maxSpeed;

        [SerializeField] private float groundOffset;
        [SerializeField] private float groundCheckDistance;
        [SerializeField] private LayerMask groundLayerMask;

        private Collider2D _collider;
        private Rigidbody2D _rigidbody;

        public int Damage => damage;
        public int Health { get; private set; }
        public bool Grounded { get; private set; }

        private void Awake()
        {
            _collider = GetComponent<CapsuleCollider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();

            Health = maxHealth;
        }

        public void CheckGrounded(out bool leftFoot, out bool rightFoot)
        {
            var leftFootPosition = new Vector2(transform.position.x + _collider.bounds.min.x, transform.position.y);
            leftFoot = Physics2D.Raycast(leftFootPosition + Vector2.up * groundOffset, Vector2.down,
                groundOffset + groundCheckDistance, groundLayerMask);

            var rightFootPosition = new Vector2(transform.position.x + _collider.bounds.max.x, transform.position.y);
            rightFoot = Physics2D.Raycast(rightFootPosition + Vector2.up * groundOffset, Vector2.down,
                groundOffset + groundCheckDistance, groundLayerMask);

            Grounded = leftFoot || rightFoot;
        }

        public void Move(ref Vector2 targetVelocity, int direction)
        {
            if (direction == 0)
            {
                var deceleration = 1f;
                targetVelocity.x = Mathf.MoveTowards(targetVelocity.x, 0f, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                targetVelocity.x = Mathf.MoveTowards(targetVelocity.x, direction * maxSpeed,
                    acceleration * Time.fixedDeltaTime);
            }
        }

        public void CustomGravity(ref Vector2 targetVelocity)
        {
            targetVelocity.y = Mathf.MoveTowards(targetVelocity.y, -maxFallSpeed,
                gravity * Time.fixedDeltaTime);
        }

        public void ApplyVelocity(Vector2 targetVelocity) => _rigidbody.velocity = targetVelocity;

        public void DoDamage()
        {
            // TODO: Do damage.
        }

        public void TakeDamage(IDoDamage damageDealer)
        {
            Health -= damageDealer.Damage;
        }

        public void Death()
        {
        }

        private void OnDrawGizmos()
        {
            if (_collider == null) _collider = GetComponent<CapsuleCollider2D>();

            Gizmos.color = Color.magenta;
            
            var offset = Vector3.up * groundOffset;
            var distance = Vector3.down * (groundOffset + groundCheckDistance);
            var leftFootPosition = new Vector3(_collider.bounds.min.x, transform.position.y);
            Gizmos.DrawLine(leftFootPosition + offset, leftFootPosition + offset + distance);
            var rightFootPosition = new Vector3(_collider.bounds.max.x, transform.position.y);
            Gizmos.DrawLine(rightFootPosition + offset, rightFootPosition + offset + distance);
        }
    }
}