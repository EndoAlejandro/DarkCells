using DarkHavoc.ImpulseComponents;
using UnityEngine;

namespace DarkHavoc.PlayerComponents
{
    [CreateAssetMenu(menuName = "ScriptableObjects/PlayerStats", fileName = "NewPlayerStats")]
    public class PlayerStats : ScriptableObject
    {
        [Header("Stats")]
        [SerializeField] private int maxHealth = 10;

        [SerializeField] private float takeDamageTime;
        [SerializeField] private float immunityTime = 1f;

        [Header("Movement")]
        [SerializeField] private LayerMask layer;

        [SerializeField] private LayerMask groundLayers;
        [SerializeField] private LayerMask ceilingLayers;

        [SerializeField] private float maxSpeed;

        [SerializeField] private float acceleration;
        [SerializeField] private float groundDeceleration = 60f;
        [SerializeField] private float airDeceleration = 30f;
        [SerializeField] private float grounderDistance;
        [SerializeField] private float ceilingDistance;
        [SerializeField] private WallDetection wallDetection;

        [Header("Jump")]
        [SerializeField] private float jumpForce;

        [SerializeField] private float airJumpForce;
        [SerializeField] private float groundingForce;
        [SerializeField] private float fallAcceleration;
        [SerializeField] private float jumpBuffer = 0.2f;
        [SerializeField] private float maxFallSpeed;
        [SerializeField] private float jumpEndEarlyGravityModifier = 3f;
        [SerializeField] private float coyoteTime = 0.2f;

        [Header("Wall")]
        [SerializeField] private Vector2 wallJumpForce = new Vector2(10f, 13f);

        [SerializeField] private float wallSlideAcceleration = 1f;
        [SerializeField] private float maxWallSlideSpeed = 1f;

        [Header("Roll")]
        [SerializeField] private float crouchCeilingDistance;

        [SerializeField] private float rollCooldown;
        [SerializeField] private float speedBonus;
        [SerializeField] private float speedBonusDeceleration;
        [SerializeField] private float dodgeTickTime = 1f;
        [SerializeField] private ImpulseAction rollAction;

        [Header("Attack")]
        [SerializeField] private LayerMask attackLayerMask;

        [SerializeField] private int damage;
        [Range(0f, 1f)] [SerializeField] private float movementReduction;
        [SerializeField] private float lightAttackBuffer = .2f;
        [SerializeField] private float comboTime = 1f;
        [Space] [SerializeField] private float heavyAttackTime;
        [Space] [SerializeField] private float blockTime = 1f;
        [SerializeField] private AttackImpulseAction lightAttackAction;
        [SerializeField] private AttackImpulseAction heavyAttackAction;
        [SerializeField] private ImpulseAction parryAction;
        [SerializeField] private ImpulseAction takeDamageAction;

        #region Stats

        public int MaxHealth => maxHealth;
        public float TakeDamageTime => takeDamageTime;
        public float ImmunityTime => immunityTime;

        #endregion

        #region Movement

        public LayerMask Layer => layer;
        public LayerMask GroundLayers => groundLayers;
        public LayerMask CeilingLayers => ceilingLayers;
        public float MaxSpeed => maxSpeed;
        public float Acceleration => acceleration;
        public float GroundDeceleration => groundDeceleration;
        public float AirDeceleration => airDeceleration;
        public float GrounderDistance => grounderDistance;
        public float CeilingDistance => ceilingDistance;
        public WallDetection WallDetection => wallDetection;

        #endregion

        #region Jump

        public float JumpForce => jumpForce;
        public float AirJumpForce => airJumpForce;
        public float GroundingForce => groundingForce;
        public float FallAcceleration => fallAcceleration;
        public float MaxFallSpeed => maxFallSpeed;
        public float JumpBuffer => jumpBuffer;
        public float JumpEndEarlyGravityModifier => jumpEndEarlyGravityModifier;
        public float CoyoteTime => coyoteTime;

        #endregion

        #region Wall Slide

        public Vector2 WallJumpForce => wallJumpForce;
        public float WallSlideAcceleration => wallSlideAcceleration;
        public float MaxWallSlideSpeed => maxWallSlideSpeed;

        #endregion

        #region Roll

        public float CrouchCeilingDistance => crouchCeilingDistance;
        public float RollCooldown => rollCooldown;
        public float SpeedBonus => speedBonus;
        public float SpeedBonusDeceleration => speedBonusDeceleration;
        public float DodgeTickTime => dodgeTickTime;
        public ImpulseAction RollAction => rollAction;

        #endregion

        #region Attack

        public LayerMask AttackLayerMask => attackLayerMask;
        public int Damage => damage;
        public float MovementReduction => movementReduction;
        public float LightAttackBuffer => lightAttackBuffer;
        public float ComboTime => comboTime;
        public float BlockTime => blockTime;
        public AttackImpulseAction LightAttackAction => lightAttackAction;
        public AttackImpulseAction HeavyAttackAction => heavyAttackAction;
        public ImpulseAction ParryAction => parryAction;
        public ImpulseAction TakeDamageAction => takeDamageAction;

        #endregion
    }
}