using System;
using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.PlayerComponents;
using DarkHavoc.Senses;
using UnityEngine;

namespace DarkHavoc.Enemies.CagedShocker
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class CagedShocker : Enemy, IStunnable
    {
        public event Action OnStunned;
        public event Action<float> OnTelegraph;
        public float IdleTime => Stats != null ? Stats.IdleTime : 0f;
        public override float Damage => Stats != null ? Stats.Damage : 0;
        public bool Grounded => LeftFoot || RightFoot;
        public bool LeftFoot { get; private set; }
        public bool RightFoot { get; private set; }
        public float StunTime { get; private set; }
        public Player Player { get; private set; }
        public CagedShockerStats Stats => stats as CagedShockerStats;

        private Collider2D _collider;
        private Rigidbody2D _rigidbody;
        private Collider2D[] _results;

        private Vector2 _targetVelocity;

        private void Awake()
        {
            _collider = GetComponent<CapsuleCollider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();

            _results = new Collider2D[50];
        }

        private void OnEnable()
        {
            if (Stats == null) stats = ScriptableObject.CreateInstance<CagedShockerStats>();
            Health = Stats != null ? Stats.MaxHealth : 0;
            MaxHealth = Health;
        }

        private void FixedUpdate()
        {
            if (!IsAlive) return;

            CheckGrounded();
            CustomGravity();

            ApplyVelocity();
        }

        public void CheckGrounded()
        {
            var leftFootPosition =
                new Vector2(_collider.bounds.min.x - Stats.FootPositionOffset.x, transform.position.y);
            LeftFoot = Physics2D.Raycast(leftFootPosition + Vector2.up * Stats.FootPositionOffset.y, Vector2.down,
                Stats.GroundOffset + Stats.GroundCheckDistance, Stats.GroundLayerMask);

            var rightFootPosition =
                new Vector2(_collider.bounds.max.x + Stats.FootPositionOffset.x, transform.position.y);
            RightFoot = Physics2D.Raycast(rightFootPosition + Vector2.up * Stats.FootPositionOffset.y, Vector2.down,
                Stats.GroundOffset + Stats.GroundCheckDistance, Stats.GroundLayerMask);
        }

        public void Move(int direction)
        {
            if (direction == 0)
            {
                var deceleration = Stats.Acceleration;
                _targetVelocity.x = Mathf.MoveTowards(_targetVelocity.x, 0f, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _targetVelocity.x = Mathf.MoveTowards(_targetVelocity.x, direction * Stats.MaxSpeed,
                    Stats.Acceleration * Time.fixedDeltaTime);
            }
        }

        public void CustomGravity()
        {
            _targetVelocity.y = Mathf.MoveTowards(_targetVelocity.y, -Stats.MaxFallSpeed,
                Stats.Gravity * Time.fixedDeltaTime);
        }

        public void ApplyVelocity() => _rigidbody.velocity = _targetVelocity;

        public void TelegraphAttack(float time) => OnTelegraph?.Invoke(time);

        public void SeekPlayer()
        {
            var tempPlayer =
                EntityVision.CircularCheck<Player>(MidPoint.position, Stats.DetectionDistance, ref _results);
            if (tempPlayer == null) return;

            if (!IsPlayerVisible(tempPlayer))
                return;

            Player = tempPlayer;
        }

        public bool IsPlayerVisible(Player player) =>
            EntityVision.IsVisible<Player>(MidPoint.position, player.MidPoint.position, FacingLeft, gameObject.layer);

        public void KeepTrackPlayer()
        {
            if (Player == null) return;

            float distance = Vector3.Distance(Player.transform.position, transform.position);
            if (distance > Stats.ScapeDistance) Player = null;
        }

        public override void TakeDamage(IDoDamage damageDealer, float damageMultiplier, bool unstoppable)
        {
            if (damageDealer.transform.TryGetComponent(out Player player))
                Player = player;

            base.TakeDamage(damageDealer, damageMultiplier, unstoppable);
        }

        public override void Death()
        {
            _collider.enabled = false;
            _rigidbody.simulated = false;
            base.Death();
        }

        public override float GetNormalizedHorizontal() => Mathf.Abs(_rigidbody.velocity.x) / Stats.MaxSpeed;


        public void Stun() => OnStunned?.Invoke();

        private void OnDrawGizmos()
        {
            if (Stats == null) stats = ScriptableObject.CreateInstance<CagedShockerStats>();
            if (_collider == null) _collider = GetComponent<CapsuleCollider2D>();

            DetectionRange(MidPoint.position);
            Gizmos.color = Color.magenta;
            GroundRays(transform.position);
            WallRays();
        }

        private void DetectionRange(Vector3 position)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(position, Stats.DetectionDistance);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, Stats.ScapeDistance);
        }

        private void GroundRays(Vector3 position)
        {
            var offset = Vector3.up * Stats.FootPositionOffset.y;
            var distance = Vector3.down * (Stats.GroundOffset + Stats.GroundCheckDistance);
            // Left Foot.
            var leftFootPosition =
                new Vector3(_collider.bounds.min.x - Stats.FootPositionOffset.x, position.y);
            Gizmos.DrawLine(leftFootPosition + offset, leftFootPosition + offset + distance);
            // Right Foot.
            var rightFootPosition =
                new Vector3(_collider.bounds.max.x + Stats.FootPositionOffset.x, position.y);
            Gizmos.DrawLine(rightFootPosition + offset, rightFootPosition + offset + distance);
        }

        private void WallRays()
        {
            float horizontal = FacingLeft ? _collider.bounds.min.x : _collider.bounds.max.x;
            horizontal += Stats.WallDetection.HorizontalOffset;
            var direction = FacingLeft ? Vector2.left : Vector2.right;
            // Top Ray.
            Vector2 topOrigin = new Vector2(horizontal, _collider.bounds.max.y + Stats.WallDetection.TopOffset);
            Gizmos.DrawLine(topOrigin, topOrigin + (direction * Stats.WallDetection.DistanceCheck));
            // Middle Ray.
            var centerOrigin = new Vector2(horizontal, _collider.bounds.center.y + Stats.WallDetection.MidOffset);
            Gizmos.DrawLine(centerOrigin, centerOrigin + (direction * Stats.WallDetection.DistanceCheck));
            // Bottom Ray.
            Vector2 bottomOrigin = new Vector2(horizontal, _collider.bounds.min.y + Stats.WallDetection.BottomOffset);
            Gizmos.DrawLine(bottomOrigin, bottomOrigin + (direction * Stats.WallDetection.DistanceCheck));
        }
    }
}