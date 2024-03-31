using DarkHavoc.Boss.SharedStates;
using DarkHavoc.Enemies;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Boss.Colossal.States
{
    public class ColossalBuffState : BossAttackState
    {
        public ColossalBuffState(Colossal boss, BossAnimation animation, EnemyHitBox hitBox, float offset) :
            base(boss, animation, hitBox, AnimationState.BuffAttack, offset)
        {
        }

        protected override void AnimationOnAttackPerformed()
        {
            base.AnimationOnAttackPerformed();
            boss.ActivateBuff(0f);
        }
    }
}