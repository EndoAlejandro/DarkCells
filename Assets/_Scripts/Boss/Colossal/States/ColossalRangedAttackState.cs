using DarkHavoc.Boss.SharedStates;
using DarkHavoc.Enemies;
using DarkHavoc.Fx;
using DarkHavoc.ServiceLocatorComponents;
using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Boss.Colossal.States
{
    public class ColossalRangedAttackState : BossAttackState
    {
        public ColossalRangedAttackState(Boss boss, BossAnimation animation, EnemyHitBox hitBox, AnimationState animationState, float offset = 0, bool useTelegraph = true) : base(boss, animation, hitBox, animationState, offset, useTelegraph)
        {
        }
        
        protected override void AnimationOnAttackPerformed()
        {
            base.AnimationOnAttackPerformed();
            ServiceLocator.GetService<FxManager>()?.PlayFx(BossFx.ColossalRanged, boss.transform.position);
        }
    }
}