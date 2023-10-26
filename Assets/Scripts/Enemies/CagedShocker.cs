using System;
using AttackComponents;
using PlayerComponents;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class CagedShocker : MonoBehaviour, IDoDamage, ITakeDamage
    {
        [SerializeField] private CagedShockerStats stats;

        private Collider2D _collider;
        private Rigidbody2D _rigidbody;
        public event Action OnTakeDamage;

        public float IdleTime => stats != null ? stats.IdleTime : 0f;
        public int Damage => stats != null ? stats.Damage : 0;
        public int Health { get; private set; }
        public bool Grounded { get; private set; }
        public bool FacingLeft { get; private set; }
        public Player Player { get; private set; }

        private void Awake()
        {
            _collider = GetComponent<CapsuleCollider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            if (stats == null) stats = ScriptableObject.CreateInstance<CagedShockerStats>();
            Health = stats != null ? stats.MaxHealth : 0;
        }

        public void CheckGrounded(out bool leftFoot, out bool rightFoot)
        {
            var leftFootPosition =
                new Vector2(_collider.bounds.min.x - stats.FootPositionOffset.x, transform.position.y);
            leftFoot = Physics2D.Raycast(leftFootPosition + Vector2.up * stats.FootPositionOffset.y, Vector2.down,
                stats.GroundOffset + stats.GroundCheckDistance, stats.GroundLayerMask);

            var rightFootPosition =
                new Vector2(_collider.bounds.max.x + stats.FootPositionOffset.x, transform.position.y);
            rightFoot = Physics2D.Raycast(rightFootPosition + Vector2.up * stats.FootPositionOffset.y, Vector2.down,
                stats.GroundOffset + stats.GroundCheckDistance, stats.GroundLayerMask);

            Grounded = leftFoot || rightFoot;
        }

        public void CheckWallCollisions(out bool facingWall)
        {
            Physics2D.queriesStartInColliders = false;
            facingWall = false;
            var wallCheckTopOffset = stats != null ? stats.WallCheckTopOffset : 0f;
            var wallCheckBottomOffset = stats != null ? stats.WallCheckBottomOffset : 0f;

            float horizontal = FacingLeft ? _collider.bounds.min.x : _collider.bounds.max.x;
            var direction = FacingLeft ? Vector2.left : Vector2.right;

            // Center Check.
            var origin = new Vector2(horizontal, _collider.bounds.center.y);
            facingWall = WallRayCast(origin, direction);
            if (facingWall) return;

            // Top Check.
            origin.y = _collider.bounds.max.y - wallCheckTopOffset;
            facingWall = WallRayCast(origin, direction);
            if (facingWall) return;

            // Bottom Check.
            origin.y = _collider.bounds.min.y + wallCheckBottomOffset;
            facingWall = WallRayCast(origin, direction);
        }

        private bool WallRayCast(Vector2 origin, Vector2 direction) =>
            Physics2D.Raycast(origin, direction, stats.WallDistanceCheck, stats.GroundLayerMask);

        public void Move(ref Vector2 targetVelocity, int direction)
        {
            if (direction == 0)
            {
                var deceleration = 1f;
                targetVelocity.x = Mathf.MoveTowards(targetVelocity.x, 0f, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                targetVelocity.x = Mathf.MoveTowards(targetVelocity.x, direction * stats.MaxSpeed,
                    stats.Acceleration * Time.fixedDeltaTime);
            }
        }

        public void CustomGravity(ref Vector2 targetVelocity)
        {
            targetVelocity.y = Mathf.MoveTowards(targetVelocity.y, -stats.MaxFallSpeed,
                stats.Gravity * Time.fixedDeltaTime);
        }

        public void ApplyVelocity(Vector2 targetVelocity) => _rigidbody.velocity = targetVelocity;

        public void DoDamage(ITakeDamage takeDamage) => Debug.Log($"Enemy do damage to {takeDamage.transform.name}");

        public void TakeDamage(int damage, Vector2 damageSource)
        {
            Health -= damage;
            OnTakeDamage?.Invoke();
        }

        public void Death()
        {
        }

        public float GetNormalizedHorizontal() => Mathf.Abs(_rigidbody.velocity.x) / stats.MaxSpeed;
        public void SetFacingLeft(bool value) => FacingLeft = value;

        private void OnDrawGizmos()
        {
            if (_collider == null) _collider = GetComponent<CapsuleCollider2D>();

            Gizmos.color = Color.magenta;

            // Ground Rays.
            var offset = Vector3.up * stats.FootPositionOffset.y;
            var distance = Vector3.down * (stats.GroundOffset + stats.GroundCheckDistance);
            // Left Foot.
            var leftFootPosition =
                new Vector3(_collider.bounds.min.x - stats.FootPositionOffset.x, transform.position.y);
            Gizmos.DrawLine(leftFootPosition + offset, leftFootPosition + offset + distance);
            // Right Foot.
            var rightFootPosition =
                new Vector3(_collider.bounds.max.x + stats.FootPositionOffset.x, transform.position.y);
            Gizmos.DrawLine(rightFootPosition + offset, rightFootPosition + offset + distance);

            // Wall Rays.
            float horizontal = FacingLeft ? _collider.bounds.min.x : _collider.bounds.max.x;
            var direction = FacingLeft ? Vector2.left : Vector2.right;
            // Top Ray.
            Vector2 topOrigin = new Vector2(horizontal, _collider.bounds.max.y - stats.WallCheckTopOffset);
            Gizmos.DrawLine(topOrigin, topOrigin + (direction * stats.WallDistanceCheck));
            // Middle Ray.
            var centerOrigin = new Vector2(horizontal, _collider.bounds.center.y);
            Gizmos.DrawLine(centerOrigin, centerOrigin + (direction * stats.WallDistanceCheck));
            // Bottom Ray.
            Vector2 bottomOrigin = new Vector2(horizontal, _collider.bounds.min.y + stats.WallCheckBottomOffset);
            Gizmos.DrawLine(bottomOrigin, bottomOrigin + (direction * stats.WallDistanceCheck));
        }
    }
}