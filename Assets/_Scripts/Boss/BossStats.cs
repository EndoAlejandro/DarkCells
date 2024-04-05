using DarkHavoc.Enemies;
using UnityEngine;

namespace DarkHavoc.Boss
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Boss/BossStats", fileName = "BossStats", order = 0)]
    public class BossStats : BaseStats
    {
        [SerializeField] private float buffDuration = 10f;
        [SerializeField] private int breakpointAmount = 4;
        [SerializeField] private Color buffOutlineColor;
        [SerializeField] private float telegraphTime = 1f;
        public float BuffDuration => buffDuration;
        public int BreakpointAmount => breakpointAmount;
        public Color BuffOutlineColor => buffOutlineColor;
        public float TelegraphTime => telegraphTime;
    }
}