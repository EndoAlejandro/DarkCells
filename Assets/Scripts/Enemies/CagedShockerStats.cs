using UnityEngine;

namespace Enemies
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

        #region Stats

        public int MaxHealth => maxHealth;

        #endregion

        #region Movement

        public float IdleTime => idleTime;

        public float Gravity => gravity;

        public float Acceleration => acceleration;

        public float MaxFallSpeed => maxFallSpeed;

        public float MaxSpeed => maxSpeed;

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

        #endregion
    }
}