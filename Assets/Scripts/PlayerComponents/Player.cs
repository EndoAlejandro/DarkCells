using System;
using AttackComponents;
using PlayerComponents.PlayerActions;
using UnityEngine;

namespace PlayerComponents
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerStateMachine))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Player : MonoBehaviour, IDoDamage
    {
        public event Action<bool> OnGroundedChanged;

        [SerializeField] private PlayerStats stats;

        [SerializeField] private Transform attackOffset;

        [SerializeField] private CapsuleCollider2D defaultCollider;
        [SerializeField] private CapsuleCollider2D rollCollider;

        private Rigidbody2D _rigidbody;
        private CapsuleCollider2D _collider;

        private InputReader _inputReader;
        private AttackAction _attackAction;
        private JumpAction _jumpAction;
        private RollAction _rollAction;

        public PlayerStats Stats => stats;

        public bool HasBufferedJump => _jumpAction is { IsAvailable: true };
        public bool HasBufferedLightAttack => _attackAction is { IsAvailable: true };

        public bool FacingLeft { get; private set; }
        public bool Grounded { get; private set; }
        public int Damage => Stats != null ? Stats.Damage : 0;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _inputReader = GetComponent<InputReader>();
            Actions();
            SetPlayerCollider(true);
        }

        private void Actions()
        {
            _attackAction = new AttackAction(this, _rigidbody, attackOffset, _inputReader);
            _jumpAction = new JumpAction(this, _rigidbody, _inputReader);
            _rollAction = new RollAction(this, _inputReader);
        }

        private void Update()
        {
            _jumpAction.Tick();
            _rollAction.Tick();
            _attackAction.Tick();
        }

        public void Attack(ref Vector2 targetVelocity) => _attackAction.UseAction(ref targetVelocity);
        public void Jump(ref Vector2 targetVelocity) => _jumpAction.UseAction(ref targetVelocity);
        public void Roll(ref Vector2 targetVelocity) => _rollAction.UseAction(ref targetVelocity);

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

        public void CheckCollisions(ref Vector2 targetVelocity)
        {
            // Physics2D.queriesStartInColliders = false;

            bool groundHit = CheckCollisionCustomDirection(Vector2.down);
            bool ceilingHit = CheckCollisionCustomDirection(Vector2.up);

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

            // Physics2D.queriesStartInColliders = true;
        }

        private bool CheckCollisionCustomDirection(Vector2 direction) => Physics2D.CapsuleCast(_collider.bounds.center,
            _collider.size, _collider.direction, 0f, direction, stats.GrounderDistance, ~stats.Layer);

        public void ApplyVelocity(Vector2 targetVelocity) => _rigidbody.velocity = targetVelocity;

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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            if (stats == null) return;
            if (_collider == null) _collider = GetComponent<CapsuleCollider2D>();

            Gizmos.DrawLine(_collider.bounds.center, _collider.bounds.center + Vector3.down * stats.GrounderDistance);
            Gizmos.DrawLine(_collider.bounds.center, _collider.bounds.center + Vector3.up * stats.GrounderDistance);
        }

        public void DoDamage()
        {
            throw new NotImplementedException();
        }
    }
}