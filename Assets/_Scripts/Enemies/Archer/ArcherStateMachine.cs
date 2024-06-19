using DarkHavoc.Enemies.Archer.States;
using DarkHavoc.Enemies.CagedShocker.States;
using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.Archer
{
    public class ArcherStateMachine : FiniteStateBehaviour
    {
        private Archer _archer;
        private Collider2D _collider;
        private EnemyAnimation _animation;

        protected override void References()
        {
            _archer = GetComponent<Archer>();
            _collider = GetComponent<Collider2D>();
            _animation = GetComponentInChildren<EnemyAnimation>();
        }

        protected override void StateMachine()
        {
            var idle = new IdleState(_archer);
            var patrol = new SideToSidePatrolState(_archer, _collider);

            var chase = new ChaseSideToSideState(_archer, _collider, _archer.HitBox);
            var telegraph = new TelegraphState(_archer, _archer.HitBox, 1f);
            var lightAttack = new ArcherAttackState(_archer, _archer.HitBox, _animation, isUnstoppable: true);

            var death = new EnemyDeathState(_archer);

            stateMachine.SetState(idle);

            stateMachine.AddTransition(idle, patrol, () => idle.Ended && _archer.Grounded);
            stateMachine.AddTransition(patrol, idle, () => patrol.Ended || !_archer.Grounded);

            var toChase = new IState[] { idle, patrol };
            stateMachine.AddManyTransitions(toChase, chase, () => _archer.Player != null);
            stateMachine.AddTransition(chase, telegraph, () => chase.FirstHitBoxAvailable);
            stateMachine.AddTransition(telegraph, lightAttack, () => telegraph.Ended);

            stateMachine.AddTransition(lightAttack, idle, () => lightAttack.Ended);

            stateMachine.AddAnyTransition(death, () => !_archer.IsAlive);
        }
    }
}