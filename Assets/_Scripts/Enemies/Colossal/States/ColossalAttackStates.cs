using DarkHavoc.Fx;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Enemies.Colossal.States
{
    public class ColossalRangedAttackState : ColossalAttackState
    {
        public override string ToString() => "Ranged Attack";
        public override AnimationState AnimationState => AnimationState.RangedAttack;

        public ColossalRangedAttackState(Colossal colossal, ColossalAnimation animation, EnemyHitBox hitBox,
            float duration) : base(colossal, animation, hitBox, duration)
        {
        }


        public override void OnEnter()
        {
            base.OnEnter();
            animation.OnRangedAttack += AnimationOnAttack;
        }

        public override void OnExit()
        {
            base.OnExit();
            animation.OnRangedAttack -= AnimationOnAttack;
        }
    }

    public class ColossalMeleeAttackState : ColossalAttackState
    {
        public override string ToString() => "Melee Attack";
        public override AnimationState AnimationState => AnimationState.MeleeAttack;

        public ColossalMeleeAttackState(Colossal colossal, ColossalAnimation animation, EnemyHitBox hitBox,
            float duration) : base(colossal, animation, hitBox, duration)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            animation.OnMeleeAttack += AnimationOnAttack;
        }

        public override void OnExit()
        {
            base.OnExit();
            animation.OnMeleeAttack -= AnimationOnAttack;
        }

        protected override void AnimationOnAttack()
        {
            base.AnimationOnAttack();

            // Extra smoke fx - visual only.
            Vector2 position = new Vector2(hitBox.transform.position.x, colossal.transform.position.y);
            ServiceLocator.GetService<FxManager>().GetFx(FxType.ColossalMelee, position);
        }
    }

    public class ColossalBuffAttackState : ColossalAttackState
    {
        public override string ToString() => "Buff Attack";
        public override AnimationState AnimationState => AnimationState.BuffAttack;

        public ColossalBuffAttackState(Colossal colossal, ColossalAnimation animation, EnemyHitBox hitBox,
            float duration) : base(colossal, animation, hitBox, duration)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            animation.OnBuffAttack += AnimationOnAttack;
        }

        public override void OnExit()
        {
            base.OnExit();
            animation.OnBuffAttack -= AnimationOnAttack;
        }

        protected override void AnimationOnAttack()
        {
            base.AnimationOnAttack();
            colossal.ActivateBuff();
        }
    }
}