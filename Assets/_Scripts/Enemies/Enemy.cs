using System;
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
        private Collider2D[] _results;
        protected Vector2 targetVelocity;

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

        public void Move(int direction)
        {
            if (direction == 0)
            {
                var deceleration = Stats.Acceleration * 2;
                targetVelocity.x = Mathf.MoveTowards(targetVelocity.x, 0f, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                var speed = Player == null ? Stats.PatrolSpeed : Stats.MaxSpeed;
                targetVelocity.x = Mathf.MoveTowards(targetVelocity.x, direction * speed,
                    Stats.Acceleration * Time.fixedDeltaTime);
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
            if (tempPlayer == null) return;

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

        public virtual void TakeDamage(IDoDamage damageDealer, float damageMultiplier, bool isUnstoppable)
        {
            if (!IsAlive) return;

            if (damageDealer.transform.TryGetComponent(out Player player)) Player = player;
            Health = Mathf.Max(Health - damageDealer.Damage, 0f);
            OnDamageTaken?.Invoke();
        }

        public virtual void Death()
        {
            collider.enabled = false;
            rigidbody.simulated = false;
            OnDeath?.Invoke();
        }

        public float GetNormalizedHorizontal() => Mathf.Abs(rigidbody.velocity.x) / Stats.MaxSpeed;
    }
}