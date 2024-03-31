using DarkHavoc.Enemies.CagedShocker.States;
using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.Fx;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.HeavySlicer
{
    public class HeavySlicerStateMachine : FiniteStateBehaviour
    {
        private HeavySlicer _heavySlicer;
        private Collider2D _collider;
        private EnemyAnimation _animation;

        protected override void References()
        {
            _heavySlicer = GetComponent<HeavySlicer>();
            _collider = GetComponent<Collider2D>();
            _animation = GetComponentInChildren<EnemyAnimation>();
        }

        protected override void StateMachine()
        {
            var idle = new IdleState(_heavySlicer);
            var patrol = new SideToSidePatrolState(_heavySlicer, _collider);
            var chase = new ChaseSideToSideState(_heavySlicer, _collider, _heavySlicer.HitBox);
            var telegraph = new TelegraphState(_heavySlicer, _heavySlicer.HitBox, 1.5f);
            var attack = new EnemyAttackFxState(_heavySlicer, _heavySlicer.HitBox, _animation, new[]
                { FxType.HeavySlicer1, FxType.HeavySlicer2 }, true);
            var death = new EnemyDeathState(_heavySlicer);

            stateMachine.SetState(idle);

            stateMachine.AddTransition(idle, patrol, () => idle.Ended && _heavySlicer.Grounded);
            stateMachine.AddTransition(patrol, idle, () => patrol.Ended || !_heavySlicer.Grounded);
            stateMachine.AddTransition(patrol, chase, () => _heavySlicer.Player != null);

            stateMachine.AddTransition(chase, telegraph, () => chase.FirstHitBoxAvailable);
            stateMachine.AddTransition(telegraph, attack, () => telegraph.Ended);
            stateMachine.AddTransition(attack, idle, () => attack.Ended);

            stateMachine.AddAnyTransition(death, () => !_heavySlicer.IsAlive);
        }
    }
}