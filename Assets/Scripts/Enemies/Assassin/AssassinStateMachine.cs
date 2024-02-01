using Calcatz.MeshPathfinding;
using DarkHavoc.Enemies.CagedShocker.States;
using DarkHavoc.PlayerComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.Assassin
{
    public class AssassinStateMachine : FiniteStateBehaviour
    {
        // For testing only, delete later.
        [SerializeField] private Player player;

        private Assassin _assassin;
        private Collider2D _collider;
        private Pathfinding _pathfinding;

        protected override void References()
        {
            _assassin = GetComponent<Assassin>();
            _collider = GetComponent<Collider2D>();
            _pathfinding = GetComponent<Pathfinding>();
        }

        protected override void StateMachine()
        {
            var idle = new IdleState(_assassin);
            var patrol = new SideToSidePatrolState(_assassin, _collider);
            var chase = new ChasePathState(_assassin,player, _pathfinding);

            stateMachine.SetState(chase);

            stateMachine.AddTransition(idle, patrol, () => idle.Ended && _assassin.Grounded);
            stateMachine.AddTransition(patrol, idle, () => patrol.Ended || !_assassin.Grounded);
        }
    }
}