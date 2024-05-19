using System;
using System.Collections;
using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.PlayerComponents;
using UnityEngine;

namespace DarkHavoc.Boss
{
    public abstract class Boss : MonoBehaviour, IDoDamage, ITakeDamage, IEnemy
    {
        public static event Action<ITakeDamage> OnSpawned;
        public event Action<bool> OnBuffStateChanged;
        public event Action OnDamageTaken;
        public event Action<ITakeDamage> OnDeath;
        public event Action<bool> OnXFlipped;
        public BossStats Stats => stats;
        
        public Transform MidPoint => midPoint;
        public Player Player { get; private set; }
        public float Health { get; protected set; } = 1;
        public float MaxHealth { get; protected set; }
        public float Damage => 2;
        public bool IsAlive => Health > 0f;
        public bool FacingLeft { get; private set; }
        public float StunTime => 1;
        public bool Grounded => LeftFoot || RightFoot;
        public bool LeftFoot { get; private set; }
        public bool RightFoot { get; private set; }
        public bool CanBuff { get; private set; }
        public bool IsBuffActive { get; private set; }

        [SerializeField] private BossStats stats;
        [SerializeField] private Transform midPoint;

        [Header("Attacks")]
        [SerializeField] private int breakpointAmount = 4;

        protected new Collider2D collider;
        protected new Rigidbody2D rigidbody;

        private Vector2 _targetVelocity;
        private float _breakpoint;

        private void Awake()
        {
            collider = GetComponent<Collider2D>();
            rigidbody = GetComponent<Rigidbody2D>();

            collider.enabled = false;
            rigidbody.isKinematic = true;
        }

        public void Setup()
        {
            Health = Stats.MaxHealth;
            MaxHealth = Stats.MaxHealth;
            _breakpoint = MaxHealth / breakpointAmount;

            rigidbody.isKinematic = false;
            collider.enabled = true;

            OnSpawned?.Invoke(this);
        }

        public void ActivateBuff(float buffDuration)
        {
            CanBuff = false;
            SetBuffState(true);
            StartCoroutine(BuffDeactivateAsync());
        }

        public void ForceBuffDeactivation() => SetBuffState(false);

        private IEnumerator BuffDeactivateAsync()
        {
            yield return new WaitForSeconds(Stats.BuffDuration);
            SetBuffState(false);
        }

        private void SetBuffState(bool state)
        {
            IsBuffActive = state;
            OnBuffStateChanged?.Invoke(IsBuffActive);
        }

        public DamageResult TakeDamage(IDoDamage damageDealer, float damageMultiplier, bool isUnstoppable)
        {
            if (IsBuffActive) return DamageResult.Blocked;
            if (!IsAlive) return DamageResult.Killed;

            Health = Mathf.Max(Health - damageDealer.Damage, 0f);
            OnDamageTaken?.Invoke();

            if (Health > 0 && Health % _breakpoint == 0) CanBuff = true;
            return DamageResult.Success;
        }

        public void SetFacingLeft(bool facingLeft)
        {
            FacingLeft = facingLeft;
            OnXFlipped?.Invoke(FacingLeft);
        }

        public void DoDamage(ITakeDamage takeDamage, float damageMultiplier = 1, bool unstoppable = false) =>
            takeDamage.TakeDamage(this, damageMultiplier, unstoppable);

        #region Physics

        private void FixedUpdate()
        {
            if (!IsAlive) return;

            ApplyVelocity();
        }

        public void ApplyVelocity() => rigidbody.velocity = _targetVelocity;

        public void Move(int direction)
        {
            if (direction == 0)
            {
                var deceleration = stats.Acceleration;
                _targetVelocity.x = Mathf.MoveTowards(_targetVelocity.x, 0f, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _targetVelocity.x = Mathf.MoveTowards(_targetVelocity.x, direction * stats.MaxSpeed,
                    stats.Acceleration * Time.fixedDeltaTime);
            }
        }

        public void Death()
        {
            collider.enabled = false;
            rigidbody.simulated = false;
            OnDeath?.Invoke(this);
        }

        public float GetNormalizedHorizontal() => Mathf.Clamp(rigidbody.velocity.magnitude, 0f, 1f);

        #endregion
    }
}