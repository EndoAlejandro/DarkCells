using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Enemies.CagedSpider
{
    public class EnemyAttackState : IState
    {
        public override string ToString() => "Attack";
        public AnimationState AnimationState => AnimationState.LightAttack;
        public bool CanTransitionToSelf => false;
        public bool Ended { get; private set; }

        private readonly Enemy _enemy;
        private readonly EnemyHitBox _hitbox;
        private readonly EnemyAnimation _animation;
        private readonly bool _isUnstoppable;

        public EnemyAttackState(Enemy enemy, EnemyHitBox hitbox, EnemyAnimation animation, bool isUnstoppable = false)
        {
            _enemy = enemy;
            _hitbox = hitbox;
            _animation = animation;
            _isUnstoppable = isUnstoppable;
        }

        public void Tick()
        {
        }

        public void FixedTick() => _enemy.Move(0);

        public void OnEnter()
        {
            Ended = false;
            _animation.OnAttackPerformed += AnimationOnAttackPerformed;
            _animation.OnAttackEnded += AnimationOnAttackEnded;
        }

        private void AnimationOnAttackEnded() => Ended = true;

        private void AnimationOnAttackPerformed()
        {
            _hitbox.Attack(_isUnstoppable);
        }

        public void OnExit()
        {
            _animation.OnAttackEnded -= AnimationOnAttackEnded;
            _animation.OnAttackPerformed -= AnimationOnAttackPerformed;
        }
    }
}