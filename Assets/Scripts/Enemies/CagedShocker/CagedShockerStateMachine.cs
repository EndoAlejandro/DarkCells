using DarkHavoc.Enemies.CagedShocker.States;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.CagedShocker
{
    public class CagedShockerStateMachine : FiniteStateBehaviour
    {
        private CagedShockerAnimation _animation;
        private CagedShocker _cagedShocker;
        private EnemyHitBox _hitBox;
        private Collider2D _collider;

        protected override void References()
        {
            _animation = GetComponentInChildren<CagedShockerAnimation>();
            _hitBox = GetComponentInChildren<EnemyHitBox>();
            _cagedShocker = GetComponent<CagedShocker>();
            _collider = GetComponent<Collider2D>();
        }

        protected override void StateMachine()
        {
            var idle = new IdleState(_cagedShocker);
            var patrol = new PatrolState(_cagedShocker, _collider);
            var chase = new ChaseState(_cagedShocker, _hitBox, _collider);
            var telegraph = new TelegraphState(_cagedShocker, _cagedShocker.Stats.TelegraphTime);
            var firstAttack = new AttackState(_cagedShocker, _hitBox, _animation, true,
                _cagedShocker.Stats.FirstAttackTime);
            var secondAttack = new AttackState(_cagedShocker, _hitBox, _animation, false,
                _cagedShocker.Stats.SecondAttackTime);
            var rest = new RestState(_cagedShocker, _cagedShocker.Stats.RestTime);
            var stun = new StunState(_cagedShocker, _cagedShocker.Stats.StunTime);
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