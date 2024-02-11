using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Enemies.BombDroid
{
    public class EnemyBombAttackState : EnemyAttackState
    {
        private readonly BombDroid _bombDroid;

        public EnemyBombAttackState(BombDroid bombDroid, BombEnemyHitBox hitbox, EnemyAnimation animation,
            bool isUnstoppable = false, AnimationState animationState = AnimationState.LightAttack) : base(bombDroid,
            hitbox, animation, isUnstoppable, animationState)
        {
            _bombDroid = bombDroid;
        }

        public override void FixedTick()
        {
            base.FixedTick();
            _bombDroid.VerticalMove(0f);
        }
    }
}