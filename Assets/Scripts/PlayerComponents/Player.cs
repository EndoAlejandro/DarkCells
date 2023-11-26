using System;
using System.Collections;
using DarkHavoc.CustomUtils;
using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.ImpulseComponents;
using DarkHavoc.PlayerComponents.PlayerActions;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerStateMachine))]
    public class Player : MonoBehaviour, IDoDamage, IEntity, ITakeDamage
    {
        // Events
        public static event Action<Player> OnPlayerSpawned;
        public event Func<Vector2, bool> TryToBlockDamage;
        public event Action<bool> OnGroundedChanged;
        public event Action<bool> OnLedgeGrabChanged;
        public event Action<bool> OnWallSlideChanged;
        public event Action OnDamageTaken;

        // Buffered Actions
        public bool HasBufferedJump => _jumpBufferedAction is { IsAvailable: true };
        public bool HasBufferedAttack => _attackBufferedAction is { IsAvailable: true };
        public bool HasBufferedRoll => _rollAction is { IsAvailable: true };
        public bool HasBufferedLedgeGrab => _ledgeGrabAction is { IsAvailable: true };
        public bool HasBufferedBlock => _blockBufferedAction is { IsAvailable: true };

        public bool CanPerformHeavyAttack =>
            _attackBufferedAction != null && _attackBufferedAction.CanPerformHeavyAttack();

        public PlayerStats Stats => stats;
        public Collider2D Collider { get; private set; }
        public bool FacingLeft { get; private set; }
        public bool Grounded { get; private set; }
        public Vector3 MidPoint => midPoint.position;
        public int Direction => FacingLeft ? -1 : 1;
        public float Damage => Stats != null ? Stats.Damage : 0f;
        public float Health { get; private set; }
        public float MaxHealth { get; private set; }
        public bool IsAlive => Health > 0f;

        [SerializeField] private PlayerStats stats;

        [SerializeField] private Transform attackOffset;
        [SerializeField] private Transform midPoint;

        [SerializeField] private Collider2D defaultCollider;
        [SerializeField] private Collider2D rollCollider;

        private Vector2 _targetVelocity;

        private Rigidbody2D _rigidbody;
        private InputReader _inputReader;
        private ImpulseAction _currentImpulseAction;

        private BufferedAction _blockBufferedAction;
        private AttackBufferedAction _attackBufferedAction;
        private JumpBufferedAction _jumpBufferedAction;
        private RollBufferedAction _rollAction;
        private LedgeGrabBufferedAction _ledgeGrabAction;

        private IEnumerator _currentImpulse;
        private IEnumerator _stopMovement;
        private Vector2 _extraForce;

        private float _impulseTimer;
        private bool _useGravity;
        private bool _wallSliding;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _inputReader = ServiceLocator.Instance.GetService<InputReader>();

            Health = Stats.MaxHealth;
            MaxHealth = Stats.MaxHealth;
            Actions();
            SetPlayerCollider(true);
            OnPlayerSpawned?.Invoke(this);
        }

        private void OnEnable() => _useGravity = true;

        private void Actions()
        {
            _blockBufferedAction = new BufferedAction(this,
                0.25f, () => _inputReader.Block);

            _attackBufferedAction = new AttackBufferedAction(attackOffset, this,
                Stats.LightAttackBuffer, () => _inputReader.Attack);

            _jumpBufferedAction = new JumpBufferedAction(this,
                _rigidbody, _inputReader, Stats.JumpBuffer, () => _inputReader.Jump);

            _rollAction = new RollBufferedAction(this,
                Stats.JumpBuffer, () => _inputReader.Roll);

            _ledgeGrabAction = new LedgeGrabBufferedAction(this,
                Stats.JumpBuffer, () => true);
        }

        private void Update()
        {
            if (!IsAlive) return;
            if (_impulseTimer > 0f) _impulseTimer -= Time.deltaTime;

            _attackBufferedAction.Tick();
            _jumpBufferedAction.Tick();
            _rollAction.Tick();
            _ledgeGrabAction.Tick();
            _blockBufferedAction.Tick();
        }

        private void FixedUpdate()
        {
            if (!IsAlive) return;

            CheckCollisions();
            CustomGravity();

            _extraForce.x = _currentImpulseAction?.Decelerate(_extraForce.x, Time.fixedDeltaTime) ?? 0f;

            ApplyVelocity();
        }

        public void AddImpulse(ImpulseAction impulse)
        {
            _currentImpulseAction = impulse;
            _impulseTimer = impulse.Time;
            _extraForce.x = _currentImpulseAction.GetTargetVelocity(Direction);
        }

        public void Roll() => _rollAction.UseAction();
        public void Attack(AttackImpulseAction attackImpulse) => _attackBufferedAction.UseAction(attackImpulse);
        public void Jump() => _jumpBufferedAction.UseAction(ref _targetVelocity);
        public void WallJump() => _jumpBufferedAction.UseWallAction(ref _targetVelocity);
        public void Block() => _blockBufferedAction.UseAction();

        private void CustomGravity()
        {
            if (!_useGravity) return;
            if (_wallSliding)
            {
                var gravity = stats.WallSlideAcceleration;

                _targetVelocity.y = Mathf.MoveTowards(_targetVelocity.y, -stats.MaxWallSlideSpeed,
                    gravity * Time.fixedDeltaTime);
            }
            else if (Grounded && _targetVelocity.y <= 0)
            {
                _targetVelocity.y = -stats.GroundingForce;
            }
            else
            {
                var inAirGravity = stats.FallAcceleration;

                if (_jumpBufferedAction.EndedJumpEarly && inAirGravity > 0f)
                    inAirGravity *= Stats.JumpEndEarlyGravityModifier;
                _targetVelocity.y = Mathf.MoveTowards(_targetVelocity.y, -stats.MaxFallSpeed,
                    inAirGravity * Time.fixedDeltaTime);
            }
        }

        public void Move(float input)
        {
            if (input == 0)
            {
                var deceleration = Grounded ? stats.GroundDeceleration : stats.AirDeceleration;
                _targetVelocity.x = Mathf.MoveTowards(_targetVelocity.x, 0f, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _targetVelocity.x = Mathf.MoveTowards(_targetVelocity.x, input * stats.MaxSpeed,
                    stats.Acceleration * Time.fixedDeltaTime);
            }
        }

        public bool CheckCeilingCollision() => CheckCollisionCustomDirection(Vector2.up, Stats.CeilingDistance);

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            bool groundHit = CheckCollisionCustomDirection(Vector2.down, stats.GrounderDistance);
            bool ceilingHit = CheckCollisionCustomDirection(Vector2.up, stats.GrounderDistance);

            if (ceilingHit) _targetVelocity.y = Mathf.Min(0, _targetVelocity.y);

            if (!Grounded && groundHit)
            {
                Grounded = true;
                OnGroundedChanged?.Invoke(Grounded);
            }
            else if (Grounded && !groundHit)
            {
                Grounded = false;
                OnGroundedChanged?.Invoke(Grounded);
            }
        }

        private bool CheckCollisionCustomDirection(Vector2 direction, float distance) => Physics2D.CapsuleCast(
            Collider.bounds.center,
            Collider.bounds.size, CapsuleDirection2D.Vertical, 0f, direction, distance,
            ~stats.Layer);

        public void ApplyVelocity()
        {
            var target = new Vector2(_impulseTimer <= 0f ? _targetVelocity.x : _extraForce.x, _targetVelocity.y);
            _rigidbody.velocity = target;
        }

        public void SetFacingLeft(bool value) => FacingLeft = value;

        public float GetNormalizedHorizontal() =>
            Mathf.Abs(_rigidbody.velocity.x) / (stats == null ? 1 : stats.MaxSpeed);

        public float GetNormalizedVertical() => _rigidbody.velocity.y;

        public void SetPlayerCollider(bool setToDefault)
        {
            Collider = setToDefault ? defaultCollider : rollCollider;
            if (setToDefault)
            {
                defaultCollider.enabled = true;
                rollCollider.enabled = false;
            }
            else
            {
                defaultCollider.enabled = false;
                rollCollider.enabled = true;
            }
        }

        public void DoDamage(ITakeDamage takeDamage, float damageMultiplier) =>
            takeDamage.TakeDamage(this, damageMultiplier);

        public void TakeDamage(IDoDamage damageDealer, float damageMultiplier = 1f)
        {
            Vector2 source = damageDealer.transform.position;
            bool result = TryToBlockDamage?.Invoke(source) ?? false;

            if (result) TryToStunEnemy(damageDealer);
            if (result || !IsAlive) return;

            Health -= damageDealer.Damage * damageMultiplier;

            AddImpulse(Stats.TakeDamageAction);

            OnDamageTaken?.Invoke();
        }

        private void TryToStunEnemy(IDoDamage damageDealer)
        {
            if (damageDealer.transform.TryGetComponent(out IStunnable stunnable)) stunnable.Stun();
        }

        public void Death() => Debug.Log("Player Dead.");

        public void LedgeGrab(bool value)
        {
            if (value)
                ResetVelocity();
            else
                _ledgeGrabAction.UseAction();

            _useGravity = !value;
            OnLedgeGrabChanged?.Invoke(value);
        }

        public void ResetVelocity() => _targetVelocity = Vector2.zero;

        public void SetWallSliding(bool value)
        {
            _wallSliding = value;
            OnWallSlideChanged?.Invoke(value);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            if (stats == null) return;
            if (Collider == null) Collider = GetComponent<Collider2D>();

            Gizmos.DrawLine(Collider.bounds.max, Collider.bounds.max + Vector3.up * stats.GrounderDistance);
            Gizmos.DrawLine(Collider.bounds.min, Collider.bounds.min + Vector3.down * stats.GrounderDistance);

            WallDetection wallDetection = Stats.WallDetection;

            float horizontal = FacingLeft ? Collider.bounds.min.x : Collider.bounds.max.x;
            float horizontalOffset =
                FacingLeft ? -Stats.WallDetection.HorizontalOffset : Stats.WallDetection.HorizontalOffset;
            horizontal += horizontalOffset;
            Vector2 direction = FacingLeft ? Vector2.left : Vector2.right;

            // Top Ray.
            Vector2 topOrigin = new Vector2(horizontal, Collider.bounds.max.y + wallDetection.TopOffset);
            Gizmos.DrawLine(topOrigin, topOrigin + (direction * wallDetection.DistanceCheck));
            // Middle Ray.
            var centerOrigin = new Vector2(horizontal, Collider.bounds.center.y + wallDetection.MidOffset);
            Gizmos.DrawLine(centerOrigin, centerOrigin + (direction * wallDetection.DistanceCheck));
            // Bottom Ray.
            Vector2 bottomOrigin = new Vector2(horizontal, Collider.bounds.min.y + wallDetection.BottomOffset);
            Gizmos.DrawLine(bottomOrigin, bottomOrigin + (direction * wallDetection.DistanceCheck));

            Gizmos.color = Color.cyan;

            float sphereOffset =
                FacingLeft ? -wallDetection.LedgeDetectorOffset.x : wallDetection.LedgeDetectorOffset.x;
            var spherePosition = new Vector2(sphereOffset + horizontal,
                Collider.bounds.max.y + wallDetection.LedgeDetectorOffset.y);

            Gizmos.DrawWireSphere(spherePosition, wallDetection.LedgeDetectorRadius);
        }
    }
}