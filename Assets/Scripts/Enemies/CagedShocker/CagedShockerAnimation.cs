namespace DarkHavoc.Enemies.CagedShocker
{
    public class CagedShockerAnimation : EnemyAnimation
    {
        private CagedShocker _cagedShocker;
        protected override float NormalizedHorizontal =>
            _cagedShocker != null ? _cagedShocker.GetNormalizedHorizontal() : 0f;

        protected override void Awake()
        {
            base.Awake();
            _cagedShocker = GetComponentInParent<CagedShocker>();
        }
    }
}