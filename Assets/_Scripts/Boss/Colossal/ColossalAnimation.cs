using UnityEngine;

namespace DarkHavoc.Boss.Colossal
{
    public class ColossalAnimation : BossAnimation
    {
        private static readonly int TurnAround = Animator.StringToHash("TurnAround");
        private static readonly int ShowOutline = Shader.PropertyToID("_ShowOutline");
        private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");

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
            materialPb.SetColor(OutlineColor,boss.Stats.BuffOutlineColor);
            materialPb.SetFloat(ShowOutline, state ? 1f : 0f);
            renderer.SetPropertyBlock(materialPb);
        }

        protected override void EnemyOnXFlipped(bool facingLeft)
        {
            base.EnemyOnXFlipped(facingLeft);
            animator.SetTrigger(TurnAround);
        }
    }
}