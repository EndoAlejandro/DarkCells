using UnityEngine;

namespace DarkHavoc.Enemies.SharedStates
{
    public class EnemyDisplaceAttackState : EnemyAttackState
    {
        public override AnimationState AnimationState => AnimationState.HeavyAttack;
        private readonly Assassin.Assassin _assassin;
        private readonly Collider2D _collider;

        private bool _canMove;
        private int _initialLayer;

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

            _initialLayer = _assassin.gameObject.layer;
            // _assassin.gameObject.layer = LayerMask.NameToLayer("IgnoreEnemy");
            Physics2D.IgnoreCollision(_collider, _assassin.Player.Collider, true);
        }

        public override void OnExit()
        {
            base.OnExit();
            _canMove = false;
            _assassin.ResetVelocity();

            // _assassin.gameObject.layer = _initialLayer;
            Physics2D.IgnoreCollision(_collider, _assassin.Player.Collider, false);
        }
    }
}