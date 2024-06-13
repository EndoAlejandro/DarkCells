using DarkHavoc.CustomUtils;
using UnityEngine;

namespace DarkHavoc.Enemies.OrbMage
{
    public class OrbMageAnimation : EnemyAnimation
    {
        private static readonly int ShowOutline = Shader.PropertyToID("_ShowOutline");

        private void Start() => enemy.OnBuffStateChanged += EnemyOnBuffStateChanged;

        private void EnemyOnBuffStateChanged(bool state)
        {
            renderer.GetPropertyBlock(materialPb);
            materialPb.SetColor(OutlineColorID, state ? enemy.Stats.BuffColor : Constants.EnemyOutlineColor);
            materialPb.SetFloat(ShowOutline, state ? 1f : 0f);
            // materialPb.SetFloat(ShowOutline, 1f);
            renderer.SetPropertyBlock(materialPb);
        }

        private void OnDestroy() => enemy.OnBuffStateChanged -= EnemyOnBuffStateChanged;
    }
}