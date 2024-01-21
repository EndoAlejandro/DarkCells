using DarkHavoc.Fx;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.Colossal
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
            Vector2 position = new Vector2(hitBox.transform.position.x, colossal.transform.position.y);
            ServiceLocator.GetService<FxProvider>().GetFx(FxType.ColossalMelee, position);
        }
    }
}