using DarkHavoc.EntitiesInterfaces;
using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Enemies.SharedStates
{
    public class EnemyAttackState : IState
    {
        public override string ToString() => "Attack";
        public virtual AnimationState AnimationState { get; }
        public bool CanTransitionToSelf => false;
        public bool Blocked { get; private set; }
        public bool Ended { get; private set; }

        protected readonly Enemy enemy;
        protected readonly EnemyHitBox hitbox;
        protected readonly EnemyAnimation animation;
        protected readonly bool isUnstoppable;

        public EnemyAttackState(Enemy enemy, EnemyHitBox hitbox, EnemyAnimation animation, bool isUnstoppable = false,
            AnimationState animationState = AnimationState.LightAttack)
        {
            this.enemy = enemy;
            this.hitbox = hitbox;
            this.animation = animation;
            this.isUnstoppable = isUnstoppable;
            AnimationState = animationState;

            this.hitbox.SetUnstoppable(this.isUnstoppable);
        }

        public void Tick()
        {
        }

        public virtual void FixedTick() => enemy.Move(0);

        public virtual void OnEnter()
        {
            Ended = false;
            Blocked = false;
            animation.OnAttackPerformed += AnimationOnAttackPerformed;
            animation.OnAttackEnded += AnimationOnAttackEnded;
        }

        protected virtual void AnimationOnAttackEnded() => Ended = true;

        protected virtual void AnimationOnAttackPerformed()
        {
            var result = hitbox.TryToAttack(isUnstoppable);
            if (result == DamageResult.Blocked) Blocked = true;
        }

        public virtual void OnExit()
        {
            animation.OnAttackEnded -= AnimationOnAttackEnded;
            animation.OnAttackPerformed -= AnimationOnAttackPerformed;
        }
    }
}