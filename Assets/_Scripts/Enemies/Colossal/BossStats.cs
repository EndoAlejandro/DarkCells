using UnityEngine;

namespace DarkHavoc.Enemies.Colossal
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Boss/BossStats", fileName = "BossStats", order = 0)]
    public class BossStats : BaseStats
    {
        [SerializeField] private float buffDuration = 10f;
        [SerializeField] private int breakpointAmount = 4;
        [SerializeField] private Color buffOutlineColor;
        public float BuffDuration => buffDuration;
        public int BreakpointAmount => breakpointAmount;
        public Color BuffOutlineColor => buffOutlineColor;
    }
}