using System;
using DarkHavoc.CustomUtils;
using UnityEngine;

namespace DarkHavoc.PlayerComponents
{
    [CreateAssetMenu(menuName = "Custom/PlayerStats", fileName = "NewPlayerStats")]
    public class PlayerStats : ScriptableObject
    {
        [Header("Stats")]
        [SerializeField] private int maxHealth = 10;

        [SerializeField] private float takeDamageTime;

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
        [SerializeField] private float ceilingDistance;

        [Header("Attack")]
        [SerializeField] private float attackMoveVelocity = 0.5f;

        [SerializeField] private int damage;

        [Space] [SerializeField] private float lightAttackTime = .5f;
        [SerializeField] private float lightAttackBuffer = .2f;
        [SerializeField] private float lightComboTime = .3f;
        [Space] [SerializeField] private float heavyAttackTime;
        [Space] [SerializeField] private float blockTime = 1f;
        [SerializeField] private ImpulseAction attackAction;
        [SerializeField] private ImpulseAction parryAction;

        #region Stats

        public int MaxHealth => maxHealth;
        public float TakeDamageTime => takeDamageTime;

        #endregion

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
        public float CeilingDistance => ceilingDistance;

        #endregion

        #region Attack

        public float AttackMoveVelocity => attackMoveVelocity;
        public int Damage => damage;
        public float LightAttackBuffer => lightAttackBuffer;
        public float LightAttackTime => lightAttackTime;
        public float LightComboTime => lightComboTime;
        public float HeavyAttackTime => heavyAttackTime;
        public float BlockTime => blockTime;
        public ImpulseAction AttackAction => attackAction;
        public ImpulseAction ParryAction => parryAction;

        #endregion
    }

    [Serializable]
    public struct ImpulseAction
    {
        [SerializeField] private float time;
        [SerializeField] private float force;
        [SerializeField] private float deceleration;
        [SerializeField] private ImpulseActionExtensions.ImpulseDirection direction;
        public float Time => time;
        public float Force => force;
        public float Deceleration => deceleration;
        public int Direction => direction == ImpulseActionExtensions.ImpulseDirection.Forward ? 1 : -1;
    }
}