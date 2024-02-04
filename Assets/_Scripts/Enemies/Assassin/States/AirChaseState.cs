using DarkHavoc.PlayerComponents;
using DarkHavoc.Senses;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.Assassin.States
{
    public class AirChaseState : IState
    {
        public override string ToString() => "Air";
        public AnimationState AnimationState => AnimationState.Air;
        public bool CanTransitionToSelf => false;

        private readonly Assassin _assassin;
        private readonly Collider2D _collider;
        private readonly EntityPathfinding _pathfinding;

        private int _horizontalDirection;
        private WallResult _wallResult;

        public AirChaseState(Assassin assassin, Collider2D collider, EntityPathfinding pathfinding)
        {
            _assassin = assassin;
            _collider = collider;
            _pathfinding = pathfinding;
        }

        public void Tick()
        {
        }

        public void FixedTick()
        {
            _wallResult =
                EntityVision.CheckWallCollision(_collider, _assassin.Stats.WallDetection, _assassin.FacingLeft);

            _horizontalDirection = (int)Mathf.Sign(_pathfinding.Direction.x);
            if (Mathf.Abs(_pathfinding.Direction.x) > _assassin.Stats.StoppingDistance * .5f ||
                !_wallResult.FacingWall)
                _assassin.Move(_horizontalDirection);
            else
                _assassin.Move(0);
        }

        public void OnEnter() => _pathfinding.StartFindPath(_assassin.Player.transform);

        public void OnExit()
        {
        }
    }
}