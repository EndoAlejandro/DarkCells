using DarkHavoc.Enemies.Assassin.States;
using DarkHavoc.Enemies.CagedShocker.States;
using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.DaggerMush
{
    public class DaggerMushStateMachine : FiniteStateBehaviour
    {
        private DaggerMush _daggerMush;
        private Collider2D _collider;
        private EntityPathfinding _pathfinding;
        private EnemyAnimation _animation;

        protected override void References()
        {
            _daggerMush = GetComponent<DaggerMush>();
            _collider = GetComponent<Collider2D>();
            _pathfinding = GetComponent<EntityPathfinding>();
            _animation = GetComponentInChildren<EnemyAnimation>();
        }

        protected override void StateMachine()
        {
            var idle = new IdleState(_daggerMush);
            var patrol = new SideToSidePatrolState(_daggerMush, _collider);
            var chase = new PathChaseState(_daggerMush, _collider, _pathfinding,
                _daggerMush.HitBox, _daggerMush.SlashHitBox);
            var airChase = new AirChaseState(_daggerMush, _collider, _pathfinding);

            var lightTelegraph = new TelegraphState(_daggerMush, _daggerMush.HitBox, 1f);
            var slashTelegraph = new TelegraphState(_daggerMush, _daggerMush.SlashHitBox, 1f);

            var lightAttack = new EnemyAttackState(_daggerMush, _daggerMush.HitBox, _animation);
            var slashAttack = new EnemyDisplaceAttackState(_daggerMush, _collider,
                _daggerMush.SlashHitBox, _animation, isUnstoppable: true);

            var stun = new StunState(_daggerMush, _daggerMush.Stats.StunTime);
            var death = new EnemyDeathState(_daggerMush);

            stateMachine.SetState(idle);

            stateMachine.AddTransition(idle, patrol, () => idle.Ended && _daggerMush.Grounded);
            stateMachine.AddTransition(patrol, idle, () => patrol.Ended || !_daggerMush.Grounded);

            var toChaseStates = new IState[] { idle, patrol };
            stateMachine.AddManyTransitions(toChaseStates, chase, () => _daggerMush.Player != null && _daggerMush.Grounded);

            stateMachine.AddTransition(chase, airChase, () => !_daggerMush.Grounded);
            stateMachine.AddTransition(airChase, chase, () => _daggerMush.Grounded);

            stateMachine.AddTransition(chase, idle, () => _daggerMush.Player == null);

            stateMachine.AddTransition(chase, lightTelegraph, () => chase.FirstHitBoxAvailable);
            stateMachine.AddTransition(lightTelegraph, lightAttack, () => lightTelegraph.Ended);
            stateMachine.AddTransition(lightAttack, idle, () => lightAttack.Ended);

            stateMachine.AddTransition(lightAttack, stun, () => lightAttack.Blocked);
            stateMachine.AddTransition(stun, idle, () => stun.Ended);

            stateMachine.AddTransition(chase, slashTelegraph, () => chase.SecondHitBoxAvailable);
            stateMachine.AddTransition(slashTelegraph, slashAttack, () => slashTelegraph.Ended);
            stateMachine.AddTransition(slashAttack, idle, () => slashAttack.Ended);

            stateMachine.AddAnyTransition(death, () => !_daggerMush.IsAlive);
        }
    }
}