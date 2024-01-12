using UnityEngine;

namespace DarkHavoc.Enemies.CagedShocker
{
    public class CagedShockerAnimation : EnemyAnimation
    {
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private CagedShocker _cagedShocker;

        protected override void Awake()
        {
            base.Awake();
            _cagedShocker = GetComponentInParent<CagedShocker>();
        }

        private void Update() =>
            animator.SetFloat(Horizontal, Mathf.Abs(_cagedShocker.GetNormalizedHorizontal()));

        protected override void OnEnable()
        {
            base.OnEnable();
            _cagedShocker.OnXFlipped += EnemyOnXFlipped;
            _cagedShocker.OnDamageTaken += CagedShockerOnDamageTaken;
        }

        private void EnemyOnXFlipped(bool facingLeft) => renderer.flipX = _cagedShocker.FacingLeft;
        private void CagedShockerOnDamageTaken() => PlayHitAnimation();

        protected override void OnDisable()
        {
            base.OnDisable();
            _cagedShocker.OnXFlipped -= EnemyOnXFlipped;
            _cagedShocker.OnDamageTaken -= CagedShockerOnDamageTaken;
        }
    }
}