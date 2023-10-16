using UnityEngine;

namespace PlayerComponents
{
    [CreateAssetMenu(menuName = "Custom/PlayerStats", fileName = "NewPlayerStats")]
    public class PlayerStats : ScriptableObject
    {
        [Header("Movement")]
        [SerializeField] private LayerMask layer;

        [SerializeField] private float maxSpeed;

        [SerializeField] private float acceleration;
        [SerializeField] private float groundDeceleration = 60f;
        [SerializeField] private float airDeceleration = 30f;
        [SerializeField] private float grounderDistance;

        [Header("Jump")]
        [SerializeField] private float jumpForce;

        [SerializeField] private float downGravity;
        [SerializeField] private float upGravity;
        [SerializeField] private float maxFallSpeed;

        [Header("Roll")]
        [SerializeField] private float rollMaxSpeed;

        [SerializeField] private float rollAcceleration;
        [Range(0f, 1f)] [SerializeField] private float rollSpeedConservation;
        [SerializeField] private float rollTime;

        public LayerMask Layer => layer;
        public float MaxSpeed => maxSpeed;
        public float GroundDeceleration => groundDeceleration;
        public float AirDeceleration => airDeceleration;
        public float Acceleration => acceleration;
        public float DownGravity => downGravity;
        public float UpGravity => upGravity;
        public float MaxFallSpeed => maxFallSpeed;
        public float JumpForce => jumpForce;
        public float GrounderDistance => grounderDistance;
        public float RollMaxSpeed => rollMaxSpeed;
        public float RollAcceleration => rollAcceleration;
        public float RollSpeedConservation => rollSpeedConservation;
        public float RollTime => rollTime;
    }
}