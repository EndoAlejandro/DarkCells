using System;
using System.Collections;
using DarkHavoc.CustomUtils;
using DarkHavoc.EntitiesInterfaces;
using UnityEngine;

namespace DarkHavoc.Enemies.Colossal
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Colossal : Enemy
    {
        public static event Action<ITakeDamage> OnSpawned; 
        public event Action<bool> OnBuffStateChanged;
        public ColossalStats Stats => stats as ColossalStats;
        public ColossalBoomerangArms BoomerangArms => boomerangArms;
        public EnemyHitBox RangedHitBox => rangedHitBox;
        public EnemyHitBox MeleeHitBox => meleeHitBox;
        public EnemyHitBox BuffHitBox => buffHitBox;

        public override float Damage => 2;
        public float StunTime => 1;
        public bool Grounded => LeftFoot || RightFoot;
        public bool LeftFoot { get; private set; }
        public bool RightFoot { get; private set; }
        public bool CanBuff { get; private set; }
        public bool IsBuffActive { get; private set; }

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
            if (transform.position.y != _initialHeight) transform.position = transform.position.With(y: _initialHeight);
        }

        public void ActivateBuff()
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

        public override void TakeDamage(IDoDamage damageDealer, float damageMultiplier, bool isUnstoppable)
        {
            if (IsBuffActive) return;
            base.TakeDamage(damageDealer, damageMultiplier, isUnstoppable);
            if (Health > 0 && Health % _breakpoint == 0) CanBuff = true;
        }

        #region Physics

        private void FixedUpdate()
        {
            if (!IsAlive) return;

            ApplyVelocity();
        }

        public void ApplyVelocity() => _rigidbody.velocity = _targetVelocity;

        public override void Move(int direction)
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

        public override void Death()
        {
            _collider.enabled = false;
            _rigidbody.simulated = false;
            base.Death();
        }

        public override float GetNormalizedHorizontal() =>
            Mathf.Clamp(_rigidbody.velocity.magnitude, 0f, 1f);

        #endregion
    }
}