using DarkHavoc.Enemies.CagedShocker.States;
using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.CagedShocker
{
    public class CagedShockerStateMachine : FiniteStateBehaviour
    {
        private EnemyAnimation _animation;
        private CagedShocker _cagedShocker;
        private Collider2D _collider;

        protected override void References()
        {
            _animation = GetComponentInChildren<EnemyAnimation>();
            _cagedShocker = GetComponent<CagedShocker>();
            _collider = GetComponent<Collider2D>();
        }

        protected override void StateMachine()
        {
            var idle = new IdleState(_cagedShocker);
            var patrol = new SideToSidePatrolState(_cagedShocker, _collider);
            var chase = new ChaseSideToSideState(_cagedShocker, _collider, _cagedShocker.HitBox);
            var telegraph = new TelegraphState(_cagedShocker, _cagedShocker.HitBox, 1.25f);
            var attack = new EnemyAttackState(_cagedShocker, _cagedShocker.HitBox, _animation);
            var rest = new RestState(_cagedShocker, _cagedShocker.Stats.RestTime);
            var stun = new StunState(_cagedShocker, _cagedShocker.Stats.StunTime);
            var dead = new EnemyDeathState(_cagedShocker);

            // Initial State.
            stateMachine.SetState(idle);

            stateMachine.AddTransition(idle, patrol, () => idle.Ended && _cagedShocker.Grounded);
            stateMachine.AddTransition(patrol, idle, () => patrol.Ended || !_cagedShocker.Grounded);

            var toChaseStates = new IState[] { idle, patrol };
            stateMachine.AddManyTransitions(toChaseStates, chase, () => _cagedShocker.Player != null);
            stateMachine.AddTransition(chase, idle, () => _cagedShocker.Player == null);

            stateMachine.AddTransition(chase, telegraph, () => chase.FirstHitBoxAvailable && chase.IsPlayerVisible);
            stateMachine.AddTransition(telegraph, attack, () => telegraph.Ended);

            stateMachine.AddTransition(attack, stun, () => attack.Blocked);
            stateMachine.AddTransition(attack, idle, () => attack.Ended);

            stateMachine.AddTransition(stun, idle, () => stun.Ended);
            stateMachine.AddTransition(rest, idle, () => rest.Ended);

            stateMachine.AddAnyTransition(dead, () => !_cagedShocker.IsAlive);
        }
    }
}