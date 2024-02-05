namespace DarkHavoc.Enemies.SharedStates
{
    public abstract class MultiHitboxState
    {
        public bool FirstHitBoxAvailable { get; private set; }
        public bool SecondHitBoxAvailable { get; private set; }
        public bool ThirdHitBoxAvailable { get; private set; }

        private readonly EnemyHitBox _firstHitBox;
        private readonly EnemyHitBox _secondHitBox;
        private readonly EnemyHitBox _thirdHitBox;

        protected MultiHitboxState(EnemyHitBox firstHitBox = null, EnemyHitBox secondHitBox = null,
            EnemyHitBox thirdHitBox = null)
        {
            _firstHitBox = firstHitBox;
            _secondHitBox = secondHitBox;
            _thirdHitBox = thirdHitBox;
        }

        protected void HitBoxCheck()
        {
            // TODO: Add visibility check for hitbox.
            FirstHitBoxAvailable = _firstHitBox != null && _firstHitBox.IsPlayerInRange();
            SecondHitBoxAvailable = _secondHitBox != null && _secondHitBox.IsPlayerInRange();
            ThirdHitBoxAvailable = _thirdHitBox != null && _thirdHitBox.IsPlayerInRange();
        }
    }
}