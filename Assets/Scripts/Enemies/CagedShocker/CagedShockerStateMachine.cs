using DarkHavoc.Enemies.CagedShocker.States;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.CagedShocker
{
    public class CagedShockerStateMachine : FiniteStateBehaviour
    {
        private CagedShockerAnimation _animation;
        private CagedShocker _cagedShocker;
        private EnemyAttack _attack;
        private Rigidbody2D _rigidbody;
        private Collider2D _collider;

        protected override void References()
        {
            _animation = GetComponentInChildren<CagedShockerAnimation>();
            _cagedShocker = GetComponent<Enemies.CagedShocker.CagedShocker>();
            _attack = GetComponent<EnemyAttack>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }

        protected override void StateMachine()
        {
            var idle = new IdleState(_cagedShocker, _rigidbody);
            var patrol = new PatrolState(_cagedShocker, _rigidbody, _collider);
            var chase = new ChaseState(_cagedShocker, _attack, _rigidbody, _collider);
            var telegraph = new TelegraphState(_cagedShocker, _cagedShocker.Stats.TelegraphTime);
            var firstAttack = new AttackState(_cagedShocker, _attack, _animation, true,
                _cagedShocker.Stats.FirstAttackTime);
            var secondAttack = new AttackState(_cagedShocker, _attack, _animation, false,
                _cagedShocker.Stats.SecondAttackTime);
            var rest = new RestState(_cagedShocker.Stats.RestTime);
            var stun = new StunState(_cagedShocker.Stats.StunTime);
            var dead = new DeadState(_cagedShocker);

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