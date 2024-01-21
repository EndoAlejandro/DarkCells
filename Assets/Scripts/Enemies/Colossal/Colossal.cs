using DarkHavoc.CustomUtils;
using DarkHavoc.ServiceLocatorComponents;
using DarkHavoc.UI;
using UnityEngine;

namespace DarkHavoc.Enemies.Colossal
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Colossal : Enemy
    {
        public EnemyHitBox RangedHitBox => rangedHitBox;
        public EnemyHitBox MeleeHitBox => meleeHitBox;
        public EnemyHitBox BuffHitBox => buffHitBox;

        public override float Damage => 2;
        public float StunTime => 1;
        public bool Grounded => LeftFoot || RightFoot;
        public bool LeftFoot { get; private set; }
        public bool RightFoot { get; private set; }
        public ColossalStats Stats => stats as ColossalStats;

        [Header("Attacks")]
        [SerializeField] private EnemyHitBox rangedHitBox;

        [SerializeField] private EnemyHitBox meleeHitBox;
        [SerializeField] private EnemyHitBox buffHitBox;

        private Collider2D _collider;
        private Rigidbody2D _rigidbody;

        private Vector2 _targetVelocity;
        private float _initialHeight;

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

            _rigidbody.isKinematic = false;
            _collider.enabled = true;

            ServiceLocator.GetService<HealthUI>().Setup(this);
        }

        private void Start() => _initialHeight = transform.position.y;

        private void Update()
        {
            if (transform.position.y != _initialHeight) transform.position = transform.position.With(y: _initialHeight);
        }

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

        public override float GetNormalizedHorizontal() =>
            Mathf.Clamp(_rigidbody.velocity.magnitude, 0f, 1f);
    }
}