namespace DarkHavoc.Enemies.DarkWarden
{
    public class DarkWardenAnimation : EnemyAnimation
    {
        private Enemy _enemy;
        protected override float NormalizedHorizontal => _enemy != null ? _enemy.GetNormalizedHorizontal() : 0f;

        protected override void Awake()
        {
            base.Awake();
            _enemy = GetComponentInParent<Enemy>();
        }
    }
}