using DarkHavoc.Enemies.CagedShocker.States;
using DarkHavoc.Enemies.CagedSpider;
using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.DarkWarden
{
    public class DarkWardenStateMachine : FiniteStateBehaviour
    {
        private DarkWarden _darkWarden;
        private Collider2D _collider;
        private EnemyAnimation _animation;

        protected override void References()
        {
            _darkWarden = GetComponent<DarkWarden>();
            _collider = GetComponent<Collider2D>();
            _animation = GetComponentInChildren<EnemyAnimation>();
        }

        protected override void StateMachine()
        {
            var idle = new IdleState(_darkWarden);
            var patrol = new SideToSidePatrolState(_darkWarden, _collider);
            var chase = new ChaseSideToSideState(_darkWarden, _collider, _darkWarden.HitBox);
            var telegraph = new TelegraphState(_darkWarden, _darkWarden.HitBox, .5f);
            var attack = new EnemyAttackState(_darkWarden, _darkWarden.HitBox, _animation, isUnstoppable: true);
            var death = new EnemyDeathState(_darkWarden);

            stateMachine.SetState(idle);

            stateMachine.AddTransition(idle, patrol, () => idle.Ended && _darkWarden.Grounded);
            stateMachine.AddTransition(patrol, idle, () => patrol.Ended || !_darkWarden.Grounded);

            var toChaseStates = new IState[] { idle, patrol };
            stateMachine.AddManyTransitions(toChaseStates, chase, () => _darkWarden.Player != null);
            stateMachine.AddTransition(chase, idle, () => _darkWarden.Player == null);

            stateMachine.AddTransition(chase, telegraph, () => chase.FirstHitBoxAvailable && chase.IsPlayerVisible);
            stateMachine.AddTransition(telegraph, attack, () => telegraph.Ended);
            stateMachine.AddTransition(attack, idle, () => attack.Ended);

            stateMachine.AddAnyTransition(death, () => !_darkWarden.IsAlive);
        }
    }
}