using UnityEngine;

namespace DarkHavoc.Enemies.OrbMage
{
    public class OrbMageAnimation : EnemyAnimation
    {
        private static readonly int ShowOutline = Shader.PropertyToID("_ShowOutline");
        private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");

        private void Start() => enemy.OnBuffStateChanged += EnemyOnBuffStateChanged;

        private void EnemyOnBuffStateChanged(bool state)
        {
            renderer.GetPropertyBlock(materialPb);
            materialPb.SetColor(OutlineColor, enemy.Stats.BuffColor);
            materialPb.SetFloat(ShowOutline, state ? 1f : 0f);
            renderer.SetPropertyBlock(materialPb);
        }

        private void OnDestroy() => enemy.OnBuffStateChanged -= EnemyOnBuffStateChanged;
    }
}