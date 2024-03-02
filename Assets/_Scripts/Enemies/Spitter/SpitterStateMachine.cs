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

            var death = new EnemyDeathState(_spitter);

            stateMachine.SetState(idle);

            stateMachine.AddTransition(idle, patrol, () => idle.Ended && _spitter.Grounded);
            stateMachine.AddTransition(patrol, idle, () => patrol.Ended || !_spitter.Grounded);

            stateMachine.AddAnyTransition(death, () => !_spitter.IsAlive);
        }
    }
}