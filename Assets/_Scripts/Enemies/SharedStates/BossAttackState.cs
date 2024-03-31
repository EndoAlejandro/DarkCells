using System;
using DarkHavoc.Fx;
using DarkHavoc.ServiceLocatorComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Enemies.SharedStates
{
    public class BossAttackState : IState
    {
        public override string ToString() => AnimationState.ToString();
        public AnimationState AnimationState { get; protected set; }
        public bool CanTransitionToSelf => false;
        public virtual bool Ended { get; protected set; }

        protected readonly Boss boss;
        protected readonly BossAnimation animation;
        protected readonly EnemyHitBox hitBox;
        protected readonly float offset;

        private readonly float _duration;
        private Action _animationOnAttack;

        public BossAttackState(Boss boss, BossAnimation animation, EnemyHitBox hitBox,
            AnimationState animationState, float offset)
        {
            this.boss = boss;
            this.animation = animation;
            this.hitBox = hitBox;
            AnimationState = animationState;
            this.offset = offset;
        }

        public virtual void Tick()
        {
        }

        public void FixedTick() => boss.Move(0);

        public virtual void OnEnter()
        {
            Ended = false;

            animation.OnAttackPerformed += AnimationOnAttackPerformed;
            animation.OnAttackEnded += AnimationOnAttackEnded;

            FxType fxType = hitBox.IsUnstoppable ? FxType.DangerousTelegraph : FxType.Telegraph;
            ServiceLocator.GetService<FxManager>()
                .PlayFx(fxType, boss.transform.position + Vector3.up * offset, 1.25f);
        }

        protected void AnimationOnAttackEnded() => Ended = true;
        protected virtual void AnimationOnAttackPerformed() => hitBox.TryToAttack(boss.IsBuffActive);

        public virtual void OnExit()
        {
            animation.OnAttackPerformed -= AnimationOnAttackPerformed;
            animation.OnAttackEnded -= AnimationOnAttackEnded;
        }
    }
}