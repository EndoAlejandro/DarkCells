using UnityEngine;

namespace DarkHavoc.Enemies.SharedStates
{
    public class EnemyDisplaceAttackState : EnemyAttackState
    {
        public override AnimationState AnimationState => AnimationState.HeavyAttack;
        private readonly Assassin.Assassin _assassin;
        private readonly Collider2D _collider;

        private bool _canMove;

        public EnemyDisplaceAttackState(Assassin.Assassin enemy, Collider2D collider, EnemyHitBox hitbox,
            EnemyAnimation animation,
            bool isUnstoppable = false) : base(enemy, hitbox, animation, isUnstoppable)
        {
            _assassin = enemy;
            _collider = collider;
        }


        public override void FixedTick()
        {
            if (!_assassin.LedgeInFront && _canMove)
                _assassin.Move(_assassin.FacingLeft ? -8 : 8, _assassin.Stats.Acceleration * 100);
            else
            {
                _assassin.ResetVelocity();
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
            Physics2D.IgnoreCollision(_collider, _assassin.Player.Collider, true);
        }

        public override void OnExit()
        {
            base.OnExit();
            _canMove = false;
            _assassin.ResetVelocity();
            Physics2D.IgnoreCollision(_collider, _assassin.Player.Collider, false);
        }
    }
}