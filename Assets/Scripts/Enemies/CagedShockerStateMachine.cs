using DarkHavoc.Enemies.CagedShockerStates;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies
{
    public class CagedShockerStateMachine : FiniteStateBehaviour
    {
        private CagedShockerAnimation _animation;
        private CagedShocker _cagedShocker;
        private Rigidbody2D _rigidbody;

        protected override void References()
        {
            _animation = GetComponentInChildren<CagedShockerAnimation>();
            _cagedShocker = GetComponent<CagedShocker>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        protected override void StateMachine()
        {
            var idle = new IdleState(_cagedShocker, _rigidbody);
            var patrol = new PatrolState(_cagedShocker, _rigidbody);
            var chase = new ChaseState(_cagedShocker, _rigidbody);
            var firstAttack = new AttackState(_cagedShocker, _animation, true, _cagedShocker.Stats.FirstAttackTime);
            var secondAttack = new AttackState(_cagedShocker, _animation, false, _cagedShocker.Stats.SecondAttackTime);
            var rest = new RestState(_cagedShocker.Stats.RestTime);
            var dead = new DeadState(_cagedShocker);

            // Initial State.
            stateMachine.SetState(idle);

            stateMachine.AddTransition(idle, patrol, () => idle.Ended && _cagedShocker.Grounded);
            stateMachine.AddTransition(patrol, idle, () => patrol.Ended || !_cagedShocker.Grounded);

            var toChaseStates = new IState[] { idle, patrol };
            stateMachine.AddManyTransitions(toChaseStates, chase, () => _cagedShocker.Player != null);
            stateMachine.AddTransition(chase, idle, () => _cagedShocker.Player == null);

            stateMachine.AddTransition(chase, firstAttack, () => chase.AttackAvailable && _cagedShocker != null);
            stateMachine.AddTransition(firstAttack, secondAttack, () => firstAttack.CanCombo);

            stateMachine.AddTransition(firstAttack, rest, () => firstAttack.Ended);
            stateMachine.AddTransition(secondAttack, rest, () => secondAttack.Ended);
            stateMachine.AddTransition(rest, idle, () => rest.Ended);

            stateMachine.AddAnyTransition(dead, () => !_cagedShocker.IsAlive);
        }
    }
}