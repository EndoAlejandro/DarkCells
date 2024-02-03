using UnityEngine;

namespace DarkHavoc.Enemies.SharedStates
{
    public class EnemyDisplaceAttackState : EnemyAttackState
    {
        public override AnimationState AnimationState => AnimationState.HeavyAttack;
        private readonly Assassin.Assassin _assassin;

        private bool _canMove;

        public EnemyDisplaceAttackState(Assassin.Assassin enemy, EnemyHitBox hitbox, EnemyAnimation animation,
            bool isUnstoppable = false) : base(enemy, hitbox, animation, isUnstoppable) => _assassin = enemy;


        public override void FixedTick()
        {
            if (!_assassin.LedgeInFront && _canMove)
                _assassin.Move(_assassin.FacingLeft ? -20 : 20, _assassin.Stats.Acceleration * 20);
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
        }

        public override void OnExit()
        {
            base.OnExit();
            _assassin.ResetVelocity();
        }
    }
}