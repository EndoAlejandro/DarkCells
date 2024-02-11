using DarkHavoc.Senses;
using DarkHavoc.StateMachineComponents;
using UnityEngine;
using AnimationState = DarkHavoc.StateMachineComponents.AnimationState;

namespace DarkHavoc.Enemies.SharedStates
{
    public class PathChaseState : MultiHitboxState, IState
    {
        public override string ToString() => "Chase Path";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => false;

        private readonly Enemy _enemy;
        private readonly Collider2D _collider;
        private readonly EntityPathfinding _pathfinding;

        private WallResult _wallResult;
        private int _horizontalDirection;

        public PathChaseState(Enemy enemy, Collider2D collider, EntityPathfinding pathfinding,
            EnemyHitBox firstHitBox = null, EnemyHitBox secondHitBox = null, EnemyHitBox thirdHitBox = null) : base(
            firstHitBox, secondHitBox, thirdHitBox)
        {
            _enemy = enemy;
            _collider = collider;
            _pathfinding = pathfinding;
        }

        public void Tick()
        {
            if (_horizontalDirection < 0 && !_enemy.FacingLeft) _enemy.SetFacingLeft(true);
            else if (_horizontalDirection > 0 && _enemy.FacingLeft) _enemy.SetFacingLeft(false);

            HitBoxCheck();
        }

        private bool HeightJump() => _pathfinding.Direction.y > .5f;
        private bool JumpFirstCase() => _wallResult.FacingWall && HeightJump();
        private bool JumpSecondCase() => _enemy.LedgeInFront;

        public void FixedTick()
        {
            _wallResult = EntityVision.CheckWallCollision
                (_collider, _enemy.Stats.WallDetection, _enemy.FacingLeft);

            if ((JumpFirstCase() || JumpSecondCase())
                && !_wallResult.TopCheck && _enemy.Grounded)
            {
                _enemy.Jump(_pathfinding.Direction.y <= -.5f);
            }

            PathfindingMovement();
        }

        private void PathfindingMovement()
        {
            _horizontalDirection = (int)Mathf.Sign(_pathfinding.Direction.x);

            if (Mathf.Abs(_pathfinding.Direction.x) > _enemy.Stats.StoppingDistance &&
                !_wallResult.MidCheck && !_enemy.LedgeInFront)
                _enemy.Move(_horizontalDirection);
            else
                _enemy.Move(0);
        }

        public void OnEnter()
        {
            if (_enemy.Player != null) _pathfinding.StartFindPath(_enemy.Player.transform);
        }

        public void OnExit()
        {
        }
    }
}