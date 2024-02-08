using DarkHavoc.Enemies.Assassin.States;
using DarkHavoc.Enemies.CagedShocker.States;
using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Enemies.ShockSweeper
{
    public class ShockSweeperStateMachine : FiniteStateBehaviour
    {
        private ShockSweeper _shockSweeper;
        private Collider2D _collider;
        private EnemyAnimation _animation;
        private EntityPathfinding _pathfinding;

        protected override void References()
        {
            _shockSweeper = GetComponent<ShockSweeper>();
            _collider = GetComponent<Collider2D>();
            _animation = GetComponentInChildren<EnemyAnimation>();
            _pathfinding = GetComponent<EntityPathfinding>();
        }

        protected override void StateMachine()
        {
            var idle = new IdleState(_shockSweeper);
            var patrol = new SideToSidePatrolState(_shockSweeper, _collider);
            var chase = new ChasePathState(_shockSweeper, _collider, _pathfinding,
                _shockSweeper.HitBox, _shockSweeper.HeavyHitBox, _shockSweeper.StaticRangedHitBox);
            var airChase = new AirChaseState(_shockSweeper, _collider, _pathfinding);

            var lightTelegraph = new TelegraphState(_shockSweeper, _shockSweeper.HitBox, 1f);
            var heavyTelegraph = new TelegraphState(_shockSweeper, _shockSweeper.HeavyHitBox, 1f);
            var rangedTelegraph = new TelegraphState(_shockSweeper, _shockSweeper.StaticRangedHitBox, 1f);

            var lightAttack = new EnemyAttackState(_shockSweeper, _shockSweeper.HitBox,
                _animation, isUnstoppable: false);
            var heavyAttack = new EnemyAttackState(_shockSweeper, _shockSweeper.HeavyHitBox,
                _animation, isUnstoppable: true, AnimationState.HeavyAttack);
            var rangedAttack = new EnemyStaticRangedAttackState(_shockSweeper, _shockSweeper.StaticRangedHitBox,
                _animation, isUnstoppable: true);

            var stun = new StunState(_shockSweeper, _shockSweeper.Stats.StunTime);
            var death = new EnemyDeathState(_shockSweeper);

            stateMachine.SetState(idle);

            stateMachine.AddTransition(idle, patrol, () => idle.Ended && _shockSweeper.Grounded);
            stateMachine.AddTransition(patrol, idle, () => patrol.Ended || !_shockSweeper.Grounded);

            var toChaseStates = new IState[] { idle, patrol };
            stateMachine.AddManyTransitions(toChaseStates, chase, () => _shockSweeper.Player != null && _shockSweeper.Grounded);

            stateMachine.AddTransition(chase, airChase, () => !_shockSweeper.Grounded);
            stateMachine.AddTransition(airChase, chase, () => _shockSweeper.Grounded);

            stateMachine.AddTransition(chase, idle, () => _shockSweeper.Player == null);

            stateMachine.AddTransition(chase, lightTelegraph, () => chase.FirstHitBoxAvailable);
            stateMachine.AddTransition(lightTelegraph, lightAttack, () => lightTelegraph.Ended);
            stateMachine.AddTransition(lightAttack, idle, () => lightAttack.Ended);

            stateMachine.AddTransition(lightAttack, stun, () => lightAttack.Blocked);
            stateMachine.AddTransition(stun, idle, () => stun.Ended);

            stateMachine.AddTransition(chase, heavyTelegraph, () => chase.SecondHitBoxAvailable);
            stateMachine.AddTransition(heavyTelegraph, heavyAttack, () => heavyTelegraph.Ended);
            stateMachine.AddTransition(heavyAttack, idle, () => heavyAttack.Ended);
            
            stateMachine.AddTransition(chase, rangedAttack, () => chase.ThirdHitBoxAvailable);
            stateMachine.AddTransition(rangedTelegraph, rangedAttack, () => rangedTelegraph.Ended);
            stateMachine.AddTransition(rangedAttack, idle, () => rangedAttack.Ended);

            stateMachine.AddAnyTransition(death, () => !_shockSweeper.IsAlive);
        }
    }
}