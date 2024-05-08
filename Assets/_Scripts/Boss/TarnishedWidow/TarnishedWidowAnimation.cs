using DarkHavoc.CustomUtils;
using UnityEngine;

namespace DarkHavoc.Boss.TarnishedWidow
{
    public class TarnishedWidowAnimation : BossAnimation
    {
        private static readonly int ShowOutline = Shader.PropertyToID("_ShowOutline");

        protected override void OnEnable()
        {
            base.OnEnable();
            boss.OnBuffStateChanged += BossOnBuffStateChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            boss.OnBuffStateChanged -= BossOnBuffStateChanged;
        }

        private void BossOnBuffStateChanged(bool state)
        {
            renderer.GetPropertyBlock(materialPb);
            materialPb.SetColor(OutlineColorID, state ? boss.Stats.BuffOutlineColor : Constants.EnemyOutlineColor);
            // materialPb.SetFloat(ShowOutline, state ? 1f : 0f);
            renderer.SetPropertyBlock(materialPb);
        }
    }
}