using DarkHavoc.Enemies;
using UnityEngine;

namespace DarkHavoc.Boss
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Boss/BossStats", fileName = "BossStats", order = 0)]
    public class BossStats : BaseStats
    {
        [SerializeField] private float buffDuration = 10f;
        [SerializeField] private Color buffOutlineColor;
        [SerializeField] private float telegraphTime = 1f;
        public float BuffDuration => buffDuration;
        public Color BuffOutlineColor => buffOutlineColor;
        public float TelegraphTime => telegraphTime;
    }
}