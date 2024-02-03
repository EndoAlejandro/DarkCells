using System;
using DarkHavoc.Fx;
using DarkHavoc.ServiceLocatorComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.Colossal.States
{
    public abstract class ColossalAttackState : IState
    {
        protected readonly Colossal colossal;
        protected readonly ColossalAnimation animation;
        protected readonly EnemyHitBox hitBox;
        private readonly float _duration;
        private float _timer;
        private Action _animationOnAttack;
        public override string ToString() => "Ranged Attack";
        public abstract AnimationState AnimationState { get; }
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;

        protected ColossalAttackState(Colossal colossal, ColossalAnimation animation, EnemyHitBox hitBox,
            float duration)
        {
            this.colossal = colossal;
            this.animation = animation;
            this.hitBox = hitBox;
            _duration = duration;
        }

        public void Tick() => _timer -= Time.deltaTime;

        public void FixedTick() => colossal.Move(0);

        public virtual void OnEnter()
        {
            FxType fxType = hitBox.IsUnstoppable ? FxType.DangerousTelegraph : FxType.Telegraph;
            ServiceLocator.GetService<FxManager>()
                .GetFx(fxType, colossal.transform.position + Vector3.up * 3.75f, 1.25f);
            
            _timer = _duration;
        }

        protected virtual void AnimationOnAttack() => hitBox.Attack(colossal.IsBuffActive);

        public virtual void OnExit()
        {
        }
    }
}