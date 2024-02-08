using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.ServiceLocatorComponents;
using DarkHavoc.StateMachineComponents;
using DarkHavoc.Fx;

namespace DarkHavoc.Enemies.Archer.States
{
    public class ArcherAttackState : EnemyAttackState
    {
        private FxManager _vfx;

        public ArcherAttackState(Enemy enemy, EnemyHitBox hitbox, EnemyAnimation animation, bool isUnstoppable = false,
            AnimationState animationState = AnimationState.LightAttack) : base(enemy, hitbox, animation, isUnstoppable,
            animationState)
        {
        }

        protected override void AnimationOnAttackPerformed()
        {
            base.AnimationOnAttackPerformed();
            _vfx ??= ServiceLocator.GetService<FxManager>();
            if (_vfx == null) return;
            _vfx.PlayFx(FxType.ArcherAttack, enemy.transform.position, flipX: enemy.FacingLeft);
        }
    }
}