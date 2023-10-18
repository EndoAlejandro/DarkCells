using UnityEngine;
using UnityEngine.Serialization;

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

        [SerializeField] private float groundingForce;
        [SerializeField] private float fallAcceleration;
        [SerializeField] private float jumpBuffer = 0.2f;
        [SerializeField] private float maxFallSpeed;
        [SerializeField] private float jumpEndEarlyGravityModifier = 3f;
        [SerializeField] private float coyoteTime = 0.2f;

        [Header("Roll")]
        [SerializeField] private float rollMaxSpeed;

        [SerializeField] private float rollAcceleration;
        [Range(0f, 1f)] [SerializeField] private float rollSpeedConservation;
        [SerializeField] private float rollTime;

        [Header("Attack")]
        [Range(0f, 1f)] [SerializeField] private float attackSpeedConservation = .1f;

        [SerializeField] private int damage;

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
        public float GroundingForce => groundingForce;
        public float FallAcceleration => fallAcceleration;
        public float MaxFallSpeed => maxFallSpeed;
        public float JumpBuffer => jumpBuffer;
        public float JumpEndEarlyGravityModifier => jumpEndEarlyGravityModifier;
        public float CoyoteTime => coyoteTime;

        #endregion

        #region Roll

        public float RollMaxSpeed => rollMaxSpeed;
        public float RollAcceleration => rollAcceleration;
        public float RollSpeedConservation => rollSpeedConservation;
        public float RollTime => rollTime;

        #endregion

        #region Attack

        public float AttackSpeedConservation => attackSpeedConservation;
        public int Damage => damage;
        public float LightAttackBuffer => lightAttackBuffer;
        public float HeavyAttackBuffer => heavyAttackBuffer;
        public float LightAttackTime => lightAttackTime;
        public float LightComboTime => lightComboTime;
        public float HeavyAttackTime => heavyAttackTime;

        #endregion
    }
}