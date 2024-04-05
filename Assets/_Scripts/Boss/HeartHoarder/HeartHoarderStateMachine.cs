using DarkHavoc.Boss.HeartHoarder.States;
using DarkHavoc.Boss.SharedStates;
using DarkHavoc.Enemies.CagedShocker.States;
using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Boss.HeartHoarder
{
    public class HeartHoarderStateMachine : FiniteStateBehaviour
    {
        private HeartHoarder _heartHoarder;
        private CapsuleCollider2D _collider;
        private HeartHoarderAnimation _animation;

        protected override void References()
        {
            _heartHoarder = GetComponent<HeartHoarder>();
            _collider = GetComponent<CapsuleCollider2D>();
            _animation = GetComponentInChildren<HeartHoarderAnimation>();
        }

        protected override void StateMachine()
        {
            var initialDelay = new AnimationOnlyState(1f, AnimationState.None);
            var awake = new AnimationOnlyState(1f, AnimationState.Awake);
            var idle = new BossIdle(_heartHoarder);

            var switcher = new HeartHoarderAttackSwitcherState(_heartHoarder, _collider);
            var walkAttack = new HeartHoarderWalkAttackState(_heartHoarder, _animation, _heartHoarder.WalkAttackHitBox,
                AnimationState.LightAttack, 4f, 5f);

            var telegraph = new HeartHoarderTelegraphState(_heartHoarder, _heartHoarder.MeleeAttackHitBox, 3f);
            var meleeAttack = new BossAttackState(_heartHoarder, _animation, _heartHoarder.MeleeAttackHitBox,
                AnimationState.MeleeAttack, useTelegraph: false);

            var disappear = new AnimationOnlyState(5f, AnimationState.Teleport, onEnterCallback: ActivateBuff);
            var airAttack = new HeartHoarderAirAttackState(_heartHoarder, _animation, _heartHoarder.AirAttackHitBox,
                AnimationState.HeavyAttack, 4f);

            var death = new BossDeathState(_heartHoarder);

            stateMachine.SetState(initialDelay);

            stateMachine.AddTransition(initialDelay, awake, () => initialDelay.Ended);
            stateMachine.AddTransition(awake, idle, () => awake.Ended);
            stateMachine.AddTransition(idle, switcher, () => idle.Ended);

            // Walk Attack transitions.
            stateMachine.AddTransition(switcher, walkAttack, () => switcher.WalkAttackAvailable);
            stateMachine.AddTransition(walkAttack, idle, () => walkAttack.Ended);

            // Melee Attack transitions.
            stateMachine.AddTransition(switcher, telegraph, () => switcher.MeleeAttackAvailable);
            stateMachine.AddTransition(telegraph, meleeAttack, () => telegraph.Ended);
            stateMachine.AddTransition(meleeAttack, idle, () => meleeAttack.Ended);

            // Air Attack transitions.
            stateMachine.AddTransition(switcher, disappear, () => switcher.AirAttackAvailable);
            stateMachine.AddTransition(disappear, airAttack, () => disappear.Ended);
            stateMachine.AddTransition(airAttack, idle, () => airAttack.Ended);

            stateMachine.AddAnyTransition(death, () => !_heartHoarder.IsAlive);
        }

        private void ActivateBuff()
        {
            _heartHoarder.SetCollisions(false);
            // _heartHoarder.ActivateBuff(10f);
        }
    }
}