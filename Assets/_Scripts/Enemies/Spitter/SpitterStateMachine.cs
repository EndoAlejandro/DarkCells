using DarkHavoc.Enemies.BombDroid;
using DarkHavoc.Enemies.CagedShocker.States;
using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.Spitter
{
    public class SpitterStateMachine : FiniteStateBehaviour
    {
        private Spitter _spitter;
        private Collider2D _collider;
        private EnemyAnimation _animation;

        protected override void References()
        {
            _spitter = GetComponent<Spitter>();
            _collider = GetComponent<Collider2D>();
            _animation = GetComponentInChildren<EnemyAnimation>();
        }

        protected override void StateMachine()
        {
            var idle = new IdleState(_spitter);
            var patrol = new SideToSidePatrolState(_spitter, _collider);
            var chase = new ChaseSideToSideState(_spitter, _collider, _spitter.HitBox);
            var telegraph = new TelegraphState(_spitter, _spitter.HitBox, 1f);
            var attack = new EnemyAttackState(_spitter, _spitter.HitBox, _animation);

            var death = new EnemyDeathState(_spitter);

            stateMachine.SetState(idle);

            stateMachine.AddTransition(idle, patrol, () => idle.Ended && _spitter.Grounded);
            stateMachine.AddTransition(patrol, idle, () => patrol.Ended || !_spitter.Grounded);

            var toChase = new IState[] { idle, patrol };
            stateMachine.AddManyTransitions(toChase, chase, () => _spitter.Player != null && _spitter.Grounded);
            stateMachine.AddTransition(chase, idle, () => _spitter.Player == null);

            stateMachine.AddTransition(chase, telegraph, () => chase.FirstHitBoxAvailable);
            stateMachine.AddTransition(telegraph, attack, () => telegraph.Ended);
            stateMachine.AddTransition(attack, idle, () => attack.Ended);

            stateMachine.AddAnyTransition(death, () => !_spitter.IsAlive);
        }
    }
}