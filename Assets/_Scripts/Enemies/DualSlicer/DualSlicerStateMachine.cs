using DarkHavoc.Enemies.CagedShocker.States;
using DarkHavoc.Enemies.SharedStates;
using DarkHavoc.Fx;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.DualSlicer
{
    public class DualSlicerStateMachine : FiniteStateBehaviour
    {
        private DualSlicer _dualSlicer;
        private Collider2D _collider;
        private EnemyAnimation _animation;

        protected override void References()
        {
            _dualSlicer = GetComponent<DualSlicer>();
            _collider = GetComponent<Collider2D>();
            _animation = GetComponentInChildren<EnemyAnimation>();
        }

        protected override void StateMachine()
        {
            var idle = new IdleState(_dualSlicer);
            var patrol = new SideToSidePatrolState(_dualSlicer, _collider);
            var chase = new ChaseSideToSideState(_dualSlicer, _collider, _dualSlicer.HitBox);
            var telegraph = new TelegraphState(_dualSlicer, _dualSlicer.HitBox, 1f);
            var attack = new EnemyAttackFxState(_dualSlicer, _dualSlicer.HitBox, _animation,
                new[] { EnemyFx.DualSlicer1, EnemyFx.DualSlicer2, EnemyFx.DualSlicer3 });
            var death = new EnemyDeathState(_dualSlicer);

            stateMachine.SetState(idle);

            stateMachine.AddTransition(idle, patrol, () => idle.Ended && _dualSlicer.Grounded);
            stateMachine.AddTransition(patrol, idle, () => patrol.Ended || !_dualSlicer.Grounded);
            stateMachine.AddTransition(patrol, chase, () => _dualSlicer.Player != null);

            stateMachine.AddTransition(chase, telegraph, () => chase.FirstHitBoxAvailable);
            stateMachine.AddTransition(telegraph, attack, () => telegraph.Ended);
            stateMachine.AddTransition(attack, idle, () => attack.Ended);

            stateMachine.AddAnyTransition(death, () => !_dualSlicer.IsAlive);
        }
    }
}