using UnityEngine;

namespace DarkHavoc.Enemies
{
    [CreateAssetMenu(menuName = "Custom/CagedShockerStats", fileName = "NewCagedShockerStats")]
    public class CagedShockerStats : ScriptableObject
    {
        [Header("Stats")]
        [SerializeField] private int maxHealth = 10;

        [Header("Movement")]
        [SerializeField] private float idleTime = 1f;

        [SerializeField] private float gravity = 3f;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float maxFallSpeed = 5f;
        [SerializeField] private float maxSpeed = 1.5f;
        [SerializeField] private float chaseStoppingDistance = .35f;

        [Header("Environment Check")]
        [SerializeField] private LayerMask groundLayerMask;

        [SerializeField] private Vector2 footPositionOffset;
        [SerializeField] private float groundOffset = .35f;
        [SerializeField] private float groundCheckDistance = .05f;
        [SerializeField] private float wallDistanceCheck = .35f;
        [SerializeField] private float wallCheckTopOffset = .3f;
        [SerializeField] private float wallCheckBottomOffset = .3f;

        [Header("Attack")]
        [SerializeField] private int damage = 1;

        [SerializeField] private float firstAttackTime = .11f;
        [SerializeField] private float secondAttackTime = .05f;
        [SerializeField] private float detectionDistance = 7f;
        [SerializeField] private float scapeDistance = 10f;
        [SerializeField] private float comboTime = 0.05f;
        [SerializeField] private float restTime = 1f;
        [SerializeField] private LayerMask attackLayer;

        #region Stats

        public int MaxHealth => maxHealth;

        #endregion

        #region Movement

        public float IdleTime => idleTime;

        public float Gravity => gravity;

        public float Acceleration => acceleration;

        public float MaxFallSpeed => maxFallSpeed;

        public float MaxSpeed => maxSpeed;
        public float ChaseStoppingDistance => chaseStoppingDistance;

        #endregion

        #region Environment Check

        public LayerMask GroundLayerMask => groundLayerMask;

        public Vector2 FootPositionOffset => footPositionOffset;

        public float GroundOffset => groundOffset;

        public float GroundCheckDistance => groundCheckDistance;

        public float WallDistanceCheck => wallDistanceCheck;

        public float WallCheckTopOffset => wallCheckTopOffset;

        public float WallCheckBottomOffset => wallCheckBottomOffset;

        #endregion

        #region Attack

        public int Damage => damage;
        public float FirstAttackTime => firstAttackTime;
        public float SecondAttackTime => secondAttackTime;
        public float DetectionDistance => detectionDistance;
        public float ScapeDistance => scapeDistance;
        public LayerMask AttackLayer => attackLayer;
        public float ComboTime => comboTime;
        public float RestTime => restTime;

        #endregion
    }
}