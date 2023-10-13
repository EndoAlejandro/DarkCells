using System;
using CustomUtils;
using UnityEngine;

namespace PlayerComponents
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerStateMachine))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerStats stats;

        private Rigidbody2D _rigidbody;
        public bool Grounded { get; private set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            
        }

        public void CustomGravity(ref Vector2 targetVelocity)
        {
            if (Grounded && targetVelocity.y <= 0)
            {
                targetVelocity.y = stats.DownGravity;
            }
            else
            {
                var upGravity = stats.UpGravity;
                // if(upGravity > 0) upGravity *= early jump cancel.
                targetVelocity.y = Mathf.MoveTowards(targetVelocity.y, -stats.MaxFallSpeed, upGravity * Time.deltaTime);
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
                targetVelocity.x = Mathf.MoveTowards(targetVelocity.x, input * stats.MaxSpeed, stats.Acceleration * Time.fixedDeltaTime);
            }
        }

        public void Jump(ref Vector2 targetVelocity)
        {
            targetVelocity.y = stats.JumpForce;
        }

        public void ApplyVelocity(Vector2 targetVelocity) => _rigidbody.velocity = targetVelocity;
    }
}