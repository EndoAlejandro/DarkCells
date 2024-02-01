using Calcatz.MeshPathfinding;
using DarkHavoc.PlayerComponents;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.Assassin
{
    public class ChasePathState : IState
    {
        public override string ToString() => "Chase Path";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => false;

        private readonly Assassin _assassin;
        private readonly Player _player;
        private readonly Pathfinding _pathfinding;

        public ChasePathState(Assassin assassin, Player player, Pathfinding pathfinding)
        {
            _assassin = assassin;
            _player = player;
            _pathfinding = pathfinding;
        }

        public void Tick()
        {
        }

        public void FixedTick()
        {
            Node[] path = _pathfinding.GetPathResult();
            if (path == null) return;

            if (path.Length > 0)
            {
                var direction = path[0].transform.position - _assassin.transform.position;

                if (Mathf.Abs(direction.x) > _assassin.Stats.StoppingDistance)
                    _assassin.Move((int)Mathf.Sign(direction.x));
                else 
                    _assassin.Move(0);
            }
        }

        public void OnEnter()
        {
            _pathfinding.StartFindPath(_player.transform, 1f, true);
        }

        public void OnExit()
        {
        }
    }
}