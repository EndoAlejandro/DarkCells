using UnityEngine;

namespace PlayerComponents
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerStateMachine))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerStats stats;

        private Rigidbody2D _rigidbody;
        private CapsuleCollider2D _collider;

        public bool IsGrounded { get; private set; }
        public bool EndedJumpEarly { get; private set; }
        public bool IsBufferedJumpAvailable { get; private set; }
        public bool IsCoyoteAvailable { get; private set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<CapsuleCollider2D>();
        }

        public void CustomGravity(ref Vector2 targetVelocity)
        {
            if (IsGrounded && targetVelocity.y <= 0)
            {
                targetVelocity.y = -stats.DownGravity;
            }
            else
            {
                var upGravity = stats.UpGravity;
                if (upGravity > 0f) upGravity *= 2f;
                targetVelocity.y = Mathf.MoveTowards(targetVelocity.y, -stats.MaxFallSpeed, upGravity * Time.deltaTime);
            }
        }

        public void Move(ref Vector2 targetVelocity, float input)
        {
            if (input == 0)
            {
                var deceleration = IsGrounded ? stats.GroundDeceleration : stats.AirDeceleration;
                targetVelocity.x = Mathf.MoveTowards(targetVelocity.x, 0f, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                targetVelocity.x = Mathf.MoveTowards(targetVelocity.x, input * stats.MaxSpeed,
                    stats.Acceleration * Time.fixedDeltaTime);
            }
        }

        public void Jump(ref Vector2 targetVelocity)
        {
            EndedJumpEarly = false;
            IsBufferedJumpAvailable = false;
            IsCoyoteAvailable = false;
            targetVelocity.y = stats.JumpForce;
        }

        public void CheckCollisions(ref Vector2 targetVelocity)
        {
            // Physics2D.queriesStartInColliders = false;

            bool groundHit = CheckCollisionCustomDirection(Vector2.down);
            bool ceilingHit = CheckCollisionCustomDirection(Vector2.up);

            if (ceilingHit) targetVelocity.y = Mathf.Min(0, targetVelocity.y);

            if (!IsGrounded && groundHit)
            {
                IsGrounded = true;
                IsCoyoteAvailable = true;
                IsBufferedJumpAvailable = true;
                EndedJumpEarly = false;
            }
            else if (IsGrounded && !groundHit)
            {
                IsGrounded = false;
            }

            // Physics2D.queriesStartInColliders = true;
        }

        private bool CheckCollisionCustomDirection(Vector2 direction) => Physics2D.CapsuleCast(_collider.bounds.center,
            _collider.size, _collider.direction, 0f, direction, stats.GrounderDistance, ~stats.Layer);

        public void ApplyVelocity(Vector2 targetVelocity) => _rigidbody.velocity = targetVelocity;
        public float GetNormalizedHorizontal()=>Mathf.Abs(_rigidbody.velocity.x) / (stats == null ? 1 : stats.MaxSpeed);
        public float GetNormalizedVertical() => _rigidbody.velocity.y;
    }
}