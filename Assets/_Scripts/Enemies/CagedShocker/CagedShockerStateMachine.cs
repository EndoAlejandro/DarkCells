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
            var chase = new ChaseState(_cagedShocker, _cagedShocker.HitBox, _collider);
            var telegraph = new TelegraphState(_cagedShocker, _cagedShocker.HitBox, 1.25f);
            var firstAttack = new CagedShockerAttackState(_cagedShocker, _cagedShocker.HitBox, _animation, true,
                _cagedShocker.Stats.FirstAttackTime);
            var secondAttack = new CagedShockerAttackState(_cagedShocker, _cagedShocker.HitBox, _animation, false,
                _cagedShocker.Stats.SecondAttackTime);
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

            stateMachine.AddTransition(chase, telegraph, () => chase.AttackAvailable);
            stateMachine.AddTransition(telegraph, firstAttack, () => telegraph.Ended);
            stateMachine.AddTransition(firstAttack, secondAttack, () => firstAttack.CanCombo);

            stateMachine.AddTransition(firstAttack, stun, () => firstAttack.Stunned);
            stateMachine.AddTransition(secondAttack, stun, () => secondAttack.Stunned);

            stateMachine.AddTransition(firstAttack, rest, () => firstAttack.Ended);
            stateMachine.AddTransition(secondAttack, rest, () => secondAttack.Ended);

            stateMachine.AddTransition(stun, idle, () => stun.Ended);
            stateMachine.AddTransition(rest, idle, () => rest.Ended);

            stateMachine.AddAnyTransition(dead, () => !_cagedShocker.IsAlive);
        }
    }
}