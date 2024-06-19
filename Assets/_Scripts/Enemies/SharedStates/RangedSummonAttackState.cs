using DarkHavoc.Fx;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Enemies.SharedStates
{
    public class RangedSummonState : EnemyAttackState
    {
        private FxManager _vfx;
        private Vector2 _target;

        public RangedSummonState(Enemy enemy, SummonEnemyHitBox hitbox, EnemyAnimation animation,
            bool isUnstoppable = false, AnimationState animationState = AnimationState.LightAttack) : base(enemy,
            hitbox, animation, isUnstoppable, animationState)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _target = enemy.Player.transform.position + Vector3.up;
            _vfx ??= ServiceLocator.GetService<FxManager>();
            _vfx.PlayFx(isUnstoppable ? EnemyFx.DangerousTelegraph : EnemyFx.Telegraph,
                enemy.transform.position + Vector3.up);
        }


        protected override void AnimationOnAttackPerformed()
        {
            ((SummonEnemyHitBox)hitbox).TryToAttack(_target, isUnstoppable);
        }
    }

    public class RangedSummonAttackState : EnemyAttackState
    {
        private FxManager _vfx;
        private Vector2 _target;

        public RangedSummonAttackState(Enemy enemy, SummonAttackEnemyHitBox hitbox, EnemyAnimation animation,
            bool isUnstoppable = false) : base(enemy,
            hitbox, animation, isUnstoppable, AnimationState.RangedAttack)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _target = enemy.Player.transform.position;
            _vfx ??= ServiceLocator.GetService<FxManager>();
            _vfx.PlayFx(isUnstoppable ? EnemyFx.DangerousTelegraph : EnemyFx.Telegraph, _target);
        }

        protected override void AnimationOnAttackPerformed()
        {
            ((SummonAttackEnemyHitBox)hitbox).TryToAttack(_target, isUnstoppable);
        }
    }
}