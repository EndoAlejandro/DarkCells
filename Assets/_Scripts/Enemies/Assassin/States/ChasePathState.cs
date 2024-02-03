using Calcatz.MeshPathfinding;
using DarkHavoc.PlayerComponents;
using DarkHavoc.Senses;
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
        private readonly Collider2D _collider;
        private readonly Pathfinding _pathfinding;

        private WallResult _wallResult;

        private Node _nextNode;
        private int _horizontalDirection;

        public ChasePathState(Assassin assassin, Player player, Collider2D collider, Pathfinding pathfinding)
        {
            _assassin = assassin;
            _player = player;
            _collider = collider;
            _pathfinding = pathfinding;
        }

        public void Tick()
        {
            if (_horizontalDirection < 0 && !_assassin.FacingLeft) _assassin.SetFacingLeft(true);
            else if (_horizontalDirection > 0 && _assassin.FacingLeft) _assassin.SetFacingLeft(false);

            if ((_wallResult.FacingWall || _assassin.LedgeInFront)
                && !_wallResult.TopCheck && _assassin.Grounded &&
                _nextNode.transform.position.y >= _assassin.transform.position.y)
            {
                _assassin.Jump();
            }
        }

        public void FixedTick()
        {
            _wallResult = EntityVision.CheckWallCollision
                (_collider, _assassin.Stats.WallDetection, _assassin.FacingLeft);
            PathfindingMovement();
        }

        private void PathfindingMovement()
        {
            Node[] path = _pathfinding.GetPathResult();
            if (path is not { Length: > 0 }) return;

            _nextNode = path[0];
            var direction = _nextNode.transform.position - _assassin.transform.position;

            _horizontalDirection = (int)Mathf.Sign(direction.x);
            float heightDifference = Mathf.Abs(direction.y);

            if (Mathf.Abs(direction.x) > _assassin.Stats.StoppingDistance ||
                !(heightDifference > 2f && _wallResult.FacingWall))
                _assassin.Move(_horizontalDirection);
            else
                _assassin.Move(0);
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