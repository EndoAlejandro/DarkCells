using UnityEngine;

namespace DarkHavoc.Enemies.Colossal
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Boss/ColossalStats", fileName = "ColossalStats", order = 0)]
    public class ColossalStats : BaseStats
    {
        [SerializeField] private float buffDuration = 10f;
        [SerializeField] private Color buffOutlineColor;
        public float BuffDuration => buffDuration;
        public Color BuffOutlineColor => buffOutlineColor;
    }
}