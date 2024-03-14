using DarkHavoc.Enemies.CagedShocker.States;
using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.Summoner
{
    public class SummonerStateMachine : FiniteStateBehaviour
    {
        private Summoner _summoner;
        private Collider2D _collider;
        private EnemyAnimation _animation;

        protected override void References()
        {
            _summoner = GetComponent<Summoner>();
            _collider = GetComponent<Collider2D>();
            _animation = GetComponentInChildren<EnemyAnimation>();
        }

        protected override void StateMachine()
        {
            var idle = new IdleState(_summoner);
            var patrol = new SideToSidePatrolState(_summoner, _collider);
            var chase = new ChaseSideToSideState(_summoner, _collider, _summoner.HitBox);

            var rangedAttack = new RangedSummonState(_summoner, _summoner.HitBox, _animation, true);

            var death = new EnemyDeathState(_summoner);

            stateMachine.SetState(idle);

            stateMachine.AddTransition(idle, patrol, () => idle.Ended && _summoner.Grounded);
            stateMachine.AddTransition(patrol, idle, () => patrol.Ended || !_summoner.Grounded);
            stateMachine.AddTransition(patrol, chase, () => _summoner.Player);

            stateMachine.AddTransition(chase, rangedAttack, () => chase.FirstHitBoxAvailable);
            stateMachine.AddTransition(rangedAttack, idle, () => rangedAttack.Ended);

            stateMachine.AddAnyTransition(death, () => !_summoner.IsAlive);
        }
    }
}