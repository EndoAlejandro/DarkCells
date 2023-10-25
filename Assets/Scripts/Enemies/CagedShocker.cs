using AttackComponents;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class CagedShocker : MonoBehaviour, IDoDamage, ITakeDamage
    {
        [SerializeField] private float idleTime = 1f;
        [SerializeField] private int damage = 1;
        [SerializeField] private int maxHealth = 10;
        [SerializeField] private float gravity;
        [SerializeField] private float acceleration;
        [SerializeField] private float maxFallSpeed;
        [SerializeField] private float maxSpeed;

        [SerializeField] private Vector2 offset;
        [SerializeField] private float groundOffset;
        [SerializeField] private float groundCheckDistance;
        [SerializeField] private float wallDistanceCheck;
        [SerializeField] private float wallCheckTopOffset;
        [SerializeField] private float wallCheckBottomOffset;
        [SerializeField] private LayerMask groundLayerMask;

        private Collider2D _collider;
        private Rigidbody2D _rigidbody;

        public float IdleTime => idleTime;
        public int Damage => damage;
        public int Health { get; private set; }
        public bool Grounded { get; private set; }
        public bool FacingLeft { get; private set; }

        private void Awake()
        {
            _collider = GetComponent<CapsuleCollider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();

            Health = maxHealth;
        }

        public void CheckGrounded(out bool leftFoot, out bool rightFoot)
        {
            var leftFootPosition = new Vector2(_collider.bounds.min.x - offset.x, transform.position.y);
            leftFoot = Physics2D.Raycast(leftFootPosition + Vector2.up * offset.y, Vector2.down,
                groundOffset + groundCheckDistance, groundLayerMask);

            var rightFootPosition = new Vector2(_collider.bounds.max.x + offset.x, transform.position.y);
            rightFoot = Physics2D.Raycast(rightFootPosition + Vector2.up * offset.y, Vector2.down,
                groundOffset + groundCheckDistance, groundLayerMask);

            Grounded = leftFoot || rightFoot;
        }

        public void CheckWallCollisions(out bool facingWall)
        {
            facingWall = false;

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
            Physics2D.Raycast(origin, direction, wallDistanceCheck, groundLayerMask);

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

        public float GetNormalizedHorizontal() => Mathf.Abs(_rigidbody.velocity.x) / maxSpeed;
        public void SetFacingLeft(bool value) => FacingLeft = value;

        private void OnDrawGizmos()
        {
            if (_collider == null) _collider = GetComponent<CapsuleCollider2D>();

            Gizmos.color = Color.magenta;

            // Ground Rays.
            var offset = Vector3.up * this.offset.y;
            var distance = Vector3.down * (groundOffset + groundCheckDistance);
            // Left Foot.
            var leftFootPosition = new Vector3(_collider.bounds.min.x - this.offset.x, transform.position.y);
            Gizmos.DrawLine(leftFootPosition + offset, leftFootPosition + offset + distance);
            // Right Foot.
            var rightFootPosition = new Vector3(_collider.bounds.max.x + this.offset.x, transform.position.y);
            Gizmos.DrawLine(rightFootPosition + offset, rightFootPosition + offset + distance);

            // Wall Rays.
            float horizontal = FacingLeft ? _collider.bounds.min.x : _collider.bounds.max.x;
            var direction = FacingLeft ? Vector2.left : Vector2.right;
            // Top Ray.
            Vector2 topOrigin = new Vector2(horizontal, _collider.bounds.max.y - wallCheckTopOffset);
            Gizmos.DrawLine(topOrigin, topOrigin + (direction * wallDistanceCheck));
            // Middle Ray.
            var centerOrigin = new Vector2(horizontal, _collider.bounds.center.y);
            Gizmos.DrawLine(centerOrigin, centerOrigin + (direction * wallDistanceCheck));
            // Bottom Ray.
            Vector2 bottomOrigin = new Vector2(horizontal, _collider.bounds.min.y + wallCheckBottomOffset);
            Gizmos.DrawLine(bottomOrigin, bottomOrigin + (direction * wallDistanceCheck));
        }
    }
}