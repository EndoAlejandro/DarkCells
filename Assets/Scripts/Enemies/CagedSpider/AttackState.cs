using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Enemies.CagedSpider
{
    public class AttackState : IState
    {
        public override string ToString() => "Attack";
        public AnimationState AnimationState => AnimationState.LightAttack;
        public bool CanTransitionToSelf => false;
        public bool Ended { get; private set; }

        private readonly Enemy _enemy;
        private readonly EnemyHitBox _hitbox;
        private readonly EnemyAnimation _animation;

        public AttackState(Enemy enemy,EnemyHitBox hitbox, EnemyAnimation animation)
        {
            _enemy = enemy;
            _hitbox = hitbox;
            _animation = animation;
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
        private void AnimationOnAttackPerformed() => _hitbox.Attack();
        
        public void OnExit()
        {
            _animation.OnAttackEnded -= AnimationOnAttackEnded;
            _animation.OnAttackPerformed -= AnimationOnAttackPerformed;
        }
    }
}