using UnityEngine;
using DarkHavoc.Enemies.CagedShocker.States;
using DarkHavoc.Enemies.OrbMage.States;
using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.StateMachineComponents;

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
            var chase = new ChaseSideToSideState(_orbMage, _collider, _orbMage.HitBox);

            var telegraph = new TelegraphState(_orbMage, _orbMage.HitBox, 1f);
            var lightAttack = new EnemyAttackState(_orbMage, _orbMage.HitBox, _animation);
            var buffAttack = new BuffAttackState(_orbMage, _animation, 5f);

            var death = new EnemyDeathState(_orbMage);

            stateMachine.SetState(idle);

            stateMachine.AddTransition(idle, patrol, () => idle.Ended && _orbMage.Grounded);
            stateMachine.AddTransition(patrol, idle, () => patrol.Ended || !_orbMage.Grounded);

            stateMachine.AddTransition(patrol, chase, () => _orbMage.Player);
            stateMachine.AddTransition(chase, telegraph, () => chase.FirstHitBoxAvailable && chase.IsPlayerVisible);
            stateMachine.AddTransition(telegraph, lightAttack, () => telegraph.Ended);

            stateMachine.AddTransition(lightAttack, idle, () => lightAttack.Ended);

            stateMachine.AddAnyTransition(death, () => !_orbMage.IsAlive);
        }
    }
}