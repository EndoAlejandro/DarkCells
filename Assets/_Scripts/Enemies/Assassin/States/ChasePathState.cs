using DarkHavoc.PlayerComponents;
using DarkHavoc.Senses;
using DarkHavoc.StateMachineComponents;
using UnityEngine;

namespace DarkHavoc.Enemies.Assassin.States
{
    public class ChasePathState : IState
    {
        public override string ToString() => "Chase Path";
        public AnimationState AnimationState => AnimationState.Ground;
        public bool CanTransitionToSelf => false;
        public bool Ended => _timer <= 0f;

        private readonly Assassin _assassin;
        private readonly Player _player;
        private readonly Collider2D _collider;
        private readonly EntityPathfinding _pathfinding;

        private WallResult _wallResult;

        private int _horizontalDirection;
        private float _timer;

        public ChasePathState(Assassin assassin, Player player, Collider2D collider, EntityPathfinding pathfinding)
        {
            _assassin = assassin;
            _player = player;
            _collider = collider;
            _pathfinding = pathfinding;
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;

            if (_horizontalDirection < 0 && !_assassin.FacingLeft) _assassin.SetFacingLeft(true);
            else if (_horizontalDirection > 0 && _assassin.FacingLeft) _assassin.SetFacingLeft(false);
        }

        private bool HeightJump() => _pathfinding.Direction.y < 1.5f;
        private bool JumpFirstCase() => _wallResult.FacingWall && HeightJump();
        private bool JumpSecondCase() => _assassin.LedgeInFront;

        public void FixedTick()
        {
            _wallResult = EntityVision.CheckWallCollision
                (_collider, _assassin.Stats.WallDetection, _assassin.FacingLeft);
            
            if ((JumpFirstCase() || JumpSecondCase())
                && !_wallResult.TopCheck && _assassin.Grounded)
            {
                _assassin.Jump();
            }
            
            PathfindingMovement();
        }


        private void PathfindingMovement()
        {
            if (_pathfinding.NextNode == null)
                _horizontalDirection = (int)(_player.transform.position.x - _assassin.transform.position.x);
            else
                _horizontalDirection = (int)Mathf.Sign(_pathfinding.Direction.x);

            if (Mathf.Abs(_pathfinding.Direction.x) > _assassin.Stats.StoppingDistance &&
                !_wallResult.MidCheck && !_assassin.LedgeInFront)
                _assassin.Move(_horizontalDirection);
            else
                _assassin.Move(0);
        }

        public void OnEnter()
        {
            _timer = 0f;
            _pathfinding.StartFindPath(_player.transform);
        }

        public void OnExit()
        {
        }
    }
}