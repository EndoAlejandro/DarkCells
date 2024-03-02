using System;
using System.Collections;
using DarkHavoc.CustomUtils;
using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.PlayerComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.Colossal
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Colossal : MonoBehaviour, IDoDamage, ITakeDamage, IEnemy
    {
        public static event Action<ITakeDamage> OnSpawned;
        public event Action<bool> OnBuffStateChanged;
        public ColossalStats Stats => stats as ColossalStats;
        public ColossalBoomerangArms BoomerangArms => boomerangArms;
        public EnemyHitBox RangedHitBox => rangedHitBox;
        public EnemyHitBox MeleeHitBox => meleeHitBox;
        public EnemyHitBox BuffHitBox => buffHitBox;
        public event Action OnDamageTaken;
        public event Action OnDeath;
        public event Action<bool> OnXFlipped;
        public float Health { get; protected set; } = 1;
        public float MaxHealth { get; protected set; }
        public bool IsAlive => Health > 0f;
        public bool FacingLeft { get; private set; }
        public float Damage => 2;
        public float StunTime => 1;
        public bool Grounded => LeftFoot || RightFoot;
        public bool LeftFoot { get; private set; }
        public bool RightFoot { get; private set; }
        public bool CanBuff { get; private set; }
        public bool IsBuffActive { get; private set; }
        public Player Player { get; private set; }
        public Transform MidPoint => midPoint;

        [SerializeField] private ColossalStats stats;
        [SerializeField] private Transform midPoint;

        [Header("Attacks")]
        [SerializeField] private ColossalBoomerangArms boomerangArms;

        [SerializeField] private EnemyHitBox rangedHitBox;
        [SerializeField] private EnemyHitBox meleeHitBox;
        [SerializeField] private EnemyHitBox buffHitBox;

        [SerializeField] private int breakpointAmount = 4;

        private Collider2D _collider;
        private Rigidbody2D _rigidbody;

        private Vector2 _targetVelocity;
        private float _initialHeight;
        private float _breakpoint;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();

            _collider.enabled = false;
            _rigidbody.isKinematic = true;
        }

        public void Setup()
        {
            Health = 100f;
            MaxHealth = Health;
            _breakpoint = MaxHealth / breakpointAmount;

            _rigidbody.isKinematic = false;
            _collider.enabled = true;

            OnSpawned?.Invoke(this);
        }

        private void Start() => _initialHeight = transform.position.y;

        private void Update()
        {
            if (transform.position.y != _initialHeight)
            {
                transform.position = transform.position.With(y: _initialHeight);
            }
        }

        public void ActivateBuff(float buffDuration)
        {
            CanBuff = false;
            SetBuffState(true);
            StartCoroutine(BuffDeactivateAsync());
        }

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

        public void ApplyVelocity() => _rigidbody.velocity = _targetVelocity;

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
            _collider.enabled = false;
            _rigidbody.simulated = false;
            OnDeath?.Invoke();
        }

        public float GetNormalizedHorizontal() => Mathf.Clamp(_rigidbody.velocity.magnitude, 0f, 1f);

        #endregion
    }
}