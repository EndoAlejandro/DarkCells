using System;
using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.PlayerComponents;
using DarkHavoc.Senses;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class CagedShocker : MonoBehaviour, IDoDamage, ITakeDamage, IEntity, IStunnable
    {
        [SerializeField] private CagedShockerStats stats;
        [SerializeField] private Transform attackOffset;
        [SerializeField] private Transform midPoint;

        private Collider2D _collider;
        private Rigidbody2D _rigidbody;
        private Collider2D[] _results;

        public event Action OnStunned;
        public event Action OnDamageTaken;
        public event Action<float> OnTelegraph;
        public float IdleTime => stats != null ? stats.IdleTime : 0f;
        public float Damage => stats != null ? stats.Damage : 0;
        public Vector3 MidPoint => midPoint.position;
        public float Health { get; private set; }
        public bool IsAlive => Health > 0f;
        public Transform AttackOffset => attackOffset;
        public bool Grounded { get; private set; }
        public bool FacingLeft { get; private set; }
        public float StunTime { get; private set; }
        public Player Player { get; private set; }
        public CagedShockerStats Stats => stats;

        private void Awake()
        {
            _collider = GetComponent<CapsuleCollider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();

            _results = new Collider2D[50];
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

        public void Move(ref Vector2 targetVelocity, int direction)
        {
            if (direction == 0)
            {
                var deceleration = stats.Acceleration;
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

        public void TelegraphAttack(float time) => OnTelegraph?.Invoke(time);

        public void SeekPlayer()
        {
            var tempPlayer = EntityVision.CircularCheck<Player>(MidPoint, stats.DetectionDistance, ref _results);
            if (tempPlayer == null) return;

            if (!IsPlayerVisible(tempPlayer))
                return;

            Player = tempPlayer;
        }

        public bool IsPlayerVisible(Player player) =>
            EntityVision.IsVisible<Player>(MidPoint, player.MidPoint, FacingLeft, gameObject.layer);

        public void KeepTrackPlayer()
        {
            if (Player == null) return;

            float distance = Vector3.Distance(Player.transform.position, transform.position);
            if (distance > stats.ScapeDistance) Player = null;
        }

        public void DoDamage(ITakeDamage takeDamage, float damageMultiplier = 1f) =>
            takeDamage.TakeDamage(this, damageMultiplier);

        public void TakeDamage(IDoDamage damageDealer, float damageMultiplier)
        {
            if (damageDealer.transform.TryGetComponent(out Player player))
                Player = player;
            
            Health -= damageDealer.Damage;
            OnDamageTaken?.Invoke();
        }

        public void Death()
        {
            _collider.enabled = false;
            _rigidbody.simulated = false;
        }

        public float GetNormalizedHorizontal() => Mathf.Abs(_rigidbody.velocity.x) / stats.MaxSpeed;
        public void SetFacingLeft(bool value) => FacingLeft = value;
        public void Stun() => OnStunned?.Invoke();

        private void OnDrawGizmos()
        {
            if (stats == null) stats = ScriptableObject.CreateInstance<CagedShockerStats>();
            if (_collider == null) _collider = GetComponent<CapsuleCollider2D>();

            DetectionRange(MidPoint);
            Gizmos.color = Color.magenta;
            GroundRays(transform.position);
            WallRays();
        }

        private void DetectionRange(Vector3 position)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(position, stats.DetectionDistance);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, stats.ScapeDistance);
        }

        private void GroundRays(Vector3 position)
        {
            var offset = Vector3.up * stats.FootPositionOffset.y;
            var distance = Vector3.down * (stats.GroundOffset + stats.GroundCheckDistance);
            // Left Foot.
            var leftFootPosition =
                new Vector3(_collider.bounds.min.x - stats.FootPositionOffset.x, position.y);
            Gizmos.DrawLine(leftFootPosition + offset, leftFootPosition + offset + distance);
            // Right Foot.
            var rightFootPosition =
                new Vector3(_collider.bounds.max.x + stats.FootPositionOffset.x, position.y);
            Gizmos.DrawLine(rightFootPosition + offset, rightFootPosition + offset + distance);
        }

        private void WallRays()
        {
            float horizontal = FacingLeft ? _collider.bounds.min.x : _collider.bounds.max.x;
            horizontal += Stats.WallDetection.HorizontalOffset;
            var direction = FacingLeft ? Vector2.left : Vector2.right;
            // Top Ray.
            Vector2 topOrigin = new Vector2(horizontal, _collider.bounds.max.y - stats.WallDetection.TopOffset);
            Gizmos.DrawLine(topOrigin, topOrigin + (direction * stats.WallDetection.DistanceCheck));
            // Middle Ray.
            var centerOrigin = new Vector2(horizontal, _collider.bounds.center.y);
            Gizmos.DrawLine(centerOrigin, centerOrigin + (direction * stats.WallDetection.DistanceCheck));
            // Bottom Ray.
            Vector2 bottomOrigin = new Vector2(horizontal, _collider.bounds.min.y + stats.WallDetection.BottomOffset);
            Gizmos.DrawLine(bottomOrigin, bottomOrigin + (direction * stats.WallDetection.DistanceCheck));
        }
    }
}