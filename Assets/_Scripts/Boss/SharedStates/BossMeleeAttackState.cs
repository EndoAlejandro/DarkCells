using DarkHavoc.Enemies;
using DarkHavoc.Fx;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Boss.SharedStates
{
    public class BossMeleeAttackState : BossAttackState
    {
        public BossMeleeAttackState(Boss boss, BossAnimation animation, EnemyHitBox hitBox, float offset) :
            base(boss, animation, hitBox, AnimationState.MeleeAttack, offset)
        {
        }

        protected override void AnimationOnAttackPerformed()
        {
            base.AnimationOnAttackPerformed();
            // Extra smoke fx - visual only.
            Vector2 position = new Vector2(hitBox.transform.position.x, boss.transform.position.y);
            ServiceLocator.GetService<FxManager>()?.PlayFx(FxType.ColossalMelee, position);
        }
    }
}