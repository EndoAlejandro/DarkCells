using DarkHavoc.ImpulseComponents;
using DarkHavoc.PlayerComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.CagedShocker
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy/CagedShockerStats", fileName = "CagedShockerStats", order = 0)]
    public class CagedShockerStats : EnemyStats
    {
        [Header("Movement")]
        [SerializeField] private float gravity = 3f;
        [SerializeField] private float maxFallSpeed = 5f;
        [SerializeField] private float chaseStoppingDistance = .35f;
        [SerializeField] private float stunTime = 3f;

        [Header("Environment Check")]
        [SerializeField] private LayerMask groundLayerMask;

        [SerializeField] private Vector2 footPositionOffset;
        [SerializeField] private float groundOffset = .35f;
        [SerializeField] private float groundCheckDistance = .05f;

        [SerializeField] private WallDetection wallDetection;

        [Header("Attack")]
        [SerializeField] private int damage = 1;

        [SerializeField] private float firstAttackTime = .11f;
        [SerializeField] private float secondAttackTime = .05f;
        [SerializeField] private float detectionDistance = 7f;
        [SerializeField] private float scapeDistance = 10f;
        [SerializeField] private float comboTime = 0.05f;
        [SerializeField] private float restTime = 1f;
        [SerializeField] private float telegraphTime = 1f;
        
        [SerializeField] private ImpulseAction firstAttackAction;
        [SerializeField] private ImpulseAction secondAttackAction;
        [SerializeField] private ImpulseAction takeDamageAction;

        #region Movement
        
        public float Gravity => gravity;
        public float MaxFallSpeed => maxFallSpeed;
        public float ChaseStoppingDistance => chaseStoppingDistance;
        public float StunTime => stunTime;

        #endregion

        #region Environment Check

        public LayerMask GroundLayerMask => groundLayerMask;
        public Vector2 FootPositionOffset => footPositionOffset;
        public float GroundOffset => groundOffset;
        public float GroundCheckDistance => groundCheckDistance;
        public WallDetection WallDetection => wallDetection;

        #endregion

        #region Attack

        public int Damage => damage;
        public float FirstAttackTime => firstAttackTime;
        public float SecondAttackTime => secondAttackTime;
        public float DetectionDistance => detectionDistance;
        public float ScapeDistance => scapeDistance;
        public float TelegraphTime => telegraphTime;
        public float ComboTime => comboTime;
        public float RestTime => restTime;
        public ImpulseAction FirstAttackAction => firstAttackAction;
        public ImpulseAction SecondAttackAction => secondAttackAction;
        public ImpulseAction TakeDamageAction => takeDamageAction;

        #endregion
    }
}