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
        [SerializeField] private float jumpBuffer = 0.2f;
        [SerializeField] private float maxFallSpeed;

        [Header("Roll")]
        [SerializeField] private float rollMaxSpeed;

        [SerializeField] private float rollAcceleration;
        [Range(0f, 1f)] [SerializeField] private float rollSpeedConservation;
        [SerializeField] private float rollTime;

        [Header("Attack")]
        [Range(0f, 1f)] [SerializeField] private float attackSpeedConservation = .1f;

        [Space] [SerializeField] private float lightAttackTime = .5f;
        [SerializeField] private float lightAttackBuffer = .2f;
        [SerializeField] private float lightComboTime = .3f;

        [Space] [SerializeField] private float heavyAttackTime;
        [SerializeField] private float heavyAttackBuffer = .2f;


        #region Movement

        public LayerMask Layer => layer;
        public float MaxSpeed => maxSpeed;
        public float Acceleration => acceleration;
        public float GroundDeceleration => groundDeceleration;
        public float AirDeceleration => airDeceleration;
        public float GrounderDistance => grounderDistance;

        #endregion

        #region Jump

        public float JumpForce => jumpForce;
        public float DownGravity => downGravity;
        public float UpGravity => upGravity;
        public float MaxFallSpeed => maxFallSpeed;
        public double JumpBuffer => jumpBuffer;

        #endregion

        #region Roll

        public float RollMaxSpeed => rollMaxSpeed;
        public float RollAcceleration => rollAcceleration;
        public float RollSpeedConservation => rollSpeedConservation;
        public float RollTime => rollTime;

        #endregion

        #region Attack

        public float AttackSpeedConservation => attackSpeedConservation;
        public float LightAttackBuffer => lightAttackBuffer;
        public float HeavyAttackBuffer => heavyAttackBuffer;
        public float LightAttackTime => lightAttackTime;
        public float LightComboTime => lightComboTime;
        public float HeavyAttackTime => heavyAttackTime;

        #endregion
    }
}