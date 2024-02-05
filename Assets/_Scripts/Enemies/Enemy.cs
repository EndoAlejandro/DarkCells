using System;
using System.Collections;
using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.PlayerComponents;
using DarkHavoc.Senses;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public abstract class Enemy : MonoBehaviour, IDoDamage, ITakeDamage, IEntity
    {
        public event Action OnDamageTaken;
        public event Action OnDeath;
        public event Action<bool> OnXFlipped;
        public event Action<bool> OnBuffStateChanged;
        public bool CanBuff { get; private set; }
        public bool IsBuffActive { get; private set; }
        public Transform MidPoint => midPoint;
        public Player Player { get; protected set; }
        public EnemyStats Stats => stats;
        public float MaxHealth { get; protected set; }
        public float Health { get; protected set; } = 1;
        public bool IsAlive => Health > 0f;
        public bool FacingLeft { get; private set; }
        public bool LeftFoot { get; private set; }
        public bool RightFoot { get; private set; }
        public bool Grounded => LeftFoot || RightFoot;
        public bool LedgeInFront => FacingLeft ? !LeftFoot && RightFoot : !RightFoot && LeftFoot;
        public abstract float Damage { get; }
        public EnemyHitBox HitBox => hitbox;

        [SerializeField] protected EnemyStats stats;
        [SerializeField] private Transform midPoint;
        [SerializeField] private EnemyHitBox hitbox;
        [SerializeField] private bool radialVision;
        [SerializeField] protected bool debug;

        protected new Collider2D collider;
        protected new Rigidbody2D rigidbody;
        protected Vector2 targetVelocity;

        private Collider2D[] _results;

        protected virtual void Awake()
        {
            collider = GetComponent<Collider2D>();
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            if (Stats == null) stats = ScriptableObject.CreateInstance<EnemyStats>();
            Health = Stats != null ? Stats.MaxHealth : 0;
            MaxHealth = Health;
        }

        private void FixedUpdate()
        {
            if (!IsAlive) return;

            KeepTrackPlayer();
            CheckGrounded();
            CustomGravity();

            ApplyVelocity();
        }

        private void CheckGrounded()
        {
            var leftFootPosition =
                new Vector2(collider.bounds.min.x - Stats.FootPositionOffset.x, transform.position.y);
            LeftFoot = Physics2D.Raycast(leftFootPosition + Vector2.up * Stats.FootPositionOffset.y, Vector2.down,
                Stats.GroundOffset + Stats.GroundCheckDistance, Stats.GroundLayerMask);

            var rightFootPosition =
                new Vector2(collider.bounds.max.x + Stats.FootPositionOffset.x, transform.position.y);
            RightFoot = Physics2D.Raycast(rightFootPosition + Vector2.up * Stats.FootPositionOffset.y, Vector2.down,
                Stats.GroundOffset + Stats.GroundCheckDistance, Stats.GroundLayerMask);
        }

        public void Move(int direction, float overrideAcceleration = -1f)
        {
            if (direction == 0)
            {
                var deceleration = Stats.Acceleration * 2;
                targetVelocity.x = Mathf.MoveTowards(targetVelocity.x, 0f, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                var acceleration = overrideAcceleration >= 0 ? overrideAcceleration : Stats.Acceleration;
                var speed = Player == null ? Stats.PatrolSpeed : Stats.MaxSpeed;
                targetVelocity.x = Mathf.MoveTowards(targetVelocity.x, direction * speed,
                    acceleration * Time.fixedDeltaTime);
            }
        }

        private void CustomGravity()
        {
            targetVelocity.y = Mathf.MoveTowards(targetVelocity.y, -Stats.MaxFallSpeed,
                Stats.Gravity * Time.fixedDeltaTime);
        }

        private void ApplyVelocity() => rigidbody.velocity = targetVelocity;

        public void SeekPlayer()
        {
            var tempPlayer =
                EntityVision.CircularCheck<Player>(MidPoint.position, Stats.DetectionDistance, ref _results);
            if (tempPlayer == null)
                return;

            if (!IsPlayerVisible(tempPlayer))
                return;

            Player = tempPlayer;
        }

        public void KeepTrackPlayer()
        {
            if (Player == null) return;

            float distance = Vector3.Distance(Player.transform.position, transform.position);
            if (distance > Stats.ScapeDistance) Player = null;
        }

        public bool IsPlayerVisible(Player player) =>
            EntityVision.IsVisible<Player>(MidPoint.position, player.MidPoint.position, FacingLeft, gameObject.layer,
                radialVision);

        public void SetFacingLeft(bool facingLeft)
        {
            FacingLeft = facingLeft;
            OnXFlipped?.Invoke(FacingLeft);
        }

        public void DoDamage(ITakeDamage takeDamage, float damageMultiplier = 1, bool unstoppable = false) =>
            takeDamage.TakeDamage(this, damageMultiplier, unstoppable);

        public virtual DamageResult TakeDamage(IDoDamage damageDealer, float damageMultiplier, bool isUnstoppable)
        {
            if (!IsAlive) return DamageResult.Killed;

            if (damageDealer.transform.TryGetComponent(out Player player)) Player = player;
            Health = Mathf.Max(Health - damageDealer.Damage, 0f);
            OnDamageTaken?.Invoke();

            if (Health > 0 && Health % (MaxHealth / 2) == 0) CanBuff = true;
            return DamageResult.Success;
        }

        public virtual void Death()
        {
            collider.enabled = false;
            rigidbody.simulated = false;
            OnDeath?.Invoke();
        }

        public float GetNormalizedHorizontal() => Mathf.Abs(rigidbody.velocity.x) / Stats.MaxSpeed;

        public void ActivateBuff(float buffDuration)
        {
            CanBuff = false;
            SetBuffState(true);
            StartCoroutine(BuffDeactivateAsync(buffDuration));
        }

        private IEnumerator BuffDeactivateAsync(float buffDuration)
        {
            yield return new WaitForSeconds(buffDuration);
            SetBuffState(false);
        }

        private void SetBuffState(bool state)
        {
            IsBuffActive = state;
            OnBuffStateChanged?.Invoke(IsBuffActive);
        }

        #region Debug

        private void OnDrawGizmos()
        {
            if (!debug) return;
            stats ??= ScriptableObject.CreateInstance<EnemyStats>();
            if (collider == null) collider = GetComponent<CapsuleCollider2D>();

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
                new Vector3(collider.bounds.min.x - Stats.FootPositionOffset.x, position.y);
            Gizmos.DrawLine(leftFootPosition + offset, leftFootPosition + offset + distance);
            // Right Foot.
            var rightFootPosition =
                new Vector3(collider.bounds.max.x + Stats.FootPositionOffset.x, position.y);
            Gizmos.DrawLine(rightFootPosition + offset, rightFootPosition + offset + distance);
        }

        private void WallRays()
        {
            float horizontal = FacingLeft ? collider.bounds.min.x : collider.bounds.max.x;
            horizontal += Stats.WallDetection.HorizontalOffset;
            var direction = FacingLeft ? Vector2.left : Vector2.right;
            // Top Ray.
            Vector2 topOrigin = new Vector2(horizontal, collider.bounds.max.y + Stats.WallDetection.TopOffset);
            Gizmos.DrawLine(topOrigin, topOrigin + (direction * Stats.WallDetection.DistanceCheck));
            // Middle Ray.
            var centerOrigin = new Vector2(horizontal, collider.bounds.center.y + Stats.WallDetection.MidOffset);
            Gizmos.DrawLine(centerOrigin, centerOrigin + (direction * Stats.WallDetection.DistanceCheck));
            // Bottom Ray.
            Vector2 bottomOrigin = new Vector2(horizontal, collider.bounds.min.y + Stats.WallDetection.BottomOffset);
            Gizmos.DrawLine(bottomOrigin, bottomOrigin + (direction * Stats.WallDetection.DistanceCheck));
        }

        #endregion
    }
}