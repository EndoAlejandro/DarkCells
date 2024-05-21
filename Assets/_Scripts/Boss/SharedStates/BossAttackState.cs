using System;
using DarkHavoc.Enemies;
using DarkHavoc.Fx;
using DarkHavoc.ServiceLocatorComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Boss.SharedStates
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
        private readonly bool _useTelegraph;

        private readonly float _duration;
        private Action _animationOnAttack;

        public BossAttackState(Boss boss, BossAnimation animation, EnemyHitBox hitBox,
            AnimationState animationState, float offset = 0f, bool useTelegraph = true)
        {
            this.boss = boss;
            this.animation = animation;
            this.hitBox = hitBox;
            AnimationState = animationState;
            this.offset = offset;
            _useTelegraph = useTelegraph;
        }

        public virtual void Tick()
        {
        }

        public virtual void FixedTick() => boss.Move(0);

        public virtual void OnEnter()
        {
            Ended = false;

            animation.OnAttackPerformed += AnimationOnAttackPerformed;
            animation.OnAttackEnded += AnimationOnAttackEnded;

            // TODO: Move all telegraph to its own state.
            if (_useTelegraph)
            {
                /*BossFx fxType = hitBox.IsUnstoppable ? BossFx.DangerousTelegraph : BossFx.Telegraph;
                ServiceLocator.GetService<FxManager>()?
                    .PlayFx(fxType, boss.transform.position + Vector3.up * offset, 1.25f);*/
            }
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