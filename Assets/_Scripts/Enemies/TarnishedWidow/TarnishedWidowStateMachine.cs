using DarkHavoc.Enemies.Colossal.States;
using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.Enemies.TarnishedWidow.States;
using DarkHavoc.StateMachineComponents;

namespace DarkHavoc.Enemies.TarnishedWidow
{
    public class TarnishedWidowStateMachine : FiniteStateBehaviour
    {
        private TarnishedWidow _tarnishedWidow;
        private TarnishedWidowAnimation _animation;

        protected override void References()
        {
            _tarnishedWidow = GetComponent<TarnishedWidow>();
            _animation = GetComponentInChildren<TarnishedWidowAnimation>();
        }

        protected override void StateMachine()
        {
            var idle = new BossIdle(_tarnishedWidow);
            var chase = new TarnishedWidowChaseState(_tarnishedWidow, 3.5f);
            var death = new BossDeathState(_tarnishedWidow);

            // Attacks.
            var meleeAttack = new BossMeleeAttackState(_tarnishedWidow, _animation,
                _tarnishedWidow.MeleeHitBox, 2f);
            var rangedAttack = new BossAttackState(_tarnishedWidow, _animation,
                _tarnishedWidow.RangedHitBox, AnimationState.RangedAttack, 2f);
            var buffAttack = new BossAttackState(_tarnishedWidow, _animation,
                _tarnishedWidow.BuffHitBox, AnimationState.BuffAttack, 2f);
            var jumpUp = new TarnishedWidowJumpUpAttackState(_tarnishedWidow, _animation,
                _tarnishedWidow.JumpHitBox, 2f);
            var jumpDown = new TarnishedWidowJumpDownAttackState(_tarnishedWidow, _animation,
                _tarnishedWidow.JumpHitBox, 2f);

            stateMachine.SetState(idle);

            stateMachine.AddTransition(idle, chase, () => idle.Ended);

            stateMachine.AddTransition(chase, jumpUp, () => _tarnishedWidow.CanBuff);
            stateMachine.AddTransition(jumpUp, jumpDown, () => jumpUp.Ended);

            stateMachine.AddTransition(chase, buffAttack, () => chase.BuffAvailable);
            stateMachine.AddTransition(chase, meleeAttack, () => chase.MeleeAvailable);
            stateMachine.AddTransition(chase, rangedAttack, () => chase.Ended && chase.RangedAvailable);

            stateMachine.AddTransition(buffAttack, idle, () => buffAttack.Ended);
            stateMachine.AddTransition(meleeAttack, idle, () => meleeAttack.Ended);
            stateMachine.AddTransition(rangedAttack, idle, () => rangedAttack.Ended);
            stateMachine.AddTransition(jumpDown, idle, () => jumpDown.Ended);

            stateMachine.AddAnyTransition(death, () => !_tarnishedWidow.IsAlive);
        }
    }
}