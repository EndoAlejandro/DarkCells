using DarkHavoc.PlayerComponents;
using DarkHavoc.Senses;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Enemies.Assassin.States
{
    public class AirChaseState : IState
    {
        public override string ToString() => "Air";
        public AnimationState AnimationState => AnimationState.Air;
        public bool CanTransitionToSelf => false;

        private readonly Enemy _enemy;
        private readonly Collider2D _collider;
        private readonly EntityPathfinding _pathfinding;

        private int _horizontalDirection;
        private WallResult _wallResult;

        public AirChaseState(Enemy enemy, Collider2D collider, EntityPathfinding pathfinding)
        {
            _enemy = enemy;
            _collider = collider;
            _pathfinding = pathfinding;
        }

        public void Tick()
        {
            if (_enemy.FacingLeft && _horizontalDirection > 0f) _enemy.SetFacingLeft(false);
            if (!_enemy.FacingLeft && _horizontalDirection < 0f) _enemy.SetFacingLeft(true);
        }

        public void FixedTick()
        {
            _wallResult =
                EntityVision.CheckWallCollision(_collider, _enemy.Stats.WallDetection, _enemy.FacingLeft);

            _horizontalDirection = (int)Mathf.Sign(_pathfinding.Direction.x);
            if (Mathf.Abs(_pathfinding.Direction.x) > _enemy.Stats.StoppingDistance * .5f ||
                !_wallResult.FacingWall)
                _enemy.Move(_horizontalDirection);
            else
                _enemy.Move(0);
        }

        public void OnEnter() => _pathfinding.StartFindPath(_enemy.Player.transform);

        public void OnExit()
        {
        }
    }
}