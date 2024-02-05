using Calcatz.MeshPathfinding;
using DarkHavoc.Enemies.Assassin.States;
using DarkHavoc.Enemies.CagedShocker.States;
using DarkHavoc.Enemies.CagedSpider;
using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.PlayerComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.Assassin
{
    public class AssassinStateMachine : FiniteStateBehaviour
    {
        // For testing only, delete later.
        [SerializeField] private Player player;

        private Assassin _assassin;
        private AssassinAnimation _animation;
        private Collider2D _collider;
        private EntityPathfinding _pathfinding;

        protected override void References()
        {
            _assassin = GetComponent<Assassin>();
            _animation = GetComponentInChildren<AssassinAnimation>();
            _collider = GetComponent<Collider2D>();
            _pathfinding = GetComponent<EntityPathfinding>();
        }

        protected override void StateMachine()
        {
            var idle = new IdleState(_assassin);
            var patrol = new SideToSidePatrolState(_assassin, _collider);
            var chase = new ChasePathState(_assassin, _collider, _pathfinding,
                _assassin.HitBox, _assassin.SlashHitBox);
            var airChase = new AirChaseState(_assassin, _collider, _pathfinding);

            var lightTelegraph = new TelegraphState(_assassin, _assassin.HitBox, 1f);
            var slashTelegraph = new TelegraphState(_assassin, _assassin.SlashHitBox, 1f);

            var lightAttack = new EnemyAttackState(_assassin, _assassin.HitBox, _animation);
            var slashAttack = new EnemyDisplaceAttackState(_assassin, _collider,
                _assassin.SlashHitBox, _animation, isUnstoppable: true);

            var stun = new StunState(_assassin, _assassin.Stats.StunTime);
            var death = new EnemyDeathState(_assassin);

            stateMachine.SetState(idle);

            stateMachine.AddTransition(idle, patrol, () => idle.Ended && _assassin.Grounded);
            stateMachine.AddTransition(patrol, idle, () => patrol.Ended || !_assassin.Grounded);

            var toChaseStates = new IState[] { idle, patrol };
            stateMachine.AddManyTransitions(toChaseStates, chase, () => _assassin.Player != null && _assassin.Grounded);

            stateMachine.AddTransition(chase, airChase, () => !_assassin.Grounded);
            stateMachine.AddTransition(airChase, chase, () => _assassin.Grounded);

            stateMachine.AddTransition(chase, idle, () => _assassin.Player == null);

            stateMachine.AddTransition(chase, lightTelegraph, () => chase.FirstHitBoxAvailable);
            stateMachine.AddTransition(lightTelegraph, lightAttack, () => lightTelegraph.Ended);
            stateMachine.AddTransition(lightAttack, idle, () => lightAttack.Ended);

            stateMachine.AddTransition(lightAttack, stun, () => lightAttack.Blocked);
            stateMachine.AddTransition(stun, idle, () => stun.Ended);

            stateMachine.AddTransition(chase, slashTelegraph, () => chase.SecondHitBoxAvailable);
            stateMachine.AddTransition(slashTelegraph, slashAttack, () => slashTelegraph.Ended);
            stateMachine.AddTransition(slashAttack, idle, () => slashAttack.Ended);

            stateMachine.AddAnyTransition(death, () => !_assassin.IsAlive);
        }
    }
}