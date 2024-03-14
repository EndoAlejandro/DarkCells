using UnityEngine;
using DarkHavoc.Enemies.CagedShocker.States;
using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.StateMachineComponents;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Enemies.OrbMage
{
    public class OrbMageStateMachine : FiniteStateBehaviour
    {
        private OrbMage _orbMage;
        private Collider2D _collider;
        private EnemyAnimation _animation;

        protected override void References()
        {
            _orbMage = GetComponent<OrbMage>();
            _collider = GetComponent<Collider2D>();
            _animation = GetComponentInChildren<EnemyAnimation>();
        }

        protected override void StateMachine()
        {
            var idle = new IdleState(_orbMage);
            var patrol = new SideToSidePatrolState(_orbMage, _collider);
            var chase = new ChaseSideToSideState(_orbMage, _collider,
                _orbMage.HitBox, _orbMage.RangedHitBox, _orbMage.BuffHitBox);

            var lightTelegraph = new TelegraphState(_orbMage, _orbMage.HitBox, 1f);
            var rangedTelegraph = new TelegraphState(_orbMage, _orbMage.RangedHitBox, 1f);
            var lightAttack = new EnemyAttackState(_orbMage, _orbMage.HitBox, _animation);
            var buffAttack = new EnemyAttackState(_orbMage, _orbMage.BuffHitBox, _animation,
                false, AnimationState.BuffAttack);

            var rangedAttack = new RangedSummonAttackState(_orbMage, _orbMage.RangedHitBox,
                _animation, true);

            var death = new EnemyDeathState(_orbMage);

            stateMachine.SetState(idle);

            stateMachine.AddTransition(idle, patrol, () => idle.Ended && _orbMage.Grounded);
            stateMachine.AddTransition(patrol, idle, () => patrol.Ended || !_orbMage.Grounded);
            stateMachine.AddTransition(patrol, chase, () => _orbMage.Player);

            stateMachine.AddTransition(chase, lightTelegraph,
                () => chase.FirstHitBoxAvailable && chase.IsPlayerVisible);
            stateMachine.AddTransition(lightTelegraph, lightAttack, () => lightTelegraph.Ended);
            stateMachine.AddTransition(lightAttack, idle, () => lightAttack.Ended);

            stateMachine.AddTransition(chase, rangedAttack, () => chase.SecondHitBoxAvailable);
            stateMachine.AddTransition(rangedTelegraph, rangedAttack, () => rangedTelegraph.Ended);
            stateMachine.AddTransition(rangedAttack, idle, () => rangedAttack.Ended);

            stateMachine.AddTransition(chase, buffAttack, () => chase.ThirdHitBoxAvailable && !_orbMage.IsBuffActive);
            stateMachine.AddTransition(buffAttack, idle, () => buffAttack.Ended);

            stateMachine.AddAnyTransition(death, () => !_orbMage.IsAlive);
        }
    }
}