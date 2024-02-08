using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Enemies.SharedStates
{
    public class EnemyDisplaceAttackState : EnemyAttackState
    {
        public override AnimationState AnimationState => AnimationState.HeavyAttack;
        private readonly Collider2D _collider;

        private bool _canMove;
        private int _initialLayer;

        public EnemyDisplaceAttackState(Enemy enemy, Collider2D collider, EnemyHitBox hitbox,
            EnemyAnimation animation,
            bool isUnstoppable = false) : base(enemy, hitbox, animation, isUnstoppable)
        {
            _collider = collider;
        }


        public override void FixedTick()
        {
            if (!enemy.LedgeInFront && _canMove)
                enemy.Move(enemy.FacingLeft ? -8 : 8, enemy.Stats.Acceleration * 100);
            else
            {
                enemy.ResetVelocity();
            }
        }

        protected override void AnimationOnAttackEnded()
        {
            base.AnimationOnAttackEnded();
            _canMove = false;
        }

        protected override void AnimationOnAttackPerformed()
        {
            base.AnimationOnAttackPerformed();
            _canMove = true;
            _initialLayer = enemy.gameObject.layer;
            Physics2D.IgnoreCollision(_collider, enemy.Player.Collider, true);
        }

        public override void OnExit()
        {
            base.OnExit();
            _canMove = false;
            enemy.ResetVelocity();
            Physics2D.IgnoreCollision(_collider, enemy.Player.Collider, false);
        }
    }
}