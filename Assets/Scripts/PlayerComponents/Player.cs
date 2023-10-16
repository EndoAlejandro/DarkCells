using System;
using CustomUtils;
using UnityEngine;

namespace PlayerComponents
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerStateMachine))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerStats stats;
        [SerializeField] private CapsuleCollider2D defaultCollider;
        [SerializeField] private CapsuleCollider2D rollCollider;

        public PlayerStats Stats => stats;

        public bool FacingLeft { get; private set; }
        public bool Grounded { get; private set; }
        public bool EndedJumpEarly { get; private set; }
        public bool IsCoyoteAvailable { get; private set; }

        public bool HasBufferedJump => _isBufferedJumpAvailable && _time < _timeJumpWasPressed + stats.JumpBuffer;

        public bool HasBufferedLightAttack =>
            _isBufferedAttackAvailable && _time < _timeAttackWasPressed + stats.LightAttackBuffer;

        private InputReader _input;
        private Rigidbody2D _rigidbody;
        private CapsuleCollider2D _collider;

        private bool _jumpInputDown;
        private bool _isBufferedJumpAvailable;
        private bool _canAirJump;
        private float _timeJumpWasPressed;

        private bool _attackInputDown;
        private bool _isBufferedAttackAvailable = true;
        private float _timeAttackWasPressed;

        private float _time;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _input = GetComponent<InputReader>();
            SetPlayerCollider(true);
        }

        private void Update()
        {
            _time += Time.deltaTime;

            if (_input.Jump)
            {
                _jumpInputDown = true;
                _timeJumpWasPressed = _time;
            }

            if (_input.LightAttack)
            {
                _attackInputDown = true;
                _isBufferedAttackAvailable = true;
                _timeAttackWasPressed = _time;
            }
        }

        public void CustomGravity(ref Vector2 targetVelocity)
        {
            if (Grounded && targetVelocity.y <= 0)
            {
                targetVelocity.y = -stats.DownGravity;
            }
            else
            {
                var upGravity = stats.UpGravity;
                if (upGravity > 0f) upGravity *= 2f;
                targetVelocity.y = Mathf.MoveTowards(targetVelocity.y, -stats.MaxFallSpeed, upGravity * Time.deltaTime);
            }
        }

        public void Attack()
        {
            var horizontalSpeed = _rigidbody.velocity.x * Stats.AttackSpeedConservation;
            ApplyVelocity(_rigidbody.velocity.With(x: horizontalSpeed));

            _attackInputDown = false;
            _isBufferedAttackAvailable = false;
            
            // TODO: Attack hit box.
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

        public void Roll(ref Vector2 targetVelocity)
        {
            var direction = FacingLeft ? -1 : 1;
            targetVelocity.x = Mathf.MoveTowards(targetVelocity.x, direction * stats.RollMaxSpeed,
                stats.RollAcceleration * Time.fixedDeltaTime);
        }

        public void Jump(ref Vector2 targetVelocity)
        {
            if (!Grounded)
            {
                if (_canAirJump) _canAirJump = false;
                else return;
            }

            _jumpInputDown = false;
            EndedJumpEarly = false;
            _isBufferedJumpAvailable = false;
            IsCoyoteAvailable = false;

            targetVelocity.y = stats.JumpForce;
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
                _canAirJump = true;
                IsCoyoteAvailable = true;
                _isBufferedJumpAvailable = true;
                EndedJumpEarly = false;
            }
            else if (Grounded && !groundHit)
            {
                Grounded = false;
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
    }
}