using DarkHavoc.Boss.SharedStates;
using DarkHavoc.Enemies;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Boss.HeartHoarder.States
{
    public class HeartHoarderAirAttackState : BossAttackState
    {
        private readonly HeartHoarder _heartHoarder;
        private readonly CompositeHitBox _compositeHitBox;

        public HeartHoarderAirAttackState(HeartHoarder heartHoarder, BossAnimation animation,
            CompositeHitBox compositeHitBox,
            AnimationState animationState,
            float offset) : base(heartHoarder, animation, compositeHitBox, animationState, offset)
        {
            _heartHoarder = heartHoarder;
            _compositeHitBox = compositeHitBox;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            // TODO: Expose the middle 
            _compositeHitBox.ResetIndex();
            _heartHoarder.Teleport(new Vector3(0f, _heartHoarder.transform.position.y, 0f));
        }

        protected override void AnimationOnAttackPerformed()
        {
            _heartHoarder.SetCollisions(true);
            _compositeHitBox.TryToAttacks(_heartHoarder.IsBuffActive);
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}