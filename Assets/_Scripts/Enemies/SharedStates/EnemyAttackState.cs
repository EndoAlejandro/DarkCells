using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Enemies.SharedStates
{
    public class EnemyAttackState : IState
    {
        public override string ToString() => "Attack";
        public virtual AnimationState AnimationState => AnimationState.LightAttack;
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

            _hitbox.SetUnstoppable(_isUnstoppable);
        }

        public void Tick()
        {
        }

        public virtual void FixedTick() => _enemy.Move(0);

        public virtual void OnEnter()
        {
            Ended = false;
            _animation.OnAttackPerformed += AnimationOnAttackPerformed;
            _animation.OnAttackEnded += AnimationOnAttackEnded;
        }

        protected virtual void AnimationOnAttackEnded() => Ended = true;

        protected virtual void AnimationOnAttackPerformed()
        {
            var result = _hitbox.TryToAttack(_isUnstoppable);
            if (result == DamageResult.Blocked) Ended = true;
        }

        public virtual void OnExit()
        {
            _animation.OnAttackEnded -= AnimationOnAttackEnded;
            _animation.OnAttackPerformed -= AnimationOnAttackPerformed;
        }
    }
}