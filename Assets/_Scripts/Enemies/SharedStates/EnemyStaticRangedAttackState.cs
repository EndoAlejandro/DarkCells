using DarkHavoc.Fx;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Enemies.SharedStates
{
    public class EnemyStaticRangedAttackState : EnemyAttackState
    {
        private FxManager _vfx;
        private Vector2 _target;

        public EnemyStaticRangedAttackState(Enemy enemy, StaticRangedEnemyHitBox hitbox, EnemyAnimation animation,
            bool isUnstoppable = false) : base(enemy,
            hitbox, animation, isUnstoppable, AnimationState.RangedAttack)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _target = enemy.Player.transform.position;
            _vfx ??= ServiceLocator.GetService<FxManager>();
            _vfx.PlayFx(isUnstoppable ? FxType.DangerousTelegraph : FxType.Telegraph, _target);
        }

        protected override void AnimationOnAttackPerformed()
        {
            ((StaticRangedEnemyHitBox)hitbox).TryToAttack(_target, isUnstoppable);
        }
    }
}