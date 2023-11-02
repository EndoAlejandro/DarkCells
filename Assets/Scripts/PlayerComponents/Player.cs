using System;
using System.Collections;
using DarkHavoc.AttackComponents;
using DarkHavoc.CustomUtils;
using DarkHavoc.ImpulseComponents;
using DarkHavoc.PlayerComponents.PlayerActions;
using UnityEngine;

namespace DarkHavoc.PlayerComponents
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerStateMachine))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Player : MonoBehaviour, IDoDamage, IEntity, ITakeDamage
    {
        public event Func<Vector2, bool> TryToBlockDamage;
        public event Action<bool> OnGroundedChanged;
        public event Action OnDamageTaken;
        public bool HasBufferedJump => _jumpAction is { IsAvailable: true };
        public bool HasBufferedAttack => _attackAction is { IsAvailable: true };
        public bool HasBufferedRoll => _rollAction is { IsAvailable: true };

        public bool FacingLeft { get; private set; }
        public bool Grounded { get; private set; }
        public Vector3 MidPoint => midPoint.position;
        public int Direction => FacingLeft ? -1 : 1;
        public float Damage => Stats != null ? Stats.Damage : 0f;
        public float Health { get; private set; }
        public bool IsAlive => Health > 0f;

        [SerializeField] private PlayerStats stats;

        [SerializeField] private Transform attackOffset;
        [SerializeField] private Transform midPoint;

        [SerializeField] private CapsuleCollider2D defaultCollider;
        [SerializeField] private CapsuleCollider2D rollCollider;

        private Vector2 _targetVelocity;
        
        private Rigidbody2D _rigidbody;
        private CapsuleCollider2D _collider;

        private InputReader _inputReader;

        private AttackAction _attackAction;
        private JumpAction _jumpAction;
        private BufferedAction _rollAction;

        private IEnumerator _currentImpulse;

        public PlayerStats Stats => stats;
        

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _inputReader = GetComponent<InputReader>();

            Health = Stats.MaxHealth;
            Actions();
            SetPlayerCollider(true);
        }

        private void Actions()
        {
            _attackAction = new AttackAction(attackOffset, this, Stats.LightAttackBuffer, () => _inputReader.Attack);
            _jumpAction = new JumpAction(this, _rigidbody, _inputReader, Stats.JumpBuffer, () => _inputReader.Jump);
            _rollAction = new BufferedAction(this, Stats.JumpBuffer, () => _inputReader.Roll);
        }

        private void Update()
        {
            _attackAction.Tick();
            _jumpAction.Tick();
            _rollAction.Tick();
        }

        public void Roll() => _rollAction.UseAction();
        public void Attack(AttackImpulseAction attackImpulse) => _attackAction.UseAction(attackImpulse);
        public void Jump(ref Vector2 targetVelocity) => _jumpAction.UseAction(ref targetVelocity);

        public void CustomGravity(ref Vector2 targetVelocity)
        {
            if (Grounded && targetVelocity.y <= 0)
            {
                targetVelocity.y = -stats.GroundingForce;
            }
            else
            {
                var inAirGravity = stats.FallAcceleration;
                if (_jumpAction.EndedJumpEarly && inAirGravity > 0f) inAirGravity *= Stats.JumpEndEarlyGravityModifier;
                targetVelocity.y = Mathf.MoveTowards(targetVelocity.y, -stats.MaxFallSpeed,
                    inAirGravity * Time.fixedDeltaTime);
            }
        }

        public void Move(ref Vector2 targetVelocity, float input)
        {
            if (input == 0)
            {
                var deceleration = Grounded ? stats.GroundDeceleration : stats.AirDeceleration;
                targetVelocity.x = Mathf.MoveTowards(targetVelocity.x, 0f, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                targetVelocity.x = Mathf.MoveTowards(targetVelocity.x, input * stats.MaxSpeed,
                    stats.Acceleration * Time.fixedDeltaTime);
            }
        }

        public bool CheckCeilingCollision() => CheckCollisionCustomDirection(Vector2.up, Stats.CeilingDistance);

        public void CheckCollisions(ref Vector2 targetVelocity)
        {
            Physics2D.queriesStartInColliders = false;

            bool groundHit = CheckCollisionCustomDirection(Vector2.down, stats.GrounderDistance);
            bool ceilingHit = CheckCollisionCustomDirection(Vector2.up, stats.GrounderDistance);

            if (ceilingHit) targetVelocity.y = Mathf.Min(0, targetVelocity.y);

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
            _collider.bounds.center,
            _collider.size, _collider.direction, 0f, direction, distance, ~stats.Layer);

        public void ApplyVelocity(Vector2 targetVelocity)
        {
            Vector2 finalVelocity =
                new Vector2(Mathf.Abs(_horizontalOverride) > 0.02f ? _horizontalOverride : targetVelocity.x,
                    targetVelocity.y);
            _rigidbody.velocity = finalVelocity;
        }

        public void SetFacingLeft(bool value) => FacingLeft = value;

        public float GetNormalizedHorizontal() =>
            Mathf.Abs(_rigidbody.velocity.x) / (stats == null ? 1 : stats.MaxSpeed);

        public float GetNormalizedVertical() => _rigidbody.velocity.y;

        public void SetPlayerCollider(bool setToDefault)
        {
            _collider = setToDefault ? defaultCollider : rollCollider;
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
            var result = TryToBlockDamage?.Invoke(damageDealer.transform.position) ?? false;
            if (result || !IsAlive) return;

            Health -= damageDealer.Damage * damageMultiplier;

            if (_currentImpulse != null) StopCoroutine(_currentImpulse);
            _currentImpulse = ImpulseActionAsync(Stats.TakeDamageAction);
            StartCoroutine(_currentImpulse);

            OnDamageTaken?.Invoke();
        }

        private float _horizontalOverride = 0f;

        private IEnumerator ImpulseActionAsync(ImpulseAction action)
        {
            float x = action.GetTargetVelocity(Direction);
            while (Mathf.Abs(x) > 0.02f)
            {
                x = action.Decelerate(x, Time.deltaTime);
                _horizontalOverride = x;
                yield return null;
            }
        }

        public void Death()
        {
            Debug.Log("Player Dead.");
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            if (stats == null) return;
            if (_collider == null) _collider = GetComponent<CapsuleCollider2D>();

            Gizmos.DrawLine(_collider.bounds.max, _collider.bounds.max + Vector3.up * stats.GrounderDistance);
            Gizmos.DrawLine(_collider.bounds.min, _collider.bounds.min + Vector3.down * stats.GrounderDistance);
        }
    }
}