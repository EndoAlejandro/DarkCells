using DarkHavoc.Enemies.CagedShocker.States;
using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Enemies.Ghoul
{
    public class GhoulStateMachine : FiniteStateBehaviour
    {
        private Ghoul _ghoul;
        private Collider2D _collider;
        private EnemyAnimation _animation;

        protected override void References()
        {
            _ghoul = GetComponent<Ghoul>();
            _collider = GetComponent<Collider2D>();
            _animation = GetComponentInChildren<EnemyAnimation>();
        }

        protected override void StateMachine()
        {
            var spawn = new AnimationOnlyState(2f, AnimationState.Awake);
            var idle = new IdleState(_ghoul);
            var chase = new ChaseSideToSideState(_ghoul, _collider, _ghoul.HitBox);
            var telegraph = new TelegraphState(_ghoul, _ghoul.HitBox, 1f);
            var attack = new EnemyAttackState(_ghoul, _ghoul.HitBox, _animation, true);

            var death = new EnemyDeathState(_ghoul);

            stateMachine.SetState(spawn);

            stateMachine.AddTransition(spawn, idle, () => spawn.Ended);
            stateMachine.AddTransition(idle, chase, () => _ghoul.Player != null && _ghoul.Grounded && idle.Ended);
            stateMachine.AddTransition(chase, telegraph, () => chase.FirstHitBoxAvailable);

            stateMachine.AddTransition(telegraph, attack, () => telegraph.Ended);
            stateMachine.AddTransition(attack, idle, () => attack.Ended);

            stateMachine.AddAnyTransition(death, () => !_ghoul.IsAlive);
        }
    }
}